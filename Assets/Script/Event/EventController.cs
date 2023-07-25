using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    [SerializeField]
    GameObject[] events;

    [SerializeField]
    GameObject choiceEvent;

    [SerializeField]
    GameObject choiceDialogue;

    [SerializeField]
    GameObject choiceMain;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    GameObject player;

    // 각 보상의 색상 정보(RGBA -> Vector3(color.r, color.g, color.b)를 담고있는 딕셔너리.
    [SerializeField]
    Dictionary<Vector3, string> itemMap;

    //public Text[] texts;
    public List<Transform> texts = new List<Transform>();

    RaycastHit hit;

    ChoiceGetter choiceGetter;

    [SerializeField]
    float range;

    private void Start()
    {
        player = GameObject.Find("Player");
        itemMap = new Dictionary<Vector3, string>();

        // 보상 오브젝트의 색상 값을 키값으로 하여 딕셔너리 구성
        itemMap.Add(new Vector3(1f, 0f, 0f), "Ability"); // 능력
    }

    void Update()
    {
        EventCheck();
    }

    void EventCheck()
    {
        // event trigger detectedslxl 
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
        {
            //event type : choice
            // tag.Contains(value)사용하여 특정 문자열 포함하는지 확인
            if (Input.GetKeyDown(KeyCode.E) && hit.transform.tag.Contains("Event"))
            {
                GameManager_JS.Instance.SetAbilityIndex(hit.transform.tag[tag.Length - 1]); // 능력 오브젝트의 tag 끝의 index를 지정. -> 실패. 다른 방안 시도 예정.
                Destroy(hit.transform.gameObject, 0.01f);

                ChoiceEventStart();
            }
        }
    }

    void ChoiceEventStart()
    {
        choiceEvent.SetActive(true);
        GameManager_JS.Instance.PanelOff();
        Time.timeScale = 0f;
        player.SetActive(false);
        // add function about stopping all objects
    }

    public void SwitchToChoice()
    {
        choiceDialogue.SetActive(false);
        choiceMain.SetActive(true);
        TextSet_Ability();
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
        GameManager_JS.Instance.PanelOn();
        player.SetActive(true);
        Time.timeScale = 1f;
        // add function about restarting move of all objects
    }

    // 능력 선택지 세팅
    void TextSet_Ability()
    {
        choiceGetter = choiceMain.GetComponent<ChoiceGetter>();

        // 접근할 DB 딕셔너리 인덱스. 이후에 인덱스 지정 메서드를 구현하여 변수 입력 필요. -> GameManager에 인덱스 저장 변수 구현
        int DBAccessNum = GameManager_JS.Instance.GetAbilityIndex(); // 보상 오브젝트의 tag로부터 저장된 인덱스 값

        // 선택지 개수만큼 반복 수행.
        for (int i = 0; i < choiceGetter.choices.Count; i++)
        {
            for (int j = 0; j < choiceGetter.choices[0].childCount; j++)
            {
                // 선택지 양식 하나의 텍스트들을 변수에 입력
                texts.Add(choiceGetter.choices[i].GetChild(j));
            }

            // 해당 텍스트에 DB 데이터 입력.
            EventDBManager.instance.TextDisplay_Ability(DBAccessNum, texts, i);
            texts.Clear();
        }


    }
}
