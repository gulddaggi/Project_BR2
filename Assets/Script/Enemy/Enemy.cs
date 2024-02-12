using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

// 초당 데미지 클래스
public class TimeDamage
{
    public float time = 0f;
    public float targetTime = 0f;
    public float calcDamage = 0f;
}

public class Enemy : MonoBehaviour
{
    public GameObject Player;

    protected Animator EnemyAnimator;
    protected bool isAttack;
    Rigidbody EnemyRigid;

    public float Movespeed;
    private NavMeshAgent nav;

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
    float[] stackDamageArray = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};

    // 프레임 당 스택 데미지. % 단위로 전달받은 스택 데미지를 참고하여 계산된 데미지.
    float calcStackDamage = 0f;

    // 디버프 배열
    int[] debuffArray;
    bool[] excutionArray = { false, false, false, false, false };

    // 타이머 변수.
    public float time = 0f;
    bool timerOn = false;

    // 타이머 작동 시간
    float targetTime = 0f;

    // 초당 데미지 리스트
    List<TimeDamage> timeDamageList = new List<TimeDamage>();

    Player playerdata;

    protected virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enemySpawner = this.gameObject.GetComponentInParent<EnemySpawner>();
        debuffChecker = this.gameObject.GetComponent<DebuffChecker>();
        EnemyAnimator = GetComponent<Animator>();
        SR = gameObject.GetComponent<MeshRenderer>();
        FullHP = EnemyHP;
    }

    void Update()
    {
        //TakeTimeDamage();
    }

    private void FixedUpdate()
    {
        TakeTimeDamage();
    }

    void Track_Player()
    {
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if (distance < Enemy_Recognition_Range)
        {
            nav.destination = Player.transform.position;
        }
        else
        {
            EnemyRigid.velocity = Vector3.zero;
        }
    }

    public void SetHpBar()
    {
        uiCanvas = GameObject.Find("EnemyUI").GetComponent<Canvas>();
        hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarFrame = hpBar.transform.GetChild(0).gameObject;
        hpBarFill = hpBar.transform.GetChild(1).gameObject;

        var _hpbar = hpBar.GetComponent<EnemyHp>();
        _hpbar.targetTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;
    }

    void Enemy_Anim_Manage()
    {
        if (EnemyRigid.velocity.normalized != Vector3.zero)
        {
            // 속력벡터가 0이 아닐 시
            EnemyAnimator.SetTrigger("Track");
        }
        else if (EnemyRigid.velocity == Vector3.zero)
        {
            EnemyAnimator.SetTrigger("Idle");
        }
    }

    public void EnemyAttackOn()
    {
        isAttack = true;
        attackRangeObj.SetActive(true);
        Invoke("EnemyAttackOff", 1f);
    }

    protected void EnemyAttackOff()
    {
        attackRangeObj.SetActive(false);
        isAttack = false;
    }

    public void CounterAttacked(float _damage, Player _player)
    {
        playerdata = _player;
        debuffArray = playerdata.GetAttackDebuff();
        Debug.Log("Counter Damaged!");
        ApplyDamage(_damage, 4);
        // 디버프 적용
        debuffChecker.DebuffCheckJS(playerdata.GetAttackDebuff());
    }

    void OnTriggerEnter(Collider other)
    {
        // 공격 종류에 따른 피격 관련 기능 수행
        if (other.tag == "PlayerAttack")
        {
            Debug.Log("Damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerStackDamage != 0f) totalStackDamage = playerdata.PlayerStackDamage;
            float damage = playerdata.PlayerAttackDamage;
            debuffArray = playerdata.GetAttackDebuff();

            // 요 두 함수가 특수 공격 게이지 판정
            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            ApplyDamage(damage, 0);

            // 디버프 적용
            if (EnemyHP <= (FullHP * 0.2f))
            {
                excutionArray = playerdata.GetExecutionAbilityArray();
                debuffChecker.DebuffCheckJS(debuffArray, excutionArray);
            }
            else
            {
                debuffChecker.DebuffCheckJS(debuffArray);
            }
        }

        if (other.tag == "StrongPlayerAttack")
        {
            Debug.Log("Strongly Damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerStackDamage != 0f) totalStackDamage = playerdata.PlayerStackDamage;
            float damage = playerdata.PlayerStrongAttackDamage;
            debuffArray = playerdata.GetStAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            ApplyDamage(damage, 1);

            // 디버프 적용
            if (EnemyHP <= (FullHP * 0.2f))
            {
                excutionArray = playerdata.GetExecutionAbilityArray();
                debuffChecker.DebuffCheckJS(debuffArray, excutionArray);
            }
            else
            {
                debuffChecker.DebuffCheckJS(debuffArray);
            }
        }

        if (other.tag == "PlayerDodgeAttack")
        {
            Debug.Log("Dodge damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerStackDamage != 0f) totalStackDamage = playerdata.PlayerStackDamage;
            float damage = playerdata.PlayerDodgeAttackDamage;
            debuffArray = playerdata.GetDodgeAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            ApplyDamage(damage, 2);

            // 디버프 적용
            if (EnemyHP <= (FullHP * 0.2f))
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

            // 플레이어로부터 데미지, 디버프 배열 반환
            playerdata = other.transform.GetComponent<Field>().playerstatus;
            // 익사 디버프 여부 확인
            if (playerdata.PlayerStackDamage != 0f) totalStackDamage = playerdata.PlayerStackDamage;
            float damage = playerdata.PlayerFieldAttackDamage;
            debuffArray = playerdata.GetFieldAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            ApplyDamage(damage, 3);

            // 디버프 적용
            if (EnemyHP <= (FullHP * 0.2f))
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
    }

    public void Dead()
    {
        enemySpawner.EnemyDead();
        hpBar.SetActive(false);
        gameObject.SetActive(false);
    }

    public void TakeDamage(float _damage)
    {
        if (!isBoss && HPOn == false)
        {
            HPOn = true;
            SetHpBar();
        }

        EnemyHP -= _damage;
        
        if (EnemyHP <= 0)
        {
            Dead();
        }

        // 체력 바 업데이트
        if (!isBoss)
        {
            hpBarFill.GetComponent<Image>().fillAmount = EnemyHP / FullHP;
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
        TimeDamage timeDamage = new TimeDamage();
        timeDamage.targetTime = _time;
        timeDamage.calcDamage = EnemyHP * (totalStackDamage * 0.01f) * Time.fixedDeltaTime;

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
}
