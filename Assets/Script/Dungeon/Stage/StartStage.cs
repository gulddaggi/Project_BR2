using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage : Dungeon
{
    protected override void Start()
    {
        base.Start();
        StartReward();
    }

    void StartReward()
    {
        // 제일 처음 시도일 경우, 스토리 진행을 위해 처음 보상을 능력으로 지정해야함
        //if (GameManager_JS.Instance.GetTryCount() == 0) reward = rewardCreator.CreateReward(1);
        /*else*/ reward = rewardCreator.CreateReward(2);
        
        GameObject rewardObj = Instantiate(reward, rewardPos.position, Quaternion.identity);
        rewardObj.transform.SetParent(this.gameObject.transform);
        rewardObj.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();

        if (isClear)
        {
            reward.SetActive(false);
        }
    }
}
