using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    [SerializeField]
    float duration = 2f; // 변화하는 데 걸리는 시간

    [SerializeField]
    Image panelImage;

    [SerializeField]
    GameObject rewardSocket;
    
    bool moveTrigger = false;

    private void Start()
    {
        panelImage = GameManager_JS.Instance.GetPanel();
        StartCoroutine(FadeInPanel());
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && GameManager_JS.Instance.GetIsMoveOn())
        {
            StartCoroutine(FadeOutPanel());
        }
    }

    private IEnumerator FadeOutPanel()
    {
        Color startColor = panelImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float currentAlpha = Mathf.Lerp(startColor.a, 255f, elapsedTime / duration);
            if (currentAlpha >= 15f)
            {
                break;
            }
            Debug.Log(currentAlpha);

            // 현재 알파 값을 새로운 색상으로 설정
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, currentAlpha);
            panelImage.color = newColor;

            elapsedTime += Time.deltaTime * 0.05f;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        GameManager_JS.Instance.NextStage(this.gameObject.transform);
    }

    private IEnumerator FadeInPanel()
    {
        Color startColor = panelImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float currentAlpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / duration);

            // 현재 알파 값을 새로운 색상으로 설정
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, currentAlpha);
            panelImage.color = newColor;

            elapsedTime += Time.deltaTime * 0.5f;
            yield return null;
        }
    }
}
