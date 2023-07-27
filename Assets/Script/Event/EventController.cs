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
        // 상호작용 키 입력시 이벤트 감지 수행.
        if (Input.GetKeyDown(KeyCode.E))
        {
            EventCheck();
        }
    }

    void EventCheck()
    {
        // 이벤트 감지.
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
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

                case 1:
                    CoinEvent(index);
                    break;

                default:
                    break;
            }

            Destroy(hit.transform.gameObject, 0.01f);
            
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
