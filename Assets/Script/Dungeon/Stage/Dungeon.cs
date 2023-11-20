using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    protected EnemySpawner enemySpawner;

    protected int enemyCount = 0;
    protected bool isClear = false;

    protected override void Start()
    {
        //base.Start();

        // 보상 생성기 Get
        rewardCreator = this.gameObject.GetComponent<RewardCreator>();
        BakeNavMesh();

        // 다음 보상 세팅
        if (rewardCreator != null)
        {
            SetNextReward();
        }

    }

    private void OnEnable()
    {
        // 턴 기반 이벤트 실행
        Debug.Log(this.gameObject.name + "턴기반 이벤트 실행");
        EventManager.Instance.TurnBasedEventPost();
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

    // 출구에 표시하기 위한 다음 보상을 생성.
    protected void SetNextReward()
    {
        // 출구 개수만큼 다음 보상 생성.
        for (int i = 0; i < exitObjects.Length; i++)
        {
            // 랜덤 보상 생성 후 변수 저장.
            reward = rewardCreator.CreateReward();
            
            // 테스트용 보상 생성 함수 실행.
            //reward = rewardCreator.CreateReward(0, true);
            // 생성된 보상을 해당 인덱스 출구에 표시
            exitObjects[i].GetComponent<Exit>().CreateSampleReward(reward);
        }
    }

    // 클리어 시 실행 함수. 스테이지 이동 허용 및 보상 생성.
    void Clear()
    {
        if (GameManager_JS.Instance.GetDungeonCount() == 1)
        {
            isClear = true;
            CreateReward();
            GameManager_JS.Instance.SetIsMoveOn(isClear);
        }
        else
        {
            if (enemySpawner.IsAllWaveEnd())
            {
                isClear = true;
                CreateReward();
                GameManager_JS.Instance.SetIsMoveOn(isClear);
            }
        }
    }

    protected virtual void Update()
    {
        if (enemyCount == 0 && !isClear)
        {
            Clear();
        }
    }

    public void DecEnemyCount()
    {
        --enemyCount;
    }

    public void SetEnemyCount(int _value)
    {
        enemyCount = _value;
    }

    //테스트용
    public void Die()
    {
        GameManager_JS.Instance.InitStage();
    }

    void BakeNavMesh()
    {
        NavMeshSurface navMeshSurface = this.gameObject.GetComponent<NavMeshSurface>();
        navMeshSurface.RemoveData();
        navMeshSurface.BuildNavMesh();
    }
}
