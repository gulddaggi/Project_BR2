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
    public Image hpBarImage;
    public Image hpBarImage2;
    public bool HPOn = false;

    // 1티어 업그레이드 디버프 적용 여부 확인 변수. 이후 bool[]로 변경 예정
    private bool isDrawnOn = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        enemySpawner = this.gameObject.GetComponentInParent<EnemySpawner>();
        debuffChecker = this.gameObject.GetComponent<DebuffChecker>();
        //EnemyRigid = GetComponent<Rigidbody>();
        EnemyAnimator = GetComponent<Animator>();
        //nav = GetComponent<NavMeshAgent>();
        SR = gameObject.GetComponent<MeshRenderer>();
        //hp바
        FullHP = EnemyHP;
        //SetHpBar();
    }

    // Update is called once per frame
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
        GameObject HPBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = HPBar.GetComponentsInChildren<Image>()[1];
        hpBarImage2 = HPBar.GetComponentsInChildren<Image>()[0];

        var _hpbar = HPBar.GetComponent<EnemyHp>();
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
        // 삭제 예정
        float[] tmpArray = new float[2] { 0f, 0f };

        // 디버프 배열
        int[] debuffArray;

        // 공격 종류에 따른 피격 관련 기능 수행
        if (other.tag == "PlayerAttack" && isHit == false)
        {
            if (HPOn == false)
            {
                HPOn = true;
                SetHpBar();
            }

            isHit = true;
            Debug.Log("Damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) isDrawnOn = true;
            float damage = playerdata.PlayerAttackDamage;
            debuffArray = playerdata.GetAttackDebuff();

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, isDrawnOn);
            
            // 체력 바 업데이트
            hpBarImage.fillAmount = EnemyHP / FullHP;

            isHit = false;
        }
        else if (other.tag == "StrongPlayerAttack" && isHit == false)
        {
            if (HPOn == false)
            {
                HPOn = true;
                SetHpBar();
            }

            isHit = true;
            Debug.Log("Strongly Damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) isDrawnOn = true;
            float damage = playerdata.PlayerStrongAttackDamage;
            debuffArray = playerdata.GetStAttackDebuff();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, isDrawnOn);

            // 체력 바 업데이트
            hpBarImage.fillAmount = EnemyHP / FullHP;
        }
        else if (other.tag == "PlayerDodgeAttack" && isHit == false)
        {
            /*if (HPOn == false)
            {
                HPOn = true;
                SetHpBar();
            }*/

            isHit = true;
            Debug.Log("Dodge damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerDrawnDamage != 0f) isDrawnOn = true;
            float damage = playerdata.PlayerDodgeAttackDamage;
            debuffArray = playerdata.GetDodgeAttackDebuff();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, isDrawnOn);

            // 체력 바 업데이트
            hpBarImage.fillAmount = EnemyHP / FullHP;
        }

        isHit = false;

        if (EnemyHP <= 0)
        {
            enemySpawner.EnemyDead();
            gameObject.SetActive(false);
            //hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
        }

        IEnumerator GetDamaged()
        {
            SR.material.color = Color.red;

            yield return new WaitForSeconds(0.6f);
            SR.material.color = Color.white;
            isHit = false;

        }
    }
}
