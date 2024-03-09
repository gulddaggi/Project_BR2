using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform uiElement;
    [SerializeField] private TextMeshProUGUI uiText;

    public float moveDuration = 1.5f;
    private float uIDisableTime = 5.0f;

    void OnEnable()
    {
        GameManager_JS.Instance.OnStageChanged.AddListener(() => SetStageUIText());
    }

    void OnDisable()
    {
        GameManager_JS.Instance.OnStageChanged.RemoveListener(() => SetStageUIText());
    }

    void Start()
    {
        // 초기 위치를 화면 상단으로 설정
        uiElement.anchoredPosition = new Vector2(0, Screen.height);
        uiText = transform.GetComponentInChildren<TextMeshProUGUI>();

        // 찾은 TextMeshProUGUI에 접근
        if (uiText != null)
        {
            // SetStageUIText 메서드를 콜백으로 등록
            LeanTween.move(uiElement, new Vector2(0, -50), moveDuration).setEase(LeanTweenType.easeOutExpo)
                .setOnComplete(() => SetStageUIText());
        }
        else
        {
            Debug.LogError("TextMeshProUGUI를 확인할 없습니다.");
        }

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

        if (currentSceneName == "HomeScene")
        {
            uiText.text = "봄이 닿지 않는 정원";
        }
        else if (currentSceneName == "DungeonScene_JSTest")
        {
            if (GameManager_JS.Instance.curStage.ToString().StartsWith("Stage 1"))
            {
                uiText.text = "깊고 어두운 숲";
            }
            else if (GameManager_JS.Instance.curStage.ToString().StartsWith("Stage 2"))
            {
                uiText.text = "깊은 숲 최심부 - 하피의 둥지";
            }
            else
            {
                uiText.text = "깊고 어두운 숲 초입";
            }
        }
        Debug.Log("현재 Scene 이름 : " + currentSceneName + " / Stage UI Text 출력 확인");
    }
}