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
    GameObject enemy;

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

    private void Awake()
    {
        if (spawnPoints.Count != 0)
        {
            curEnemyCount = spawnPoints[0].transform.childCount;
            this.gameObject.GetComponentInParent<Dungeon>().SetEnemyCount(curEnemyCount);
        }

    }

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
        }

    }

    void Update()
    {
        if (spawnTrigger && curEnemyCount == 0)
        {
            Debug.Log("적 스폰");
            spawnTrigger = false;
            EnemySpawn();
        }
    }

    // 스폰할 적의 수 지정 및 반환
    public int Spawn_ReturnCount()
    {
        int count = Random.Range(1, spawnPoints.Count);
        //EnemySpawn(count);
        return count;
    }

    // 적 스폰
    void EnemySpawn()
    {
        int points;

        if (curWaveCount == 0)
        {
            points = spawnPoints[0].childCount;

            for (int i = 0; i < points; i++)
            {
                GameObject spawnEnemy = Instantiate(enemy, spawnPoints[i].GetChild(i).position, Quaternion.identity);
                spawnEnemy.transform.SetParent(this.gameObject.transform);
                Debug.Log("적 생성");
                ++curEnemyCount;
            }
        }
        else
        {
            for (int i = curWaveCount; i < spawnPoints.Count; i++)
            {
                points = spawnPoints[i].childCount;
                for (int j = 0; j < points; j++)
                {
                    GameObject spawnEnemy = Instantiate(enemy, spawnPoints[i].GetChild(i).position, Quaternion.identity);
                    spawnEnemy.transform.SetParent(this.gameObject.transform);
                    ++curEnemyCount;
                }
            }
        }
        ++curWaveCount;
        this.gameObject.GetComponentInParent<Dungeon>().SetEnemyCount(curEnemyCount);
    }

    public void EnemyDead()
    {
        --curEnemyCount;
        this.gameObject.GetComponentInParent<Dungeon>().DecEnemyCount();

        Debug.Log("EnemyCount : " + curEnemyCount);
        if (curEnemyCount == 0 && curWaveCount != 0)
        {
            Debug.Log("트리거 전환");
            spawnTrigger = true;

        }
    }

}
