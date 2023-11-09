using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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
    public HPBarEnemy hpBar;

    private GameObject hpBarInstance;
    private Canvas uiCanvas;
    private Image hpBarImage;

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
        SetHpBar();
        FullHP = EnemyHP;
        hpBar = GameObject.Find("HP_Enemy(Clone)").GetComponent<HPBarEnemy>();

    }

    // Update is called once per frame
    void Update()
    {
        hpBar.HPUpdate(FullHP, EnemyHP);
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

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("EnemyUI").GetComponent<Canvas>();
        GameObject HPBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = HPBar.GetComponentInChildren<Image>();

        var _hpbar = HPBar.GetComponent<EnemyHp>();
        _hpbar.targetTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;

        hpBarInstance = Instantiate(hpBarPrefab);
        hpBarInstance.transform.SetParent(transform);
        hpBarInstance.transform.localPosition = hpBarOffset;
        hpBarImage = hpBarInstance.GetComponent<Image>();
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
        float[] tmpArray = new float[2] { 0f, 0f };

        if (other.tag == "PlayerAttack" && isHit == false)
        {
            isHit = true;
            Debug.Log("Damaged!");

            var playerdata = other.transform.GetComponentInParent<Player>();
            tmpArray = playerdata.PlayerAttack(EnemyHP);

            EnemyHP = tmpArray[0];

            debuffChecker.DebuffCheck((int)tmpArray[1]);
            StartCoroutine(GetDamaged());

            hpBarImage.fillAmount = EnemyHP / FullHP;
        }
        else if (other.tag == "StrongPlayerAttack" && isHit == false)
        {
            Debug.Log("Strongly Damaged!");
            var playerdata = other.transform.GetComponentInParent<Player>();
            EnemyHP = (playerdata.PlayerStrongAttack(EnemyHP));
            tmpArray = playerdata.PlayerAttack(EnemyHP);
            EnemyHP = tmpArray[0];
            StartCoroutine(GetDamaged());
            debuffChecker.DebuffCheck((int)tmpArray[1]);

            hpBarImage.fillAmount = EnemyHP / FullHP;
        }
        else if (other.tag == "PlayerDodgeAttack" && isHit == false)
        {
            isHit = true;
            Debug.Log("Dodge damaged!");

            var playerdata = other.transform.GetComponentInParent<Player>();
            tmpArray = playerdata.PlayerDodgeAttack(EnemyHP);

            EnemyHP = tmpArray[0];

            debuffChecker.DebuffCheck((int)tmpArray[1]);
            StartCoroutine(GetDamaged());

            hpBarImage.fillAmount = EnemyHP / FullHP;
        }

        if (EnemyHP <= 0)
        {
            enemySpawner.EnemyDead();
            gameObject.SetActive(false);
            hpBarImage.GetComponentInParent<Image>().color = Color.clear;
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
