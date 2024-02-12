using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grogon : Enemy
{
    public GameObject bulletPrefab;

    public Transform bulletSpawnPoint; // 총알 발사 위치

    public float bulletSpeed = 10.0f; // 원거리 공격 속도와 방향을 설정할 변수

    protected override void Start()
    {
        base.Start();
        animator.applyRootMotion = false;
    }

    protected override void EnemyAttackOn()
    {
        isAttack = true;
        animator.SetBool("isAttack", true);
        Invoke("EnemyAttackRangeON", 0.1f);
        Invoke("EnemyAttackOff", 1f);
    }

    protected override void EnemyAttackRangeON()
    {
        LaunchBullet();
    }

    protected override void EnemyAttackOff()
    {
        animator.SetBool("isAttack", false);
        isAttack = false;
    }


    void LaunchBullet()
    {
        // 원거리 공격을 발사하는 함수
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // 플레이어를 바라보는 방향을 구합니다
        Vector3 targetDirection = player.position - bulletSpawnPoint.position;

        // 총알이 플레이어 방향으로 회전하도록 설정합니다
        bulletInstance.transform.rotation = Quaternion.LookRotation(targetDirection);

        // 총알을 앞으로 이동시킴
        Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = targetDirection.normalized * bulletSpeed; // bulletSpeed는 총알의 속도
    }
}
