using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EventDBManager : MonoBehaviour
{
    public static EventDBManager instance;

    //import할 능력 DB 목록
    [SerializeField]
    string[] DBFiles_Ability;

    //import할 대화 DB 목록
    [SerializeField]
    string[] DBFiles_Dialogue;

    //import할 상점 DB 목록
    [SerializeField]
    string DBFiles_Merchant;

    // 한 종류(정령)의 선택지 딕셔너리를 담는 딕셔너리. 전체 능력에 대한 자료구조.
    [SerializeField]
    Dictionary<int, Dictionary<int, Ability>> totalAbilityDic = new Dictionary<int, Dictionary<int, Ability>>();

    // 선택지 객체들을 담는 딕셔너리. 
    [SerializeField]
    Dictionary<int, Ability> abilityDic;

    // 한 종류(정령)의 대화 딕셔너리를 담는 딕셔너리. 전체 대화에 대한 자료구조.
    [SerializeField]
    Dictionary<int, Dictionary<int, Dialogue>> totalDialogueDic = new Dictionary<int, Dictionary<int, Dialogue>>();

    // 대화 객체들을 담는 딕셔너리.
    [SerializeField]
    Dictionary<int, Dialogue> dialogueDic;

    // 상점 객체 딕셔너리
    [SerializeField]
    Dictionary<int, ShopItem> merchantDic = new Dictionary<int, ShopItem>();

    // 능력 타입 별 개수 저장 배열
    [SerializeField]
    int[] abilityCountArr = new int[3]{0, 0, 0};

    AbilitySelector abilitySelector;
    List<int> lineList;
    List<int> indexList = new List<int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DBImporter DBImporter = this.gameObject.GetComponentInChildren<DBImporter>();
            abilitySelector = this.gameObject.GetComponentInChildren<AbilitySelector>(); // 콜백으로 바꿔야함

            // 능력 DB 전체 임포트 수행.
            Import_Ability(DBImporter);

            // 대화 DB 전체 임포트 수행.
            Import_Dialogue(DBImporter);

            // 상점 DB 임포트 수행.
            Import_Merchant(DBImporter);

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    // 능력 DB 전체 임포트
    void Import_Ability(DBImporter _DBImporter)
    {
        for (int i = 0; i < DBFiles_Ability.Length; i++)
        {
            Ability[] abilityArray = _DBImporter.DBImport_Ability(DBFiles_Ability[i]); // 한 종류의 능력 데이터 전부를 임포트하여 반환받음.

            abilityDic = new Dictionary<int, Ability>();

            for (int j = 0; j < abilityArray.Length; j++)
            {
                abilityDic.Add(j, abilityArray[j]); // 임포트된 능력 하나씩 담당 딕셔너리에 삽입
            }

            totalAbilityDic.Add(i, abilityDic);
        }

        _DBImporter.GetCount(abilityCountArr);
    }

    // Listener 등록 함수
    public void AddListenerPlaerStatus(Player player)
    {
        for (int i = 0; i < totalAbilityDic.Count; i++)
        {
            for (int j = 0; j < totalAbilityDic[i].Count; j++)
            {
                int tmpi = 0;
                int tmpj = int.Parse(totalAbilityDic[i][j].ID);
                Debug.Log(totalAbilityDic[i][j].ID);
                totalAbilityDic[i][j].OnSelected.AddListener(() => player.AbilitySelected(tmpi, tmpj));
                Debug.Log(i + "," + j + "이벤트 등록 완료");
            }
        }
    }

    // 대화 DB 전체 임포트
    void Import_Dialogue(DBImporter _DBImporter)
    {
        for (int i = 0; i < DBFiles_Dialogue.Length; i++)
        {
            Dialogue[] dialogueArray = _DBImporter.DBImporter_Dialogue(DBFiles_Dialogue[i]);

            dialogueDic = new Dictionary<int, Dialogue>();

            for (int j = 0; j < dialogueArray.Length; j++)
            {
                dialogueDic.Add(j, dialogueArray[j]);
            }

            totalDialogueDic.Add(i, dialogueDic);
        }
    }

    //상정 DB 임포트
    void Import_Merchant(DBImporter _DBImporter)
    {
        ShopItem[] shopItemArray = _DBImporter.DBImporter_Merchant(DBFiles_Merchant);
        for (int i = 0; i < shopItemArray.Length; i++)
        {
            merchantDic.Add(i, shopItemArray[i]);
        }
        lineList = Enumerable.Range(0, merchantDic.Count - 1).ToList();
    }

    // 대화 텍스트 출력.
    public List<string> TextDisplay_Ability_Dialogue(int dia_index, int _id)
    {
        List<string> dialogues = new List<string>();

        for (int i = 0; i < totalDialogueDic[dia_index][_id-1].texts.Length; i++)
        {
            dialogues.Add(totalDialogueDic[dia_index][_id - 1].texts[i]);
        }

        return dialogues;
    }

    // 선택지 텍스트 출력.
    public int[] TextDisplay_Ability_Choice(int ab_index, List<Transform> format, int index)
    {
        int[] returnArray = new int[2];

        // 추출 대상 인덱스와 가산 수치.
        int[] selected = abilitySelector.Select(index, abilityCountArr[0]); // 추출 대상 수정 함수 호출. 수정 필요.
        int line = selected[0];
        int grade = selected[1];

        // 이후 개발 시에는 지정된 ID
        format[0].GetComponent<Text>().text = totalAbilityDic[ab_index][line].ability_name;
        format[1].GetComponent<Text>().text = totalAbilityDic[ab_index][line].description;
        format[2].GetComponent<Text>().text = totalAbilityDic[ab_index][line].option;
        format[3].GetComponent<Text>().text = "+" + totalAbilityDic[ab_index][line].plus_Value[grade] + "%";
        SetRank(format[4], grade);
        if(totalAbilityDic[ab_index][line].sub_Description != "")
        {
            format[5].gameObject.SetActive(true);
            format[5].GetComponentInChildren<Text>().text = totalAbilityDic[ab_index][line].sub_Description;
        }

        returnArray[0] = line;
        returnArray[1] = int.Parse(totalAbilityDic[ab_index][line].plus_Value[grade]);

        return returnArray;
    }

    // 적용 능력치 인덱스 지정. DB 파일 완성 이후 변수 구분 기능 수정 필요.
    int StatIndex(int _index, int _value)
    {
        if (_index == 0 || _index == 1)
        {
            return _index;
        }
        else
        {
            // 4, 5
            if (_value >= 4 && _value < 6)
            {
                return 2;
            }
            // 6
            else
            {
                return 3;
            }
        }
    }

    void SetRank(Transform transform, int grade)
    {
        Text tr_Text = transform.GetComponent<Text>();

        switch (grade)
        {
            case 0:
                tr_Text.text = "일반";
                break;

            case 1:
                tr_Text.text = "희귀";
                tr_Text.color = Color.blue;
                break;

            case 2:
                tr_Text.text = "영웅";
                tr_Text.color = new Color(195f, 0f, 173f, 255f);
                break;

            default:
                break;
        }
    }

    // 상품 텍스트 출력.
    public ShopItem TextDisplay_And_ClassReturn_Merchant(List<Transform> format)
    {
        // 추출 대상 인덱스와 가산 수치.
        int line = ReturnRandom();
        // lineList.Remove(line);

        // 이후 개발 시에는 지정된 ID
        format[0].GetComponent<Text>().text = merchantDic[line].item_Name;
        format[1].GetComponent<Text>().text = merchantDic[line].description;
        format[2].GetComponent<Text>().text = merchantDic[line].option_Name;
        format[3].GetComponent<Text>().text = Value(merchantDic[line].value);
        if (merchantDic[line].turn == "0") format[4].GetComponent<Text>().text = "즉시";
        else format[4].GetComponent<Text>().text = merchantDic[line].turn;
        format[5].GetComponent<Text>().text = "- " + merchantDic[line].price.ToString();
        ShopItem shopItem = merchantDic[line];
        return shopItem;
    }

    // 인덱스 추출. 바꿔야함.
    public int ReturnRandom()
    {
        while (true)
        {
            int tmp = Random.Range(0, merchantDic.Count);
            if (!indexList.Contains(tmp))
            {
                indexList.Add(tmp);
                return tmp;
            }
        }
    }

    private string Value(string _value)
    {
        string returnValue = "+ ";
        Debug.Log(_value[_value.Length - 1]);
        if (_value[_value.Length - 1] == '!')
        {
            for (int i = 0; i < _value.Length - 1; i++)
            {
                returnValue += _value[i];
            }
        }
        else
        {
            returnValue += _value + "%";
        }

        return returnValue;
    }

    // 이벤트 종료 후 초기화
    public void InitData()
    {
        lineList.Clear();
        // 리스트 다시 채우기
        lineList = Enumerable.Range(0, merchantDic.Count - 1).ToList();
        indexList.Clear();
    }

    public int GetTotalDicNum()
    {
        return totalAbilityDic.Count;
    }

    public int[] GetTotalIDNum()
    {
        int[] tmp = new int[totalAbilityDic.Count];
        for (int i = 0; i < totalAbilityDic.Count; i++)
        {
            tmp[i] = totalAbilityDic[i].Count;
        }
        return tmp;
    }
}
