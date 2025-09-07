using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObjectPool : MonoBehaviour
{
    public GameObject prefab; // 재사용할 객체 프리팹
    public int initialPoolSize = 9; // 스테이지 개수

    [SerializeField]
    private GameObject[] stages;

    private void Awake()
    {

    }

    private void CreateStageInfo()
    {
        // 지정 크기만큼 스테이지 정보 생성
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform); // prefab -> 리스트 인덱스에 해당하는 프리팹으로 변경 필요
            obj.SetActive(false);
            GameManager_JS.Instance.StageInput(obj);
        }
    }

    private void CreateStagePool()
    {
        // 스테이지 정보 기반으로 객체 풀 생성
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform); // prefab -> 리스트 인덱스에 해당하는 프리팹으로 변경 필요
            obj.SetActive(false);
            GameManager_JS.Instance.StageInput(obj);
        }
    }
}
