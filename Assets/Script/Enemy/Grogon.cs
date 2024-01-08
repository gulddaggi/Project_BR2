using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grogon : Enemy
{
    public GameObject bulletPrefab;

    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        /*
        if (!isHit)
        {
            // 여기에 적의 행동 로직을 추가합니다.

            // 원거리 공격을 발사할 때의 로직
            if (언제 원거리 공격을 발사할지의 조건)
            {
                LaunchBullet(); // 원거리 공격 발사
            }
        }
        */
    }


    void LaunchBullet()
    {
        // 원거리 공격을 발사하는 함수
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }
}
