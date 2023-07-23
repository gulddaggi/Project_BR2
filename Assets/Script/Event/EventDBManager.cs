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
    Dictionary<int, Dictionary<int, Ability>> choiceDic = new Dictionary<int, Dictionary<int, Ability>>();

    [SerializeField]
    Dictionary<int, Ability> eventDic;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DBImporter DBimporter = GetComponent<DBImporter>();


            for (int i = 0; i < DBFiles.Length; i++)
            {
                // 전체 이벤트 DB 임포트 시작. 지금은 능력만.
                Ability[] abilityArray = DBimporter.DBImport(DBFiles[i]); // 한 종류의 능력 데이터 전부를 임포트하여 반환받음.

                eventDic = new Dictionary<int, Ability>();

                for (int j = 0; j < abilityArray.Length; j++)
                {
                    eventDic.Add(j, abilityArray[j]); // 임포트된 능력 하나씩 담당 딕셔너리에 삽입
                }

                choiceDic.Add(i, eventDic);
            }
        }
    }

    // 선택지 텍스트 표현. 이후에는 추첨된 능력을 출력해야함.
    public void TextDisplay_Ability(List<Transform> format, int index) // 인덱스 변경 필요
    {
        // 이후 개발 시에는 지정된 ID
        format[0].GetComponent<Text>().text = choiceDic[0][index].name;
        format[1].GetComponent<Text>().text = choiceDic[0][index].description;
        format[2].GetComponent<Text>().text = choiceDic[0][index].option_Name;
        format[3].GetComponent<Text>().text = choiceDic[0][index].plus_Value[0];
    }
}
