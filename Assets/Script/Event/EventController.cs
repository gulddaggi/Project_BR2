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
    GameObject[] event_Merchant;

    [SerializeField]
    GameObject eventUpgrade;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    GameObject player;

    public List<Transform> texts = new List<Transform>();

    RaycastHit hit;

    ChoiceGetter choiceGetter;
    DialogueGetter dialogueGetter;
    [SerializeField]
    ItemFormGetter itemFormGetter;

    [SerializeField]
    List<string> dialogues = new List<string>();

    [SerializeField]
    float range;

    // 전체 이름 배열
    string[] namesArray = { "물의 정령", "불의 정령" };

    // 감지된 이벤트 중 ability의 NPCNAME을 임시저장하는 변수
    string tmpName;

    // 감지된 이벤트의 TypeIndex를 임시저장하는 변수 
    int tmpTypeIndex;

    // 이벤트 진행중 파악 여부. GameManager_JS에도 존재.
    public bool eventOn = false;

    private void Start()
    {
        //player = GameObject.Find("Player");
    }

    void Update()
    {
        if (!eventOn && !GameManager_JS.Instance.isEventOn)
        {
            EventCheck();
        }
    }

    void EventCheck()
    {
        // 이벤트 감지.
        if (Input.GetKey(KeyCode.E) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
        {
            if (hit.transform.tag == "Upgrade")
            {
                if (eventOn) return;
                UpgradeEventStart();
            }
            else
            {
                if (eventOn) return;
                eventOn = true;
                GameManager_JS.Instance.isEventOn = true;
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

                    // 이벤트 : 코인
                    case 1:
                        CoinEvent(tmpTypeIndex);
                        break;

                    // 이벤트 : 상점
                    case 2:
                        MerchantEventStart();
                        break;

                    // 이벤트 : 잼
                    case 3:
                        GemEvent(tmpTypeIndex);
                        break;
                    default:
                        break;
                }

                Destroy(hit.transform.gameObject, 0.01f);
            }
        }
    }

    // 능력 테스트용 함수
    public void AbilityTest(int _type)
    {
        if (eventOn) return;
        eventOn = true;
        GameManager_JS.Instance.isEventOn = true;

        // 현재 테스트중인 능력의 타입 인덱스
        tmpTypeIndex = _type;

        tmpName = namesArray[_type];
        ChoiceEventStart();
    }

    // 능력 선택 이벤트 시작
    void ChoiceEventStart()
    {
        // 선택 전체 UI 활성화
        events[0].SetActive(true);

        GameManager_JS.Instance.PanelOff();
        Time.timeScale = 0f;
        //player.SetActive(false);

        if (!GameManager_JS.Instance.dialogueChecks[tmpTypeIndex].IsEncounter)
        {
            event_Ability[0].SetActive(true);
            TextSet_Ability_Dialogue();
        }
        else
        {
            SwitchToChoice();
        }
    }

    // 업그레이드 이벤트 시작
    void UpgradeEventStart()
    {
        eventOn = true;
        GameManager_JS.Instance.isEventOn = true;
        eventUpgrade.SetActive(true);
        Time.timeScale = 0f;
    }

    // 업그레이드 이벤트 종료
    public void UpgradeEventEnd()
    {
        Time.timeScale = 1f;
        eventUpgrade.SetActive(false);
        eventOn = false;
        GameManager_JS.Instance.isEventOn = false;
    }

    public void SwitchToChoice()
    {
        // 대화 UI 비활성화
        event_Ability[0].SetActive(false);
        // 선택 UI 활성화
        event_Ability[1].SetActive(true);
        TextSet_Ability_Choice();
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
        event_Ability[1].SetActive(false);
        GameManager_JS.Instance.PanelOn();
        //player.SetActive(true);
        Time.timeScale = 1f;
        eventOn = false;
        GameManager_JS.Instance.isEventOn = false;
    }

    // 대화 내용 세팅
    void TextSet_Ability_Dialogue()
    {
        dialogueGetter = event_Ability[0].GetComponent<DialogueGetter>();

        // 이름 세팅
        dialogueGetter.objs[0].GetComponentInChildren<Text>().text = tmpName;

        int count = GameManager_JS.Instance.dialogueChecks[tmpTypeIndex].EncounterCount;

        // 대화 세팅. DB에 접근. 
        dialogues = EventDBManager.instance.TextDisplay_Ability_Dialogue(tmpTypeIndex, count + 1);

        GameManager_JS.Instance.dialogueChecks[tmpTypeIndex].EncounterCount += 1;
        GameManager_JS.Instance.dialogueChecks[tmpTypeIndex].IsEncounter = true;

        // 대화 진행
        DialogueContinue();
    }

    public void DialogueContinue()
    {
        if (dialogues.Count != 0)
        {
            string output = dialogues[0];
            output = output.Replace("'", ",");
            dialogueGetter.objs[1].GetComponentInChildren<Text>().text = output;
            dialogues.Remove(dialogues[0]);
        }
        else
        {
            SwitchToChoice();
        }
    }

    // 능력 선택지 세팅
    void TextSet_Ability_Choice()
    {
        choiceGetter = event_Ability[1].GetComponent<ChoiceGetter>();

        // 이름 세팅.
        choiceGetter.choices[0].GetComponentInChildren<Text>().text = tmpName;

        // 선택지 세팅. DB에 접근. 선택지 개수만큼 반복 수행.
        for (int i = 1; i < choiceGetter.choices.Count; i++)
        {
            for (int j = 0; j < choiceGetter.choices[i].childCount; j++)
            {
                // 선택지 양식 하나의 텍스트들을 변수에 입력
                texts.Add(choiceGetter.choices[i].GetChild(j));
            }

            // 해당 텍스트에 DB 데이터 입력.
            int[] tmp = EventDBManager.instance.TextDisplay_Ability_Choice(tmpTypeIndex, texts, i-1);
            int[] indexArray = new int[3] { tmpTypeIndex, tmp[0], tmp[1] };
            choiceGetter.choices[i].GetComponent<AbilityChoice>().SetChoiceValue(indexArray);
            texts.Clear();
        }
    }

    // 코인 획득 이벤트
    void CoinEvent(int value)
    {
        GameObject obj = Instantiate(events[1], player.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        obj.GetComponent<TextMesh>().text = "+" + value.ToString();
        obj.transform.SetParent(this.gameObject.transform.parent);
        Destroy(obj, 2f);
        GameManager_JS.Instance.Coin = value;
        eventOn = false;
        GameManager_JS.Instance.isEventOn = false;
    }

    // 잼 획득 이벤트
    void GemEvent(int value)
    {
        GameObject obj = Instantiate(events[1], player.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        obj.GetComponent<TextMesh>().text = "+" + value.ToString();
        obj.transform.SetParent(this.gameObject.transform.parent);
        Destroy(obj, 2f);
        GameManager_JS.Instance.Gem = GameManager_JS.Instance.Gem + value;
        eventOn = false;
        GameManager_JS.Instance.isEventOn = false;
    }

    void MerchantEventStart()
    {
        // 선택 전체 UI 활성화
        events[2].SetActive(true);
        GameManager_JS.Instance.PanelOff();
        Time.timeScale = 0f;
        //player.SetActive(false);
        TextSet_Merchant();
    }

    public void MerchantEventEnd()
    {
        events[2].SetActive(false);
        GameManager_JS.Instance.PanelOn();
        //player.SetActive(true);
        Time.timeScale = 1f;
        eventOn = false;
        GameManager_JS.Instance.isEventOn = false;
    }

    void TextSet_Merchant()
    {
        itemFormGetter = event_Merchant[0].GetComponent<ItemFormGetter>();

        // 코인 세팅
        itemFormGetter.objs[1].GetComponent<Text>().text = "Coin : " + GameManager_JS.Instance.Coin.ToString();
        List<ShopItem> shopItemList = new List<ShopItem>();

        // 상품 세팅. DB에 접근. 선택지 개수만큼 반복 수행.
        for (int i = 3; i < itemFormGetter.objs.Count; i++)
        {
            for (int j = 0; j < itemFormGetter.objs[i].childCount; j++)
            {
                // 선택지 양식 하나의 텍스트들을 변수에 입력
                texts.Add(itemFormGetter.objs[i].GetChild(j));
            }
            
            // 해당 텍스트에 DB 데이터 입력.
            shopItemList.Add(EventDBManager.instance.MerchantTextDisplay(texts));
            texts.Clear();
        }

        itemFormGetter.AddShopItem(shopItemList);
    }
}
