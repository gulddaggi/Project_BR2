using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class BossText : MonoBehaviour
{

    public enum BossDialogueCheck
    {
        Appear, Overdriving
    }

    public RectTransform uiElement;

    public float moveDuration = 1.5f;
    private float uIDisableTime = 5.0f;

    [SerializeField]
    TextMeshProUGUI BossDialogueText;

    string[] BossDialogues = { "봄도 겨울도, 영원히 계속될 수는 없다.",
        "너무 늦었다. 봄은 죽었고, 스스로의 구제도 베풀지 못하리.",
        "교훈이 필요한 모양이구나,",
        "진흙탕 속의 투쟁, 혹은 명예로운 퇴장. 인생은 기나긴 선택의 연속이다.",
        "네 아이를 구하지 못할 것이다. 다름아닌 너 때문에."
    };

    string[] OverdrivingDialogues =
    {
        "선을 넘었구나.",
        "발버둥쳐도 결말은 바뀌지 않는다. 네 아이도 돌아오지 않아.",
        "가장 깊은 숲의 분노를 보여주마.",
        "이제 와서 무엇 하나 바꿀 수 없다."
    };

    // Start is called before the first frame update
    void Start()
    {
        uiElement.anchoredPosition = new Vector2(0, Screen.height);
        BossDialogueText.text = "";
    }

    // Update is called once per frame
    public void BossTexting(float SetupTime)
    {
        StartCoroutine(UIRectSetUp(SetupTime));
    }

    IEnumerator UIRectSetUp(float SetupTime)
    {
        yield return new WaitForSeconds(SetupTime);
        StartCoroutine("MoveUI");
    }

    IEnumerator MoveUI()
    {
        LeanTween.move(uiElement, new Vector2(475, 15), moveDuration).setEase(LeanTweenType.easeOutExpo);
        yield return new WaitForSeconds(uIDisableTime);
        LeanTween.move(uiElement, new Vector2(0, Screen.height), moveDuration).setEase(LeanTweenType.easeOutExpo);
    }

    public IEnumerator BossDialogue()
    {
        Debug.Log("보스 다이얼로그 체크 시작");
        int ran = Random.Range(0, BossDialogues.Length);
        BossDialogueText.text = BossDialogues[ran];
        Debug.Log("보스 등장 다이얼로그");
        yield return new WaitForSeconds(5.5f);
        BossDialogueText.text = "";
    }
    public IEnumerator OverdrivingBossDialogue()
    {
        int ran = Random.Range(0, OverdrivingDialogues.Length);
        BossDialogueText.text = OverdrivingDialogues[ran];

        Debug.Log("폭주 다이얼로그");

        yield return new WaitForSeconds(5.5f);
        BossDialogueText.text = "";
    }
}
