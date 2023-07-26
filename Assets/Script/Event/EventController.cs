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
        // 이벤트 감지. 레이케스트 감지와 상호작용 키 입력 확인.
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask)
            && Input.GetKeyDown(KeyCode.E))
        {
            // 보상 오브젝트의 이벤트 관련 데이터 저장.
            int type = hit.transform.gameObject.GetComponent<EventData>().EventType;
            int index = hit.transform.gameObject.GetComponent<EventData>().TypeIndex;

            switch (type)
            {
                // 이벤트 : 능력
                case 0:
                    GameManager_JS.Instance.SetAbilityIndex(index);
                    ChoiceEventStart();
                    break;

                default:
                    break;
            }

            Destroy(hit.transform.gameObject, 0.01f);
            
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
        int DBAccessNum = GameManager_JS.Instance.GetAbilityIndex(); // 보상 오브젝트로부터 전달받은 세부(능력) 인덱스 값.

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
