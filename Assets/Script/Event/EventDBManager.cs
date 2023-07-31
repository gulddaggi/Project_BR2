using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDBManager : MonoBehaviour
{
    public static EventDBManager instance;

    [SerializeField]
    string DBFileName;

    //import할 능력 DB 목록
    [SerializeField]
    string[] DBFiles_Ability;

    //import할 대화 DB 목록
    [SerializeField]
    string[] DBFiles_Dialogue;

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

    // 능력 타입 별 개수 저장 배열
    [SerializeField]
    int[] abilityCountArr = new int[3]{0, 0, 0};

    AbilitySelector abilitySelector;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DBImporter DBImporter = this.gameObject.GetComponentInChildren<DBImporter>();
            abilitySelector = this.gameObject.GetComponentInChildren<AbilitySelector>();

            // 능력 DB 전체 임포트 수행.
            Import_Ability(DBImporter);

            // 대화 DB 전체 임포트 수행.
            Import_Dialogue(DBImporter);

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
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
        int[] selected = abilitySelector.Select(index-1, abilityCountArr[index-1]);
        int line = selected[0];
        int grade = selected[1];

        // 이후 개발 시에는 지정된 ID
        format[0].GetComponent<Text>().text = totalAbilityDic[ab_index][line].ability_name;
        format[1].GetComponent<Text>().text = totalAbilityDic[ab_index][line].description;
        format[2].GetComponent<Text>().text = totalAbilityDic[ab_index][line].option_Name;
        format[3].GetComponent<Text>().text = "+" + totalAbilityDic[ab_index][line].plus_Value[grade] + "%";
        Debug.Log(totalAbilityDic[ab_index][line].plus_Value[grade]);
        returnArray[0] = StatIndex(line);
        Debug.Log(returnArray[0]);
        returnArray[1] = int.Parse(totalAbilityDic[ab_index][line].plus_Value[grade]);

        return returnArray;
    }

    // 적용 능력치 인덱스 지정. 테스트용
    int StatIndex(int _value)
    {
        // 0, 1
        if (_value < 2)
        {
            return 0;
        }
        // 2, 3
        else if (_value >= 2 && _value < 4)
        {
            return 1;
        }
        //4, 5
        else if(_value >= 4 && _value < 6)
        {
            return 2;
        }
        // 6
        else if(_value >= 6)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }
}
