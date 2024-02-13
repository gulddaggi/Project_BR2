using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float destroyTime = 5f;       // 투사체 파괴 시간

    protected virtual void Start()
    {
        // 일정 시간 후에 투사체 파괴
        StartCoroutine(SelfDestruct());
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // 투사체가 다른 콜라이더와 충돌했을 때 수행할 동작
        if (other.CompareTag("Enemy"))
        {
            // 적에게 맞았을 때의 동작을 여기에 작성
            Debug.Log("플레이어 투사체 충돌 감지 - 적");
        }
    }

    protected virtual IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
