using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 생성 클래스
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    List<Transform> spawnPoints = new List<Transform>();

    [SerializeField]
    GameObject enemy;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 스폰할 적의 수 지정 및 반환
    public int Spawn_ReturnCount()
    {
        int count = Random.Range(1, spawnPoints.Count);
        EnemySpawn(count);
        return count;
    }

    // 적 스폰
    void EnemySpawn(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            int randpos = Random.Range(0, spawnPoints.Count);
            GameObject spawnEnemy = Instantiate(enemy, spawnPoints[randpos].position, Quaternion.identity);
            spawnEnemy.transform.SetParent(this.gameObject.transform);
            spawnEnemy.GetComponent<Ghoul>().SetSpawner(this.gameObject.GetComponent<EnemySpawner>());
            spawnPoints.Remove(spawnPoints[randpos]);
        }
    }

    public void EnemyDead()
    {
        this.gameObject.GetComponentInParent<Dungeon>().DecEnemyCount();
    }

}
