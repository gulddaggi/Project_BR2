using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int Spawn_ReturnCount()
    {
        int count = Random.Range(1, spawnPoints.Count);

        for (int i = 0; i < count; i++)
        {
            int randpos = Random.Range(0, spawnPoints.Count);
            GameObject spawnEnemy = Instantiate(enemy, spawnPoints[randpos].position, Quaternion.identity);
            spawnEnemy.transform.SetParent(this.gameObject.transform);
            spawnEnemy.GetComponent<Ghoul>().SetSpawner(this.gameObject.GetComponent<EnemySpawner>());
            spawnPoints.Remove(spawnPoints[randpos]);
        }

        return count;
    }

    public void EnemyDead()
    {
        this.gameObject.GetComponentInParent<Dungeon>().DecEnemyCount();
    }

}
