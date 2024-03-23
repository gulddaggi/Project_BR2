using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Harpy : MonoBehaviour
{
    public Transform AttackPO1;
    public Transform AttackPO2;

    public GameObject Harpy_point;
    public GameObject Harpy_Big_Projecter;

    float maxHP;

    Vector3 LookVec;
    Vector3 TauntVec;
    [SerializeField] bool isLook;
    [SerializeField] bool isOverdriving = false; // 보스 폭주 패턴 돌입 체크

    public float Boss_Pattern_Waiting_Time = 3.0f;
    public UnityEvent<float, float> OnBossHPUpdated;
    public BossText bossText;

    [SerializeField]
    private GameObject bossHPUI;

    #region * 하피 투사체 오브젝트 풀링

    public BossBullet BossBullet1_Harpy;
    private List<BossBullet> BossBulletPool = new List<BossBullet>();
    private readonly int BossBulletMaxCount = 20;
    private int currentBulletIndex = 0;

    public GameObject Player;

    protected Animator EnemyAnimator;
    protected bool isAttack;
    Rigidbody EnemyRigid;

    public float Movespeed;
    private HitEffectManager hitEffectManager;

    [SerializeField] public float Enemy_Recognition_Range;

    [SerializeField]
    public float EnemyHP = 10;
    public float PrimitiveHP; // 원 HP 수치. 보스 폭주 체크시에 사용 

    [SerializeField]
    public float Damage = 10f;

    protected EnemySpawner enemySpawner;
    protected DebuffChecker debuffChecker;

    [SerializeField]
    protected GameObject attackRangeObj;

    protected MeshRenderer SR;

    [SerializeField]
    protected bool isHit = false;

    //HP바 생성
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    public float FullHP;

    private Canvas uiCanvas;

    // hp UI 변수
    public GameObject hpBar;
    public GameObject hpBarFill;
    public GameObject hpBarFrame;

    public bool HPOn = false;
    public bool isBoss = false;

    // 스택 데미지. 단위 %로 전달. 
    public float totalStackDamage = 0f;

    // 스택 데미지 배열.
    float[] stackDamageArray = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

    // 프레임 당 스택 데미지. % 단위로 전달받은 스택 데미지를 참고하여 계산된 데미지.
    float calcStackDamage = 0f;

    // 디버프 배열
    int[] debuffArray;
    float[] excutionArray = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

    // 타이머 변수.
    public float time = 0f;


    // 초당 데미지 리스트
    List<TimeDamage> timeDamageList = new List<TimeDamage>();

    public Player playerdata;

    //공격 딜레이 값
    public float AttackDelay = 1f;

    //탐색 범위
    public float range = 20f;

    protected bool damaged = false;

    bool isDead = false;

    // 이 범위 내에 플레이어가 들어올시 공격
    [SerializeField] protected float EnemyPlayerAttackDistance = 3;

    public Transform player;
    public UnityEngine.AI.NavMeshAgent nvAgent;
    public Animator animator;

    protected void Start()
    {
        EnemyHP = 300;
        maxHP = EnemyHP;

        OnBossHPUpdated.Invoke(maxHP, EnemyHP);
        bossText = GameObject.FindObjectOfType<BossText>();
        if (bossText == null)
        {
            Debug.LogError("BossText를 찾을 수 없습니다.");
            return;
        }

        bossText.BossTexting(4f);

        Player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < BossBulletMaxCount; ++i)
        {
            BossBullet bul = Instantiate<BossBullet>(BossBullet1_Harpy);
            bul.gameObject.SetActive(false); 

            BossBulletPool.Add(bul);
            // 투사체 풀 생성 후 setActive(false)
        }
        StartCoroutine("Check_Camera");
        PrimitiveHP = EnemyHP;
        FullHP = EnemyHP;
        isBoss = true;
        hitEffectManager = this.gameObject.GetComponent<HitEffectManager>();
        
    }

    private void OnEnable()
    {
        SetHpBar_Boss();
    }

    #endregion


    // Update is called once per frame
    void Update()
    {
        if(isLook) // 시선이 플레이어를 향하도록 함
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            LookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(Player.transform.position + LookVec);
        }
        Boss_Overdrive_Check(); // 폭주 패턴 체크
    }

    IEnumerator Check_Camera() // 하피 패턴관리
    {
        yield return new WaitForSeconds(13f);
        StartCoroutine("Harpy_Pattern_Management");
    }

    IEnumerator Harpy_Pattern_Management() // 하피 패턴관리
    {
        yield return new WaitForSeconds(0.1f);

        int RAD_ACT = Random.Range(0, 2); // 패턴 처리 위해서 난수 부여
        Debug.Log(RAD_ACT);

        // 하드모드 패턴 추가시 Range를 늘리는 방향으로 구현 가능. 아니면 break문을 없애서 조건을 늘리던가..


        switch (RAD_ACT)
        {
            case 0:
                StartCoroutine(Harpy_Fire_1());
                break;

            case 1:
                StartCoroutine(Harpy_Fire_2());
                break;

        }
    }

    IEnumerator Harpy_Fire_1() // 무작위 방향으로 투사체 난사
    {

        for (int j = 0; j < BossBulletMaxCount; j++)
        {
            if (!BossBulletPool[currentBulletIndex].gameObject.activeSelf)
            {

                int AttackPosition_Random = Random.Range(1, 2);
                
                if (AttackPosition_Random == 1) BossBulletPool[currentBulletIndex].transform.position = AttackPO1.position;
                if (AttackPosition_Random == 2) BossBulletPool[currentBulletIndex].transform.position = AttackPO2.position;

                // 공격 위치 난수설정

                // 총알 활성화
                BossBulletPool[currentBulletIndex].gameObject.SetActive(true);
                // Debug.Log("Pattern 1 Activated!");

                // 방금 9번째 총알을 발사했다면 다시 0번째 총알을 발사할 준비를 한다.
                if (currentBulletIndex >= BossBulletMaxCount - 1)
                {
                    currentBulletIndex = 0;
                }
                else
                {
                    currentBulletIndex++;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        if (isOverdriving) { yield return new WaitForSeconds(Boss_Pattern_Waiting_Time); } // 일반 패턴
        else{ yield return new WaitForSeconds((2 * Boss_Pattern_Waiting_Time) / 3); } // 폭주 패턴
        StartCoroutine(Harpy_Pattern_Management());
    }

    IEnumerator Harpy_Fire_2() // 중앙에서 일정 거리동안 순간이동하고 큰 투사체 발사
    {
        Debug.Log("Pattern 2 Activated!");
        int ran = Random.Range(0, 360); //랜덤으로 0~360도
        float x = Mathf.Cos(ran * Mathf.Deg2Rad) * 8f; // 정해진 위치에서 5만큼 떨어진 원형 랜덤 방향으로 생성
        float z = Mathf.Sin(ran * Mathf.Deg2Rad) * 8f; // 정해진 위치에서 5만큼 떨어진 원형 랜덤 방향으로 생성

        gameObject.transform.position = new Vector3(0f, 0f, 0f) + new Vector3(x, 0f, z);

        yield return new WaitForSeconds(0.5f);
        GameObject Big_Projecter = Instantiate(Harpy_Big_Projecter, gameObject.transform.position, gameObject.transform.rotation);

        if (isOverdriving) { yield return new WaitForSeconds(Boss_Pattern_Waiting_Time); } // 일반 패턴
        else
        {
            GameObject Big_Projecter_2 = Instantiate(Harpy_Big_Projecter, gameObject.transform.position, gameObject.transform.rotation);
            yield return new WaitForSeconds((2 * Boss_Pattern_Waiting_Time) / 3);
        } // 폭주 패턴
        StartCoroutine(Harpy_Pattern_Management());
    }

    void Boss_Overdrive_Check()
    {
        if (EnemyHP <= PrimitiveHP / 2)
        {
            isOverdriving = true;
            // Debug.Log("하피 폭주 패턴 개시");
        }
    }


    public void CounterAttacked(float _damage, Player _player)
    {
        playerdata = _player;
        debuffArray = playerdata.GetCounterAttackDebuffArray();
        Debug.Log("Counter Damaged!");
        ApplyDamage(_damage, 4);
        OnBossHPUpdated.Invoke(maxHP, EnemyHP);

        // 디버프 적용
        if (EnemyHP <= (FullHP * 0.3f))
        {
            excutionArray = playerdata.GetExecutionAbilityArray();
            debuffChecker.DebuffCheckJS(debuffArray, excutionArray);
        }
        else
        {
            debuffChecker.DebuffCheckJS(debuffArray);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // 공격 종류에 따른 피격 관련 기능 수행
        if (other.tag == "PlayerAttack")
        {
            Debug.Log("Damaged!");
            // EnemyAnimator.SetTrigger("Damaged");
            // attackRangeObj.SetActive(false);
            damaged = true;

            hitEffectManager.ShowHitEffect(transform.position, 0);

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.transform.GetComponentInParent<Player>();
            float damage = playerdata.PlayerAttackDamage;
            debuffArray = playerdata.GetAttackDebuff();

            ApplyDamage(damage, 0);

            // 요 두 함수가 특수 공격 게이지 판정
            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            OnBossHPUpdated.Invoke(maxHP, EnemyHP);
        }

        if (other.tag == "StrongPlayerAttack")
        {
            Debug.Log("Strongly Damaged!");
            // EnemyAnimator.SetTrigger("Damaged");
            // attackRangeObj.SetActive(false);
            damaged = true;

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.transform.GetComponentInParent<Player>();
            float damage = playerdata.PlayerStrongAttackDamage;
            debuffArray = playerdata.GetStAttackDebuff();

            hitEffectManager.ShowHitEffect(transform.position, 0);

            if (GameManager_JS.Instance != null)
            {
                Debug.Log("피격 및 게이지 판정");
                GameManager_JS.Instance.Guage();
            }
            else
            {
                Debug.Log("게임매니저가 탐지되지 않았습니다. 조치가 필요합니다.");
            }

            // 피격 시 체력 감소 계산
            ApplyDamage(damage, 1);

            // 요 두 함수가 특수 공격 게이지 판정
            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            OnBossHPUpdated.Invoke(maxHP, EnemyHP);

            // 디버프 적용
            if (EnemyHP <= (FullHP * 0.3f))
            {
                excutionArray = playerdata.GetExecutionAbilityArray();
                debuffChecker.DebuffCheckJS(debuffArray, excutionArray);
            }
            else
            {
                debuffChecker.DebuffCheckJS(debuffArray);
            }
        }

        if (other.tag == "PlayerAttackProjectile")
        {
            if (!isBoss && HPOn == false)
            {
                HPOn = true;
            }

            Debug.Log("Damaged by Player Projectile");
            // EnemyAnimator.SetTrigger("Damaged");
            // attackRangeObj.SetActive(false);
            damaged = true;

            hitEffectManager.ShowHitEffect(transform.position, 0);

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.GetComponent<PlayerProjectile>().player;
            float damage = playerdata.PlayerAttackDamage;
            debuffArray = playerdata.GetStAttackDebuff();

            if (GameManager_JS.Instance != null)
            {
                Debug.Log("피격 및 게이지 판정");
                GameManager_JS.Instance.Guage();
                Debug.Log($"Player Special Attack Guage 채워질 양 : {playerdata.PlayerSpecialAttackFillingAmount}");
                GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            }
            else
            {
                Debug.Log("게임매니저가 탐지되지 않았습니다. 조치가 필요합니다.");
            }

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;

            // 디버프 적용
            ApplyDamage(damage, 5);

            Destroy(other.gameObject);

            // 요 두 함수가 특수 공격 게이지 판정
            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            OnBossHPUpdated.Invoke(maxHP, EnemyHP);

            // 디버프 적용
            if (EnemyHP <= (FullHP * 0.3f))
            {
                excutionArray = playerdata.GetExecutionAbilityArray();
                debuffChecker.DebuffCheckJS(debuffArray, excutionArray);
            }
            else
            {
                debuffChecker.DebuffCheckJS(debuffArray);
            }
        }

        if (other.tag == "PlayerDodgeAttack" && playerdata.PlayerDodgeAttackDamage != 0)
        {
            Debug.Log("Dodge damaged!");
            // EnemyAnimator.SetTrigger("Damaged");
            // attackRangeObj.SetActive(false);
            damaged = true;

            hitEffectManager.ShowHitEffect(transform.position, 0);

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.transform.GetComponentInParent<Player>();
            float damage = playerdata.PlayerDodgeAttackDamage;
            debuffArray = playerdata.GetDodgeAttackDebuff();

            if (GameManager_JS.Instance != null)
            {
                Debug.Log("피격 및 게이지 판정");
                GameManager_JS.Instance.Guage();
            }
            else
            {
                Debug.Log("게임매니저가 탐지되지 않았습니다. 조치가 필요합니다.");
            }

            // 피격 시 체력 감소 계산
            ApplyDamage(damage, 2);

            // 요 두 함수가 특수 공격 게이지 판정
            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            OnBossHPUpdated.Invoke(maxHP, EnemyHP);

            // 디버프 적용
            if (EnemyHP <= (FullHP * 0.3f))
            {
                excutionArray = playerdata.GetExecutionAbilityArray();
                debuffChecker.DebuffCheckJS(debuffArray, excutionArray);
            }
            else
            {
                debuffChecker.DebuffCheckJS(debuffArray);
            }
        }

        if (other.tag == "PlayerFieldAttack")
        {
            Debug.Log("Field damaged!");
            // EnemyAnimator.SetTrigger("Damaged");
            // attackRangeObj.SetActive(false);
            damaged = true;

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.transform.GetComponent<Field>().playerstatus;
            float damage = playerdata.PlayerFieldAttackDamage;
            debuffArray = playerdata.GetFieldAttackDebuff();

            // 요 두 함수가 특수 공격 게이지 판정
            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            // 피격 시 체력 감소 계산
            ApplyDamage(damage, 3);

            OnBossHPUpdated.Invoke(maxHP, EnemyHP);

            // 디버프 적용
            if (EnemyHP <= (FullHP * 0.3f))
            {
                excutionArray = playerdata.GetExecutionAbilityArray();
                debuffChecker.DebuffCheckJS(debuffArray, excutionArray);
            }
            else
            {
                debuffChecker.DebuffCheckJS(debuffArray);
            }
        }

        IEnumerator GetDamaged()
        {
            SR.material.color = Color.red;
            yield return new WaitForSeconds(0.6f);
            SR.material.color = Color.white;
        }

        if (damaged == true)
        {
            Invoke("DamagedDelay", 0.7f);
        }
    }

    void DamagedDelay()
    {
        damaged = false;
    }


    public void TakeDamage(float _damage)
    {
        if (!isBoss && HPOn == false)
        {
            HPOn = true;
        }

        EnemyHP -= _damage;

        if (EnemyHP <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }

        // 체력 바 업데이
    }

    public void SetHpBar_Boss()
    {
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            GameObject bossHPUI = canvas.transform.Find("BossHPUI").gameObject;
        }
        else
        {
            Debug.LogError("Cannot Found Canvas.");
        }
    }

    // 시간당 데미지 적용
    public void TakeTimeDamage()
    {
        // 계산된 데미지 프레임당 적용
        for (int i = 0; i < timeDamageList.Count; i++)
        {
            timeDamageList[i].time += Time.deltaTime;
            TakeDamage(timeDamageList[i].calcDamage);
        }

        // 지정된 시간 초과 시 해당 객체 삭제
        for (int i = 0; i < timeDamageList.Count; i++)
        {
            if (timeDamageList[i].time > timeDamageList[i].targetTime)
            {
                timeDamageList.Remove(timeDamageList[i]);
            }
        }
    }

    // 스택 데미지 객체 생성. 물 타입 1티어 업그레이드 디버프 데미지.
    public void SetStackDamageOn(float _time)
    {
        if (playerdata.GetAttackDebuff()[0] >= 2f)
        {
            stackDamageArray[0] = playerdata.GetStackDamageArray()[0];
        }

        TimeDamage timeDamage = new TimeDamage();
        timeDamage.targetTime = _time;
        timeDamage.calcDamage = stackDamageArray[0] / _time * Time.fixedDeltaTime;

        timeDamageList.Add(timeDamage);

    }

    // 스택 데미지 객체 생성. 1티어 업그레이드 디버프 데미지 적용.
    public void SetStackDamageOn(int _index, float _time)
    {
        if (playerdata.GetAttackDebuff()[_index] >= 2f)
        {
            stackDamageArray[_index] = playerdata.GetStackDamageArray()[_index];
        }

        if (_time == 1f)
        {
            TakeDamage(stackDamageArray[_index]);
        }
        else
        {
            TimeDamage timeDamage = new TimeDamage();
            timeDamage.targetTime = _time;
            timeDamage.calcDamage = stackDamageArray[_index] / _time * Time.fixedDeltaTime;

            timeDamageList.Add(timeDamage);
        }

    }

    // 초당 데미지 객체 생성. 불 타입 디버프 데미지.
    public void SetTimeDamageOn(float _time, float _damage)
    {
        TimeDamage timeDamage = new TimeDamage();
        timeDamage.targetTime = _time;
        timeDamage.calcDamage = _damage / _time * Time.fixedDeltaTime;

        timeDamageList.Add(timeDamage);
    }

    // 공격 데미지 및 각 능력 타입 별 데미지 적용. 
    void ApplyDamage(float _damage, int _attackType)
    {
        // 공격 데미지 적용
        TakeDamage(_damage);

        // 각 능력 타입 별 데미지 적용
        for (int i = 0; i < debuffArray.Length; i++)
        {
            if (debuffArray[i] != 0)
            {
                switch (i)
                {
                    // 물 타입 : 스택 데미지 조건 충족 시 적용
                    case 0:

                        break;

                    // 불 타입 : 해당 종류 공격 초당 데미지 디버프 활성화 시 적용
                    case 1:
                        float timeDamage = GetAbilityFlameDamage(_attackType);
                        float time = 0.0f;

                        if (debuffArray[i] == 3)
                        {
                            time = 3.0f;
                        }
                        else
                        {
                            time = 5.0f;
                        }

                        SetTimeDamageOn(time, timeDamage);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    // 공격 종류에 따른 초당 데미지 수치 반환
    float GetAbilityFlameDamage(int _type)
    {
        float returnValue;

        switch (_type)
        {
            // 약공격
            case 0:
                returnValue = playerdata.PlayerAttackBurnDamage;
                break;

            // 강공격
            case 1:
                returnValue = playerdata.PlayerStrongAttackBurnDamamge;
                break;

            // 돌진 공격
            case 2:
                returnValue = playerdata.PlayerDodgeAttackBurnDamage;
                break;

            // 필드 공격
            case 3:
                returnValue = playerdata.PlayerFieldAttackBurnDamage;
                break;

            // 카운터 공격
            case 4:
                returnValue = playerdata.PlayerCounterIgnitionDamage;
                break;

            default:
                returnValue = 0.0f;
                break;
        }
        return returnValue;
    }

    protected virtual void UpdateTarget()
    {
        //자신의 위치로부터 range만큼의 반경의 충돌체를 검사하고 
        Collider[] cols = Physics.OverlapSphere(transform.position, range);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                //반경 내에 플레이어가 존재할 경우 추적
                if (cols[i].tag == "Player")
                {
                    //Debug.Log("Enemy find Target");
                    player = cols[i].gameObject.transform;
                }
            }
        }
        else
        {
            //Debug.Log("Enemy lost Target");

            //animator.SetBool("isAttack", false);
            player = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
