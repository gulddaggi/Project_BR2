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

    GameObject reward;
    
    bool moveTrigger = false;

    private void Start()
    {
        panelImage = GameManager_JS.Instance.GetPanel();
        StartCoroutine(FadeInPanel());
        EventManager.Instance.TurnBasedEventPost();
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
        GameManager_JS.Instance.CoinOnOff(false);

        while (elapsedTime < duration)
        {
            float currentAlpha = Mathf.Lerp(startColor.a, 255f, elapsedTime / duration);
            if (currentAlpha >= 15f)
            {
                break;
            }
            //Debug.Log(currentAlpha);

            // 현재 알파 값을 새로운 색상으로 설정
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, currentAlpha);
            panelImage.color = newColor;

            elapsedTime += Time.deltaTime * 0.05f;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // 다음 스테이지로 이동
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

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        panelImage.color = color;
        GameManager_JS.Instance.CoinOnOff(true);
    }

    // 지정된 다음 보상 생성 후 출구에 표시
    public void CreateSampleReward(GameObject obj)
    {
        reward = Instantiate(obj, rewardSocket.transform.position, Quaternion.identity);
        reward.gameObject.SetActive(true);
        reward.transform.SetParent(this.gameObject.transform);
        //reward.GetComponent<SphereCollider>().enabled = false;
    }

    public GameObject GetRewardObj()
    {
        //reward.GetComponent<SphereCollider>().enabled = true;
        return reward;
    }
}
