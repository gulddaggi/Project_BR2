using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAITest2 : MonoBehaviour
{
    private Transform player;
    public UnityEngine.AI.NavMeshAgent nvAgent;
    Animator animator;
    public GameObject bulletPrefab; // 원거리 공격을 위한 프리팹
    public Transform bulletSpawnPoint; // 총알 발사 위치
    public float fireRate = 2.0f; // 원거리 공격 발사 속도 (초당 발사 횟수)
    private float nextFireTime = 0f;

    public float rotationSpeed = 5.0f; // 회전 속도
    public float moveSpeed = 3.0f; // 이동 속도

    // 원거리 공격 속도와 방향을 설정할 변수
    public float bulletSpeed = 10.0f;

    // 랜덤한 이동을 위한 변수
    public float moveInterval = 5.0f; // 이동 간격
    public float randomMoveDuration = 1.0f; // 랜덤 이동 지속 시간
    private float nextMoveTime;
    private bool isRandomMoving = false;

    private bool isAttacking = false; // 공격 중 여부를 나타내는 변수

    // Start is called before the first frame update
    void Start()
    {
        //nvAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent <Animator>();

        // 처음 이동 시간 설정
        nextMoveTime = Time.time + Random.Range(0.0f, moveInterval);

        // 0.25초마다 타깃 체크
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            animator.SetBool("isWalk", true);
            nvAgent.destination = player.position;
            float dis = Vector3.Distance(player.position, gameObject.transform.position);

            if (dis <= 15)
            {
                if (Time.time >= nextFireTime)
                {
                    animator.SetBool("isAttack", true);
                    nextFireTime = Time.time + 2.0f / fireRate;
                    LaunchBullet();
                }else
                {
                    animator.SetBool("isAttack", false);
                    if (!isRandomMoving)
                    {
                        StartCoroutine(StartRandomMove());
                    }
                }
            }
            else
            {
                animator.SetBool("isAttack", false);
                if (!isRandomMoving)
                {
                    StartCoroutine(StartRandomMove());
                }
            }
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
        if (isRandomMoving && Time.time >= nextMoveTime)
        {
            StopRandomMove();
        }
    }

    void UpdateTarget()
    {
        // 자신의 위치로부터 20f만큼의 반경의 충돌체를 검사하고 
        Collider[] cols = Physics.OverlapSphere(transform.position, 20f);
        float minDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                // 반경 내에 플레이어가 존재할 경우 추적
                if (cols[i].CompareTag("Player"))
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, cols[i].transform.position);
                    if (distanceToPlayer < minDistance)
                    {
                        minDistance = distanceToPlayer;
                        closestPlayer = cols[i].transform;
                    }
                    //player = cols[i].transform;
                }
            }
        }

        
        if (minDistance <= 15f)
        {
            player = null;
            nvAgent = null;
        }
        else
        {
            nvAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            player = closestPlayer;
        }
        
    }

    void LaunchBullet()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // 플레이어를 바라보는 방향을 구합니다
        Vector3 targetDirection = player.position - bulletSpawnPoint.position;

        // 총알이 플레이어 방향으로 회전하도록 설정합니다
        bulletInstance.transform.rotation = Quaternion.LookRotation(targetDirection);

        // 총알을 앞으로 이동시킴
        Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = targetDirection.normalized * bulletSpeed; // bulletSpeed는 총알의 속도

        // 총알 속도와 방향을 설정하고 날아가도록 함
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 20f);
    }

    // 코루틴을 이용한 랜덤 이동 시작
    IEnumerator StartRandomMove()
    {
        isRandomMoving = true;
        nvAgent.isStopped = false;

        // 일정 시간 동안 랜덤한 방향으로 이동
        while (Time.time < nextMoveTime)
        {
            float randomAngle = Random.Range(0f, 360f);
            Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward * 3;
            Vector3 destination = transform.position + randomDirection * 5.0f;
            nvAgent.SetDestination(destination);

            yield return null;
        }

        animator.SetBool("isAttack", true);
        LaunchBullet();

        isRandomMoving = false;
        nvAgent.isStopped = true;

        // 다음 랜덤 이동 시간 설정
        nextMoveTime = Time.time + Random.Range(0.0f, moveInterval);
    }

    // 랜덤 이동 중지
    void StopRandomMove()
    {
        if (isRandomMoving)
        {
            StopCoroutine("StartRandomMove");
            isRandomMoving = false;
            nvAgent.isStopped = true;

            // 다음 랜덤 이동 시간 설정
            nextMoveTime = Time.time + Random.Range(0.0f, moveInterval);
        }
    }
}

