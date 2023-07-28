using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    [SerializeField]
    GameObject[] events;

    [SerializeField]
    GameObject[] event_Ability;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    GameObject player;

    //public Text[] texts;
    public List<Transform> texts = new List<Transform>();

    RaycastHit hit;

    ChoiceGetter choiceGetter;

    [SerializeField]
    float range;

    // 감지된 이벤트 중 ability의 NPCNAME을 임시저장하는 변수
    string tmpName;

    // 감지된 이벤트의 TypeIndex를 임시저장하는 변수 
    int tmpTypeIndex;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        EventCheck();
    }

    void EventCheck()
    {
        // 이벤트 감지.
        if (Input.GetKey(KeyCode.E) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
        {
            EventData eventData = hit.transform.gameObject.GetComponent<EventData>();
            // 보상 오브젝트의 이벤트 관련 데이터 저장.
            int type = eventData.EventType;
            tmpTypeIndex = eventData.TypeIndex;

            switch (type)
            {
                // 이벤트 : 능력
                case 0:
                    tmpName = hit.transform.gameObject.GetComponent<AbilityData>().NPCNAME;
                    ChoiceEventStart();
                    break;

                case 1:

                    CoinEvent(tmpTypeIndex);
                    break;

                default:
                    break;
            }

            Destroy(hit.transform.gameObject, 0.01f);
            
        }
        else
        {
            Debug.Log("감지 안됨");
        }
    }

    // 능력 선택 이벤트 시작
    void ChoiceEventStart()
    {
        // 선택 전체 UI 활성화
        events[0].SetActive(true);
        GameManager_JS.Instance.PanelOff();
        Time.timeScale = 0f;
        player.SetActive(false);
    }

    // 코인 획득 이벤트
    void CoinEvent(int value)
    {
        GameObject obj = Instantiate(events[1], player.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        obj.GetComponent<TextMesh>().text = "+" + value.ToString();
        obj.transform.SetParent(this.gameObject.transform.parent);
        Destroy(obj, 2f);
        GameManager_JS.Instance.Coin = value;
    }

    public void SwitchToChoice()
    {
        // 대화 UI 비활성화
        event_Ability[0].SetActive(false);
        // 선택 UI 활성화
        event_Ability[1].SetActive(true);
        TextSet_Ability();
    }

    public void ChoiceEventEnd()
    {
        // UI 설정 초기화
        ChoiceEventInit();
        // 선택 전체 UI 비활성화
        events[0].SetActive(false);
    }

    // 이벤트 실행 전 상태로 초기화 실행
    void ChoiceEventInit()
    {
        event_Ability[0].SetActive(true);
        event_Ability[1].SetActive(false);
        GameManager_JS.Instance.PanelOn();
        player.SetActive(true);
        Time.timeScale = 1f;
    }

    // 능력 선택지 세팅
    void TextSet_Ability()
    {
        choiceGetter = event_Ability[1].GetComponent<ChoiceGetter>();

        // 선택지 개수만큼 반복 수행.
        for (int i = 0; i < choiceGetter.choices.Count; i++)
        {
            for (int j = 0; j < choiceGetter.choices[0].childCount; j++)
            {
                // 선택지 양식 하나의 텍스트들을 변수에 입력
                texts.Add(choiceGetter.choices[i].GetChild(j));
            }

            // 해당 텍스트에 DB 데이터 입력.
            EventDBManager.instance.TextDisplay_Ability(tmpTypeIndex, texts, i);
            texts.Clear();
        }
    }
}
