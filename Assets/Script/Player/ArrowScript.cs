using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowScript : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    public float arrowSpeed = 10f;        // 화살 발사 속도
    public float destroyTime = 5f;       // 화살 파괴 시간

    private void Start()
    {
        // 각도 보정
        // SetInitialAngle();

        // 화살을 정면 방향으로 발사
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * arrowSpeed;

        // 일정 시간 후에 화살 파괴
        StartCoroutine(Self_Destruct());
    }

    private void OnTriggerEnter(Collider other)
    {
        // 화살이 다른 콜라이더와 충돌했을 때 수행할 동작
        if (other.CompareTag("Enemy"))
        {
            // 적에게 맞았을 때의 동작을 여기에 작성
            Debug.Log("적 피격 감지");
            ObjectPoolManager.instance.Pool.Release(this.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            // 장애물과 충돌했을 때의 동작을 여기에 작성
            Debug.Log("장애물에 부딪혔습니다!");
            ObjectPoolManager.instance.Pool.Release(this.gameObject);
        }
    }

    void SetInitialAngle()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // X 축의 회전 값을 90으로 고정
        currentRotation.x = 90f;

        // 조정된 회전 값을 적용
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    IEnumerator Self_Destruct()
    {
        yield return new WaitForSeconds(destroyTime);
        ObjectPoolManager.instance.Pool.Release(this.gameObject);
    }
}
