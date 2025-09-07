using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadeOut : MonoBehaviour
{
    private CanvasGroup cg;
    public float fadeTime = 1f; // 페이드 타임
    float accumTime = 0f;
    private Coroutine fadeCor;

    private void Awake()
    {
        cg = gameObject.GetComponent<CanvasGroup>();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.0f);
        accumTime = 0f;
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        cg.alpha = 0f;
    }
}
