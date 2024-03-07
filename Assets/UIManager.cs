using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform uiElement;
    public float moveDuration = 1.5f;
    private float uIDisableTime = 5.0f;

    void Start()
    {
        // 초기 위치를 화면 상단으로 설정
        uiElement.anchoredPosition = new Vector2(0, Screen.height);

        // LeanTween을 사용하여 UI 요소를 아래로 이동
        LeanTween.move(uiElement, new Vector2(0, -50), moveDuration).setEase(LeanTweenType.easeOutExpo);
        StartCoroutine(UIOut());
    }

    IEnumerator UIOut()
    {
        yield return new WaitForSeconds(uIDisableTime);
        LeanTween.move(uiElement, new Vector2(0, Screen.height), moveDuration).setEase(LeanTweenType.easeOutExpo);
    }
}