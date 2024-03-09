using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public RectTransform uiElement;
    [SerializeField] private TextMeshProUGUI uiText;
    public float moveDuration = 1.5f;
    private float uIDisableTime = 5.0f;

    void Start()
    {
        // 초기 위치를 화면 상단으로 설정
        uiElement.anchoredPosition = new Vector2(0, Screen.height);
        uiText = transform.GetComponentInChildren<TextMeshProUGUI>();

        // 찾은 TextMeshProUGUI에 접근하여 수정 또는 사용
        if (uiText != null)
        {
            SetStageUIText();
        }
        else
        {
            Debug.LogError("TextMeshProUGUI를 확인할 없습니다.");
        }

        // LeanTween을 사용하여 UI 요소를 아래로 이동
        LeanTween.move(uiElement, new Vector2(0, -50), moveDuration).setEase(LeanTweenType.easeOutExpo);
        StartCoroutine(UIOut());
    }

    IEnumerator UIOut()
    {
        yield return new WaitForSeconds(uIDisableTime);
        LeanTween.move(uiElement, new Vector2(0, Screen.height), moveDuration).setEase(LeanTweenType.easeOutExpo);
    }

    void SetStageUIText()
    {

        string currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName == "HomeScene")
        {
            uiText.text = "봄이 닿지 않는 정원";
        }
        else if (currentSceneName == "DungeonScene_JSTest")
        {
            uiText.text = "깊고 어두운 숲 초입";
        }
        Debug.Log("현재 Scene 이름 : " + currentSceneName + " / Stage UI Text 출력 확인");

    }
    }