using UnityEngine;

public class CameraApproachBoss : MonoBehaviour
{
    public Transform bossTransform;
    public float approachSpeed = 1.0f;

    private bool isApproaching = false;

    void Start()
    {
        // 스테이지 변경 후 1.5초 후에 CameraApproach 함수 호출
        Invoke("StartCameraApproach", 2.5f);
    }

    void StartCameraApproach()
    {
        isApproaching = true;
    }

    void Update()
    {
        if (isApproaching)
        {
            // 보스 쪽으로 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, bossTransform.position, Time.deltaTime * approachSpeed);

            // 카메라가 목표 지점에 도달했는지 확인
            if (Vector3.Distance(transform.position, bossTransform.position) < 0.1f)
            {
                isApproaching = false;
                DisableCamera();
            }
        }
    }

    void DisableCamera()
    {
        gameObject.SetActive(false); // 카메라를 비활성화
    }
}