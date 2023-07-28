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
            DBImporter DBImporter = GetComponent<DBImporter>();
            abilitySelector = this.gameObject.transform.parent.GetComponentInChildren<AbilitySelector>();

            // 능력 DB 전체 임포트 수행.
            Import_Ability(DBImporter);

            // 대화 DB 전체 임포트 수행.
            Import_Dialogue(DBImporter);
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

    // 선택지 텍스트 표현. 이후에는 추첨된 능력을 출력해야함.
    public void TextDisplay_Ability(int ab_index, List<Transform> format, int index)
    {
        // 추출 대상 인덱스와 가산 수치.
        int[] selected = abilitySelector.Select(index, abilityCountArr[index]);
        int line = selected[0];
        int grade = selected[1];

        // 이후 개발 시에는 지정된 ID
        format[0].GetComponent<Text>().text = totalAbilityDic[ab_index][line].ability_name;
        format[1].GetComponent<Text>().text = totalAbilityDic[ab_index][line].description;
        format[2].GetComponent<Text>().text = totalAbilityDic[ab_index][line].option_Name;
        format[3].GetComponent<Text>().text = "+" + totalAbilityDic[ab_index][line].plus_Value[grade] + "%";
    }
}
