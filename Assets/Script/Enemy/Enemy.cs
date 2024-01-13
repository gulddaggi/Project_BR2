using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

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

    // 1티어 업그레이드 디버프 적용 여부 확인 변수. 0이 아닐 경우 적용.
    public float drawnDamage = 0f;

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
        //Track_Player();
        //Enemy_Anim_Manage();
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

    void OnTriggerEnter(Collider other)
    {
        // 디버프 배열
        int[] debuffArray;

        // 공격 종류에 따른 피격 관련 기능 수행
        if (other.tag == "PlayerAttack")
        {
            if (!isBoss && HPOn == false)
            {
                HPOn = true;
                SetHpBar();
            }

            Debug.Log("Damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) drawnDamage = playerdata.PlayerDrawnDamage;
            float damage = playerdata.PlayerAttackDamage;
            debuffArray = playerdata.GetAttackDebuff();

            // 요 두 함수가 특수 공격 게이지 판정
            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, drawnDamage);

            // 체력 바 업데이트
            if (!isBoss)
            {
                hpBarFill.GetComponent<Image>().fillAmount = EnemyHP / FullHP;
            }
        }
        else if (other.tag == "StrongPlayerAttack")
        {
            if (!isBoss && HPOn == false)
            {
                HPOn = true;
                SetHpBar();
            }

            Debug.Log("Strongly Damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) drawnDamage = playerdata.PlayerDrawnDamage;
            float damage = playerdata.PlayerStrongAttackDamage;
            debuffArray = playerdata.GetStAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, drawnDamage);

            // 체력 바 업데이트
            if (!isBoss)
            {
                hpBarFill.GetComponent<Image>().fillAmount = EnemyHP / FullHP;
            }
        }
        else if (other.tag == "PlayerDodgeAttack")
        {
            if (!isBoss && HPOn == false)
            {
                HPOn = true;
                SetHpBar();
            }

            Debug.Log("Dodge damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) drawnDamage = playerdata.PlayerDrawnDamage;
            float damage = playerdata.PlayerDodgeAttackDamage;
            debuffArray = playerdata.GetDodgeAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, drawnDamage);

            // 체력 바 업데이트
            if (!isBoss)
            {
                hpBarFill.GetComponent<Image>().fillAmount = EnemyHP / FullHP;
            }
        }
        else if (other.tag == "PlayerFieldAttack")
        {
            if (!isBoss && HPOn == false)
            {
                HPOn = true;
                SetHpBar();
            }

            Debug.Log("Field damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) drawnDamage = playerdata.PlayerDrawnDamage;
            float damage = playerdata.PlayerFieldAttackDamage;
            debuffArray = playerdata.GetFieldAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, drawnDamage);

            // 체력 바 업데이트
            if (!isBoss)
            {
                hpBarFill.GetComponent<Image>().fillAmount = EnemyHP / FullHP;
            }
        }

        if (EnemyHP <= 0)
        {
            enemySpawner.EnemyDead();
            hpBar.SetActive(false);
            gameObject.SetActive(false);
        }

        IEnumerator GetDamaged()
        {
            SR.material.color = Color.red;
            yield return new WaitForSeconds(0.6f);
            SR.material.color = Color.white;
        }
    }
}
