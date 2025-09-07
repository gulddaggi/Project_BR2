using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierOpacityController : MonoBehaviour
{
    // 장애물의 기본 알파 값과 플레이어가 가까이 있을 때의 알파 값
    public float defaultAlpha = 1f;
    public float transparentAlpha = 0.3f;

    private Renderer[] barrierRenderers;

    private void Start()
    {
        // 장애물의 자식 오브젝트들 중 Renderer 컴포넌트 가져오기
        barrierRenderers = GetComponentsInChildren<Renderer>();

        // 기본 알파 값 설정
        SetAlpha(defaultAlpha);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 장애물 콜리더에 들어올 때 호출됨
        if (other.CompareTag("Player"))
        {
            // 플레이어가 가까이 있을 때 알파 값 설정
            SetAlpha(transparentAlpha);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 장애물 콜리더에서 나갈 때 호출됨
        if (other.CompareTag("Player"))
        {
            // 장애물의 기본 알파 값으로 설정
            SetAlpha(defaultAlpha);
        }
    }

    // 알파 값 설정 함수
    private void SetAlpha(float alpha)
    {
        Debug.Log("장애물 투명도 체크");
        // 장애물의 모든 머터리얼에 대해 알파 값 설정
        foreach (Renderer renderer in barrierRenderers)
        {
            foreach (Material material in renderer.materials)
            {
                // 셰이더 속성에 알파 값을 전달
                material.SetFloat("Alpha", alpha);
            }
        }
    }
}