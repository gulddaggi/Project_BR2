using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform Player;

    protected Animator EnemyAnimator;
    protected bool isAttack;
    Rigidbody EnemyRigid;

    public float Movespeed;
    private NavMeshAgent nav;

    [SerializeField] public float Enemy_Recognition_Range;

    [SerializeField]
    public float EnemyHP = 10;

    [SerializeField]
    public float Damage = 10f;

    protected EnemyManagerTest enemySpawner;
    protected DebuffChecker debuffChecker;

    [SerializeField]
    protected GameObject attackRangeObj;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemySpawner = this.gameObject.GetComponentInParent<EnemyManagerTest>();
        debuffChecker = this.gameObject.GetComponent<DebuffChecker>();
        //EnemyRigid = GetComponent<Rigidbody>();
        EnemyAnimator = GetComponent<Animator>();
        //nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Track_Player();
        //Enemy_Anim_Manage();
    }

    void Track_Player()
    {
        float distance = Vector3.Distance(Player.position, transform.position);
        if (distance < Enemy_Recognition_Range)
        {
            nav.destination = Player.position;
        }
        else
        {
            EnemyRigid.velocity = Vector3.zero;
        }

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
}
