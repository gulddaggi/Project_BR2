using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 생성 클래스
public class EnemySpawner : MonoBehaviour
{
    // 소환될 지점 리스트.
    [SerializeField]
    List<Transform> spawnPoints = new List<Transform>();

    // 소환할 적 프리팹. 이후에 배열로 변경.
    [SerializeField]
    GameObject[] enemyPrefabs;

    [SerializeField]
    GameObject curEnemy;

    [SerializeField]
    int curWaveCount = 0;

    // 최대 웨이브
    int maxWaveCount = 2;

    // 현재 소환된 적 수
    public int curEnemyCount = 0;

    // 웨이브 당 소환할 지점의 수. 에디터 상에서 직접 지정.
    // 최대 수 초과하지 않도록 주의.
    [SerializeField]
    int[] spawnCount = new int[2];

    // 스폰 제어 트리거
    bool spawnTrigger = false;

    public bool SPAWNTRIGGER { get { return spawnTrigger; } }

    void Start()
    {
        // 에러 감지.
        if (spawnCount[0] + spawnCount[1] > spawnPoints.Count)
        {
            Debug.LogError("지정한 소환 지점의 수 초과! EnemySpanwer 확인");
        }

        if (spawnPoints.Count != 0)
        {
            EnemySpawn();
            this.gameObject.GetComponentInParent<Dungeon>().SetEnemyCount(curEnemyCount);
        }

    }

    void Update()
    {
        if (spawnTrigger && curEnemyCount <= 0)
        {
            spawnTrigger = false;
            if (!IsAllWaveEnd())
            {
                EnemySpawn();
            }
        }
    }

    // 적 스폰
    void EnemySpawn()
    {
        int minSpawnPoints = 3; // 최소 spawnPoint 수
        int maxSpawnPoints = 6; // 최대 spawnPoint 수
        int numSpawnPoints = Random.Range(minSpawnPoints, maxSpawnPoints);
        List<int> spawnCounts = new List<int>();
        for (int i = 0; i < numSpawnPoints; i++)
        {
            int randomSpawnCount = Random.Range(0, spawnPoints.Count);
            spawnCounts.Add(randomSpawnCount);
        }
        float rad = 2f;
        float angle;
        EnemyChoice();

        if (curWaveCount == 0)
        {
            for (int i = 0; i < numSpawnPoints; i++)
            {
                GameObject spawnEnemy = Instantiate(curEnemy, 
                    spawnPoints[spawnCounts[i]].position ,
                     Quaternion.identity);
                spawnEnemy.transform.SetParent(this.gameObject.transform);
                ++curEnemyCount;
            }
            ++curWaveCount;
        }
        else
        {
                for (int i = 0; i < numSpawnPoints; i++)
                {
                    GameObject spawnEnemy = Instantiate(curEnemy, spawnPoints[spawnCounts[i]].position, Quaternion.identity);
                    spawnEnemy.transform.SetParent(this.gameObject.transform);
                    ++curEnemyCount;
                }
                ++curWaveCount;
        }
        this.gameObject.GetComponentInParent<Dungeon>().SetEnemyCount(curEnemyCount);
    }

    public void EnemyDead()
    {
        --curEnemyCount;
        this.gameObject.GetComponentInParent<Dungeon>().DecEnemyCount();

        if (curEnemyCount == 0 && curWaveCount != 0)
        {
            spawnTrigger = true;
        }
    }

    public bool IsAllWaveEnd()
    {
        return (curWaveCount > maxWaveCount);
    }

    // 소환할 적 추첨
    void EnemyChoice()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        curEnemy = enemyPrefabs[index];
    }
}
