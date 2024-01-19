using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    public GameObject arrowPrefab;  // 화살 프리팹을 저장할 변수
    public Transform arrowSpawnPoint;  // 화살이 발사될 위치를 저장할 변수

    void Update()
    {
        // 마우스 클릭을 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 위치로 Ray를 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 플레이어가 클릭한 위치로 화살을 발사
                ShootArrows(hit.point);
            }
        }
    }

    void ShootArrows(Vector3 targetPosition)
    {
        // 화살 프리팹을 복제하여 생성
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        // 화살을 발사 방향으로 회전
        Vector3 shootDirection = (targetPosition - arrowSpawnPoint.position).normalized;
        arrow.transform.forward = shootDirection;

        // 화살에 힘을 가하여 발사
        arrow.GetComponent<Rigidbody>().AddForce(shootDirection * 10f, ForceMode.Impulse);
    }
}
