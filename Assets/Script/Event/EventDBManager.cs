using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDBManager : MonoBehaviour
{
    public static EventDBManager instance;

    [SerializeField]
    string DBFileName;

    Dictionary<int, Choice> choiceDic = new Dictionary<int, Choice>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DBImporter DBimporter = GetComponent<DBImporter>();
            Choice[] choices = DBimporter.DBImport(DBFileName);

            for (int i = 0; i < choices.Length; i++)
            {
                choiceDic.Add(i + 1, choices[i]);

            }
        }
    }

    // 선택지 텍스트 표현
    public void ChoiceTextDisplay(Text[] format, int index)
    {
        // 이후 개발 시에는 지정된 ID
        format[0].text = choiceDic[index].ID;
        format[1].text = choiceDic[index].choice_Name;
        format[2].text = choiceDic[index].description;
        format[3].text = choiceDic[index].plus_Value;
    }
}
