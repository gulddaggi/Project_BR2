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
    [SerializeField]
    int maxWaveCount = 2;

    // 현재 소환된 적 수
    public int curEnemyCount = 0;

    void Start()
    {
        if (spawnPoints.Count != 0 && !IsAllWaveEnd())
        {
            EnemySpawn();
        }
        else
        {
            this.gameObject.GetComponentInParent<Dungeon>().SetEnemyCount(curEnemyCount);
        }
    }

    // 적 스폰
    void EnemySpawn()
    {
        int minSpawnPoints = 2; // 최소 spawnPoint 수
        int maxSpawnPoints = 5; // 최대 spawnPoint 수
        int numSpawnPoints = Random.Range(minSpawnPoints, maxSpawnPoints);

        List<int> spawnPointsList = new List<int>();
        for (int i = 0; i < numSpawnPoints; i++)
        {
            int randomSpawnCount = Random.Range(0, spawnPoints.Count);
            spawnPointsList.Add(randomSpawnCount);
        }

        EnemyChoice();

        for (int i = 0; i < numSpawnPoints; i++)
        {
            GameObject spawnEnemy = Instantiate(curEnemy, spawnPoints[spawnPointsList[i]].position, Quaternion.identity);
            spawnEnemy.transform.SetParent(this.gameObject.transform);
            ++curEnemyCount;
        }
        ++curWaveCount;

        this.gameObject.GetComponentInParent<Dungeon>().SetEnemyCount(curEnemyCount);
    }

    public void EnemyDead()
    {
        --curEnemyCount;
        this.gameObject.GetComponentInParent<Dungeon>().DecEnemyCount();

        if (curEnemyCount == 0 && !IsAllWaveEnd())
        {
            EnemySpawn();
        }
    }

    public bool IsAllWaveEnd()
    {
        return (curWaveCount >= maxWaveCount);
    }

    // 소환할 적 추첨
    void EnemyChoice()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        curEnemy = enemyPrefabs[index];
    }
}
