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
        reward = rewardCreator.CreateReward(2);
        GameObject rewardObj = Instantiate(reward, rewardPos.position, Quaternion.identity);
        rewardObj.transform.SetParent(this.gameObject.transform);
    }

    protected override void Update()
    {
        base.Update();
    }
}
