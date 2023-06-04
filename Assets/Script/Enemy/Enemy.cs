using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform Player;

    Animator EnemyAnimator;
    Rigidbody EnemyRigid;

    public float Movespeed;
    private NavMeshAgent nav;

    [SerializeField] public float Enemy_Recognition_Range;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRigid = GetComponent<Rigidbody>();
        EnemyAnimator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Track_Player();
        Enemy_Anim_Manage();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack")
        {
            Debug.Log("Enemy Damaged!");
        }
    }
}
