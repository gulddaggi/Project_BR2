using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerEvent : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectHp;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
            //x,y,(z)메인카메라에서XY평면까지의거리
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos,
        uiCamera, out localPos);

        rectHp.localPosition = localPos;
    }
}
