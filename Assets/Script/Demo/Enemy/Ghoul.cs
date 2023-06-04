using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghoul : MonoBehaviour
{

    // 데모용 구울 조정 스크립트. 차후에 추상클래스로 이월 필요

    GameObject player;

    Animator EnemyAnimator;
    Rigidbody EnemyRigid;

    public float EnemyHP = 10;

    public float EnemyMovespeed;
    public float Damage = 10f;

    private NavMeshAgent nav;
    public bool Screamed = false;

    [SerializeField]
    EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        EnemyRigid = GetComponent<Rigidbody>();
        EnemyAnimator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Track_Player();
        Ghoul_Anim_Manage();
        DIeCheck();
    }

    void Track_Player()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < 15f)
        {
            StartCoroutine(Nav_Ghoul());
        }
        else
        {
            
            EnemyRigid.velocity = Vector3.zero;
        }

    }

    IEnumerator Nav_Ghoul()
    {
        transform.LookAt(player.transform.position);
        if (Screamed == false)
        {
            EnemyAnimator.SetTrigger("Scream");
            Screamed = true;
            yield return new WaitForSeconds(2.0f);
        }
        nav.destination = player.transform.position;

    }

    void Ghoul_Anim_Manage()
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
            Debug.Log("Ghoul Damaged!");
            var playerdata = player.GetComponent<Player>();
            EnemyHP = (playerdata.PlayerAttack(EnemyHP));

        }
        else if (other.tag == "StrongPlayerAttack")
        {
            Debug.Log("Ghoul Strongly Damaged!");
            var playerdata = player.GetComponent<Player>();
            EnemyHP = (playerdata.PlayerStrongAttack(EnemyHP));
        }
    }

    void DIeCheck()
    {
        if(EnemyHP <= 0)
        {
            gameObject.SetActive(false);
            enemySpawner.EnemyDead();
        }
    }

    public void SetSpawner(EnemySpawner _enemySpawner)
    {
        enemySpawner = _enemySpawner;
    }
}
