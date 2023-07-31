using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerTest : MonoBehaviour
{
    //현재 시간
    float currentTime;
    //일정 시간
    float createTime = 5f;
    //적 생성
    public GameObject enemies;
    //생성된 적 개수
    int enemycnt = 0;

    //임시 변수
    int i;
    int tmp;

    //지정 위치
    public List<GameObject> place;

    //최소 시간
    float minTime = 1.0f;
    //최대 시간
    float maxTime = 5.0f;

    void Start()
    {
        
    }

    void Update()
    { 
        //시간이 흐름
        currentTime += Time.deltaTime;

        i = Random.Range(0, place.Count);

        //만약 현재 시간이 일정 시간이 된다면
        if (currentTime > createTime && enemycnt < 5)
        {
            GameObject enemy = Instantiate(enemies);
            enemy.transform.position = place[i].transform.position;
            place.RemoveAt(i);

            enemycnt++;
            currentTime = 0;
        }
    }
}
