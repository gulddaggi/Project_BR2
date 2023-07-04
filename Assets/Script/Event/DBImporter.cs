using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DB 파일 임포트 클래스
public class DBImporter : MonoBehaviour
{
    // 전달받은 파일 명의 DB 파일 임포트를 수행.
    public Choice[] DBImport(string _DBFileName)
    {
        List<Choice> choiceContentList = new List<Choice>(); // 선택 양식 리스트
        TextAsset DBData = Resources.Load<TextAsset>(_DBFileName); // csv 파일 변수 저장

        string[] data = DBData.text.Split(new char[] { '\n' }); // 엔터 단위로 행 구분

        // 항목별 저장 수행
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); // 콤마 단위로 각 항목 구분
            Choice choice = new Choice(); // 선택 지문 객체 생성

            // 객체에 데이터 삽입
            choice.choice_Name = row[1]; // 능력명
            choice.description = row[2]; // 능력 설명
            choice.option_Name = row[3]; // 옵션명
            choice.plus_Value = row[4]; // 가산 수치

            choiceContentList.Add(choice); // 배열 변환을 위해 리스트에 저장
        }

        /*
        foreach (Choice item in choiceContentList)
        {
            Debug.Log(item.choice_Name);
            Debug.Log(item.description);
            Debug.Log(item.option_Name);
            Debug.Log(item.plus_Value);
            Debug.Log("-------------");
        }
        */

        return choiceContentList.ToArray(); // 데이터가 저장된 리스트를 배열로 변환하여 반환
    }
}
