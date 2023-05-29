using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Stage
{
    // 출구 개수
    [SerializeField]
    int exitCount;

    [SerializeField]
    protected Transform rewardPos;

    [SerializeField]
    GameObject[] exitObjects;

    [SerializeField]
    protected GameObject reward;

    // 보상 생성 스크립트
    [SerializeField]
    protected RewardCreator rewardCreator;

    public int enemyCount = 0;
    private bool isClear = false;

    protected override void Start()
    {
        //base.Start();
        rewardCreator = this.gameObject.GetComponent<RewardCreator>();
        if (rewardCreator != null)
        {
            SetNextReward();
        }
    }

    public void SetReward(GameObject Obj)
    {
        reward = Obj;
        GameObject rewardObj = Instantiate(reward, rewardPos.position, Quaternion.identity);
        rewardObj.transform.SetParent(this.gameObject.transform);
        rewardObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    void SetNextReward()
    {

        for (int i = 0; i < exitObjects.Length; i++)
        {
            reward = rewardCreator.CreateReward();
            exitObjects[i].GetComponent<Exit>().CreateSampleReward(reward);
        }
    }

    void Clear()
    {
        isClear = true;
        //GameObject obj = Instantiate(reward, rewardPos);
        GameManager_JS.Instance.SetIsMoveOn(isClear);
    }

    protected virtual void Update()
    {
        if (enemyCount == 0 & !isClear)
        {
            Clear();
        }
    }
}
