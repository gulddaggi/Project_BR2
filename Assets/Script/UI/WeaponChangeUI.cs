using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChangeUI : MonoBehaviour
{
    private Camera uiCamera; //UI 카메라를 담을 변수
    private Canvas canvas; //캔버스를 담을 변수
    private RectTransform rectParent; //부모의 rectTransform 변수를 저장할 변수
    private RectTransform rectHp; //자신의 rectTransform 저장할 변수

    public Vector3 offset = Vector3.zero; //HpBar 위치 조절용, offset은 어디에 HpBar를 위치 출력할지
    public Transform CrystalTr; // 무기 변경 크리스탈 위치


    void Start()
    {
        canvas = GetComponentInParent<Canvas>(); 
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(CrystalTr.position + offset); // 월드좌표(3D)를 스크린좌표(2D)로 변경, offset은 오브젝트 머리 위치

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); // 스크린좌표에서 캔버스에서 사용할 수 있는 좌표로 변경

        rectHp.localPosition = localPos + new Vector2(150f, 0f); //그 좌표를 localPos에 저장, 거기에 hpbar를 출력
    }

}