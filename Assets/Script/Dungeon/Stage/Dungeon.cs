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

    [SerializeField]
    protected GameObject curReward = null;

    // 보상 생성 스크립트
    [SerializeField]
    protected RewardCreator rewardCreator;

    [SerializeField]
    EnemySpawner enemySpawner;

    public int enemyCount = 0;
    protected bool isClear = false;

    protected override void Start()
    {
        //base.Start();
        rewardCreator = this.gameObject.GetComponent<RewardCreator>();
        //적 생성
        enemyCount = enemySpawner.Spawn_ReturnCount();

        if (rewardCreator != null)
        {
            SetNextReward();
        }
    }

    public void SetReward(GameObject Obj)
    {
        curReward = Obj;
    }

    void CreateReward()
    {
        if (curReward != null)
        {
            GameObject rewardObj = Instantiate(curReward, rewardPos.position, Quaternion.identity);
            rewardObj.transform.SetParent(this.gameObject.transform);
            reward.gameObject.SetActive(true);
            rewardObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

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
        CreateReward();
        GameManager_JS.Instance.SetIsMoveOn(isClear);
    }

    protected virtual void Update()
    {
        if (enemyCount == 0 & !isClear)
        {
            Clear();
        }
    }

    public void DecEnemyCount()
    {
        --enemyCount;
    }

    //테스트용
    public void Die()
    {
        GameManager_JS.Instance.InitStage();
    }
}
