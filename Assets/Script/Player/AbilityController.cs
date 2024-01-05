using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 현재 플레이어 획득 능력 관련 제어 클래스.
public class AbilityController : MonoBehaviour
{
    [SerializeField]
    GameObject aLObject;

    void Start()
    {
        EventManager.Instance.AssignAbManager(aLObject.GetComponentInChildren<AbilityListManager>());
    }

    void Update()
    {
        // 키입력 확인
        if (!GameManager_JS.Instance.isEventOn)
        {
            AbilityKeyCheck();
        }
    }

    // 능력 확인창 키입력 확인
    void AbilityKeyCheck()
    {
        if (Input.GetKey(KeyCode.O))
        {
            GameManager_JS.Instance.isEventOn = true;
            AbilityListOn();
        }
    }

    void AbilityListOn()
    {
        aLObject.SetActive(true);
    }

    public void AbilityListOff()
    {
        GameManager_JS.Instance.isEventOn = false;
    }

}
