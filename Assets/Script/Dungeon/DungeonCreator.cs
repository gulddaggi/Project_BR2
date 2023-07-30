using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int stageSize = 9; // 스테이지 개수

    // 스테이지 프리팹 모음
    [SerializeField]
    private GameObject[] stages;

    private void Start()
    {
        CreateStage();
    }

    // 스테이지 생성 함수
    private void CreateStage()
    {
        // 스테이지 인덱스 임시 변수
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

            // 스테이지 생성. 좌표 지정이 이상하게 되어 있어 수정 필요.
            GameObject obj = Instantiate(stages[index], new Vector3(-4.8f, 9.0f, 0.12f), Quaternion.identity);
            // 생성과 동시에 비활성화
            obj.SetActive(false);
            // 생성된 스테이지 관리 및 접근을 위해 큐에 삽입.
            GameManager_JS.Instance.StageInput(obj);
        }
    }
}

