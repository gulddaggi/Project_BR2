using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrogonBullet : MonoBehaviour
{
    public float speed = 10.0f; // 총알의 속도
    public int damage = 10; // 총알의 피해량

    void Start()
    {
        // 일정 시간 후에 총알 파괴 
        Destroy(gameObject, 5.0f);
        transform.Translate(Vector3.up * 1.0f);
    }

    void Update()
    {
        // 총알을 앞으로 이동시킴
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어에게 피해를 입힘
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // 총알 파괴
            Destroy(gameObject);
        }
        else
        {
            // 다른 충돌 대상에 부딪힐 경우 총알 파괴
            Destroy(gameObject);
        }
    }
}
