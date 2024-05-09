using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossClear : MonoBehaviour
{
    int rewardValue = 200;

    [SerializeField]
    Text countText;

    [SerializeField]
    Text rewardText;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        countText.text = "처치 횟수 : " + GameManager_JS.Instance.BossKillCount;
        rewardText.text = "보상 : " + rewardValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BossClearEnd()
    {
        Time.timeScale = 1f;
        GameManager_JS.Instance.Gem += rewardValue;
        GameManager_JS.Instance.InitStage();
    }
}
