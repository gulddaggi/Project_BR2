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
    protected bool isAttack = false;

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

    public float AttackDelay = 1f;

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

    public GameObject FindPlayerE;
    public Vector3 FindPlayerOffset = new Vector3(0, 2.2f, 0);

    public Transform player;
    public UnityEngine.AI.NavMeshAgent nvAgent;
    public Animator animator;

    //탐색 범위
    public float range = 20f;

    // 이 범위 내에 플레이어가 들어올시 공격
    [SerializeField] protected float EnemyPlayerAttackDistance = 3;

    protected virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enemySpawner = this.gameObject.GetComponentInParent<EnemySpawner>();
        debuffChecker = this.gameObject.GetComponent<DebuffChecker>();
        EnemyAnimator = GetComponent<Animator>();
        SR = gameObject.GetComponent<MeshRenderer>();
        FullHP = EnemyHP;

        nvAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        //0.25초마다 타깃 체크
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            animator.SetBool("isWalk", true);
            nvAgent.destination = player.position;
            float dis = Vector3.Distance(player.position, gameObject.transform.position);
            if (dis <= EnemyPlayerAttackDistance && isAttack == false)
            {
                EnemyAttackOn();
            }
            else
            {
                //animator.SetBool("isAttack", false);
            }
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

    protected virtual void EnemyAttackOn()
    {
        isAttack = true;
        animator.SetBool("isAttack", true);
        Invoke("EnemyAttackRangeON", 0.3f);
        Invoke("EnemyAttackOff", AttackDelay);
    }

    protected virtual void EnemyAttackOff()
    {
        attackRangeObj.SetActive(false);
        isAttack = false;
        animator.SetBool("isAttack", false);
    }

    protected virtual void EnemyAttackRangeON()
    {
        attackRangeObj.SetActive(true);
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
            EnemyAnimator.SetTrigger("Damaged");


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
            StartCoroutine(GetDamaged());

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
            EnemyAnimator.SetTrigger("Damaged");
            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) drawnDamage = playerdata.PlayerDrawnDamage;
            float damage = playerdata.PlayerStrongAttackDamage;
            debuffArray = playerdata.GetStAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            StartCoroutine(GetDamaged());

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
            EnemyAnimator.SetTrigger("Damaged");
            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) drawnDamage = playerdata.PlayerDrawnDamage;
            float damage = playerdata.PlayerDodgeAttackDamage;
            debuffArray = playerdata.GetDodgeAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            StartCoroutine(GetDamaged());

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
            EnemyAnimator.SetTrigger("Damaged");
            SR.material.color = Color.red;
            yield return new WaitForSeconds(0.6f);
            SR.material.color = Color.white;
        }
    }

    protected virtual void UpdateTarget()
    {
        //자신의 위치로부터 10f만큼의 반경의 충돌체를 검사하고 
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

            animator.SetBool("isAttack", false);
            player = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
