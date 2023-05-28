using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Stage
{
    // 출구 개수
    [SerializeField]
    int exitCount;

    [SerializeField]
    Transform rewardPos;

    [SerializeField]
    GameObject[] exitObjects;

    [SerializeField]
    GameObject reward;

    public int enemyCount = 0;
    private bool isClear = false;

    protected override void Start()
    {
        //base.Start();
        //SetNextReward();
    }

    public void SetReward(GameObject Obj)
    {
        reward = Obj;
    }

    void SetNextReward()
    {
        for (int i = 0; i < exitObjects.Length; i++)
        {

        }
    }

    void Clear()
    {
        isClear = true;
        //GameObject obj = Instantiate(reward, rewardPos);
        GameManager_JS.Instance.SetIsMoveOn(isClear);
    }

    void Update()
    {
        if (enemyCount == 0 & !isClear)
        {
            Clear();
        }
    }
}
