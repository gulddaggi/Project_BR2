using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{

    [SerializeField]
    GameObject choiceEvent;

    [SerializeField]
    GameObject choiceDialogue;

    [SerializeField]
    GameObject choiceMain;

    [SerializeField]
    LayerMask layerMask;

    RaycastHit hit;

    ChoiceGetter choiceGetter;

    [SerializeField]
    float range;

    void Update()
    {
        EventCheck();
    }

    void EventCheck()
    {
        // event trigger detected
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
        {
            //event type : choice
            if (Input.GetKeyDown(KeyCode.E) && hit.transform.tag == "EventTrigger")
            {
                //Debug.Log("Event trigger on");
                ChoiceEventStart();
            }
        }
    }

    void ChoiceEventStart()
    {
        choiceEvent.SetActive(true);
        Time.timeScale = 0f;
        // add function about stopping all objects
    }

    public void SwitchToChoice()
    {
        choiceDialogue.SetActive(false);
        ChoiceTextSet();
        choiceMain.SetActive(true);
    }

    public void ChoiceEventEnd()
    {
        ChoiceEventInit();
        choiceEvent.SetActive(false);
    }

    void ChoiceEventInit()
    {
        choiceDialogue.SetActive(true);
        choiceMain.SetActive(false);
        Time.timeScale = 1f;
        // add function about restarting move of all objects
    }

    // 능력 선택지 세팅
    void ChoiceTextSet()
    {
        choiceGetter = choiceMain.GetComponent<ChoiceGetter>();
        for (int i = 0; i < choiceGetter.choices.Length; i++)
        {
            // 접근할 DB 딕셔너리 인덱스. 이후에 인덱스 지정 메서드를 구현하여 변수 입력 필요. 
            int DBAccessNum = i;
            // 선택지 양식 하나의 텍스트들을 변수에 입력
            Text[] texts = choiceGetter.choices[i].GetComponentsInChildren<Text>();
            // 해당 텍스트에 DB 데이터 입력
            EventDBManager.instance.ChoiceTextDisplay(texts, i);
        }
    }
}
