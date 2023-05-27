using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int stageSize = 9; // 스테이지 개수

    [SerializeField]
    private GameObject[] stages;

    [SerializeField]
    private Vector3 instPos;

    private void Start()
    {
        CreateStage();
    }

    private void CreateStage()
    {
        int index;

        // 지정 크기만큼 스테이지 정보 생성
        for (int i = 1; i < stageSize; i++)
        {
            if (i == 0)
            {
                index = 0;
            }
            else if(i == stageSize - 1)
            {
                index = 4;
            }
            else
            {
                index = Random.Range(1, 4);
            }

            GameObject obj = Instantiate(stages[index], new Vector3(-4.8f, -0.15f, 0.12f), Quaternion.identity);
            obj.SetActive(false);
            GameManager_JS.Instance.StageInput(obj);
        }
    }
}

