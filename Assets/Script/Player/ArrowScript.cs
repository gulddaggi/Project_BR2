using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowScript : MonoBehaviour
{
    public float destroyTime = 5f;       // 화살 파괴 시간

    private void Start()
    {
        // 일정 시간 후에 화살 파괴
        StartCoroutine(Self_Destruct());
    }

    private void OnTriggerEnter(Collider other)
    {
        // 화살이 다른 콜라이더와 충돌했을 때 수행할 동작
        if (other.CompareTag("Enemy"))
        {
            // 적에게 맞았을 때의 동작을 여기에 작성
            Debug.Log("플레이어 투사체 충돌 감지 - 적");
            // Destroy(gameObject);
        }
        /*
        else if (other.CompareTag("Obstacle"))
        {
            // 장애물과 충돌했을 때의 동작을 여기에 작성
            Debug.Log("플레이어 투사체 충돌 감지 - 장애물");
            Destroy(gameObject);
        }
        */
    }

    IEnumerator Self_Destruct()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}