using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrogonRangeAITest : RangeAITest
{
    private int attackCount = 0; // 공격 횟수를 저장하는 변수

    // 공격 간격
    public float attackInterval = 1f;
    private float nextAttackTime = 0f;

    // 이동할 위치
    public float reverseMoveDistance = 5f;

    public GameObject bulletPrefab; // 원거리 공격을 위한 프리팹
    public Transform bulletSpawnPoint; // 총알 발사 위치

    public float rotationSpeed = 5.0f; // 회전 속도
    public float moveSpeed = 3.0f; // 이동 속도

    // 원거리 공격 속도와 방향을 설정할 변수
    public float bulletSpeed = 10.0f;

    void Start()
    {
        nvAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nextAttackTime = Time.time + attackInterval;
        animator = GetComponent<Animator>();
        //0.25초마다 타깃 체크
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    void Update()
    {
        // 1초에 1번 공격하도록 설정
        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    // 공격 메서드
    void Attack()
    {
        if (player != null && attackCount < 3)
        {
            animator.SetBool("isAttack", true);
            LaunchBullet();

            // 공격 수 증가
            attackCount++;
        }
        else
        {
            // 플레이어 방향 반대로 이동
            Vector3 directionToPlayer = transform.position - player.position;
            Vector3 targetPosition = transform.position + directionToPlayer.normalized * reverseMoveDistance;

            // 이동
            nvAgent.SetDestination(targetPosition);
        }

        animator.SetBool("isAttack", false);
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
    }
}
