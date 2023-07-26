using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDBManager : MonoBehaviour
{
    public static EventDBManager instance;

    [SerializeField]
    string DBFileName;

    //import할 파일 목록
    [SerializeField]
    string[] DBFiles;

    // import된 선택지 객체를 담는 딕셔너리
    [SerializeField]
    Dictionary<int, Dictionary<int, Ability>> abilityDic = new Dictionary<int, Dictionary<int, Ability>>();

    [SerializeField]
    Dictionary<int, Ability> eventDic;

    // 능력 타입 별 개수 저장 배열
    [SerializeField]
    int[] abilityCountArr = new int[3]{0, 0, 0};

    AbilitySelector abilitySelector;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DBImporter DBimporter = GetComponent<DBImporter>();
            abilitySelector = this.gameObject.transform.parent.GetComponentInChildren<AbilitySelector>();

            // 모든 DB 임포트를 위해 반복. DB별로 형식이 달라 개별적으로 해야할수도
            for (int i = 0; i < DBFiles.Length; i++)
            {
                // 전체 이벤트 DB 임포트 시작. 지금은 능력만.
                Ability[] abilityArray = DBimporter.DBImport(DBFiles[i]); // 한 종류의 능력 데이터 전부를 임포트하여 반환받음.

                eventDic = new Dictionary<int, Ability>();

                for (int j = 0; j < abilityArray.Length; j++)
                {
                    eventDic.Add(j, abilityArray[j]); // 임포트된 능력 하나씩 담당 딕셔너리에 삽입
                }

                abilityDic.Add(i, eventDic);
            }

            DBimporter.GetCount(abilityCountArr);
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
        format[0].GetComponent<Text>().text = abilityDic[ab_index][line].ability_name;
        format[1].GetComponent<Text>().text = abilityDic[ab_index][line].description;
        format[2].GetComponent<Text>().text = abilityDic[ab_index][line].option_Name;
        format[3].GetComponent<Text>().text = "+" + abilityDic[ab_index][line].plus_Value[grade] + "%";
    }
}
