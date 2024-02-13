using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grogon : Enemy
{
    public GameObject bulletPrefab;

    public Transform bulletSpawnPoint; // 총알 발사 위치

    public float bulletSpeed = 10.0f; // 원거리 공격 속도와 방향을 설정할 변수

    int count = 0;

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
        isAttack = false;
        animator.SetBool("isAttack", false);
    }


    void LaunchBullet()
    {
        // 원거리 공격을 발사하는 함수
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletInstance.GetComponent<GrogonBullet>().enemy = this.GetComponent<Enemy>();

        // 플레이어를 바라보는 방향을 구합니다
        Vector3 targetDirection = player.position - bulletSpawnPoint.position;

        // 총알이 플레이어 방향으로 회전하도록 설정합니다
        bulletInstance.transform.rotation = Quaternion.LookRotation(targetDirection);

        // 총알을 앞으로 이동시킴
        Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = targetDirection.normalized * bulletSpeed; // bulletSpeed는 총알의 속도

        count++;

        if (count == 3)
        {
            StartCoroutine(MoveBackwardFromPlayer());
            count = 0;
        }
    }

    IEnumerator MoveBackwardFromPlayer()
    {
        animator.SetBool("isAttack", false);
        // 플레이어를 찾습니다.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Player object not found!");
            yield break;
        }

        // 플레이어로부터 반대 방향 벡터를 구합니다.
        Vector3 backwardDirection = -(playerObject.transform.position - transform.position).normalized;

        // 이동 속도를 계산합니다.
        float moveSpeed = 10f / 2f; // 5단위를 2초에 걸쳐 이동해야 하므로, 속도 = 거리 / 시간

        // 이동을 시작합니다.
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            // 현재 프레임에서 이동할 거리를 계산합니다.
            float frameMoveDistance = moveSpeed * Time.deltaTime;

            // 이동합니다.
            transform.Translate(backwardDirection * frameMoveDistance, Space.World);

            // 경과 시간을 누적합니다.
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기합니다.
            yield return null;
        }
        animator.SetBool("isAttack", true);
    }

}