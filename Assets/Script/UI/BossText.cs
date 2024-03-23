using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossText : MonoBehaviour
{
    public RectTransform uiElement;

    public float moveDuration = 1.5f;
    private float uIDisableTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        uiElement.anchoredPosition = new Vector2(0, Screen.height);
    }

    // Update is called once per frame
    public void BossTexting(float SetupTime) {
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
}
