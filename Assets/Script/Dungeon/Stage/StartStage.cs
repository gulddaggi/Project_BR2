using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage : Dungeon
{
    protected override void Start()
    {
        // 보상 생성기 Get
        rewardCreator = this.gameObject.GetComponent<RewardCreator>();

        // 다음 보상 세팅
        if (rewardCreator != null)
        {
            SetNextReward();
        }

        StartReward();

        // 첫 스테이지에 소환된 적의 수. 직접 입력해야함.
        enemyCount = 2;

        GameManager_JS.Instance.OnStartStageLoaded.Invoke();
    }

    void StartReward()
    {
        // 제일 처음 시도일 경우, 스토리 진행을 위해 처음 보상을 능력으로 지정해야함
        if (GameManager_JS.Instance.GetTryCount() == 0) reward = rewardCreator.CreateReward(1);
        else reward = rewardCreator.CreateReward(1);

        // 테스트용. 한 종류의 보상만 생성.
        reward = rewardCreator.CreateReward(3, true);

        
        GameObject rewardObj = Instantiate(reward, rewardPos.position, Quaternion.identity);
        rewardObj.transform.SetParent(this.gameObject.transform);
        rewardObj.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
    }
}
