using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DB 파일 임포트 클래스
public class DBImporter : MonoBehaviour
{
    // 능력 타입 별 개수 저장 배열
    int[] abilityCountArr = { 0, 0, 0 };

    // 전달받은 파일 명의 능력 DB 파일 임포트를 수행.
    public Ability[] DBImport_Ability(string _DBFileName)
    {
        List<Ability> abliityContentList = new List<Ability>(); // 선택 양식 리스트
        TextAsset DBData = Resources.Load<TextAsset>(_DBFileName); // csv 파일 변수 저장

        string[] data = DBData.text.Split(new char[] { '\n' }); // 엔터 단위로 행 구분

        for (int i = 0; i < abilityCountArr.Length; i++)
        {
            string[] row = data[0].Split(new char[] { ',' }); // 콤마 단위로 각 항목 구분
            abilityCountArr[i] = int.Parse(row[i + 6]);  // DB에 기록되어있는 개수를 전달
        }

        // 항목별 저장 수행
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); // 콤마 단위로 각 항목 구분
            Ability ability = new Ability(); // 선택 지문 객체 생성

            // 객체에 데이터 삽입
            ability.type = row[0]; // 타입
            ability.ability_name = row[1]; // 능력명
            ability.description = row[2]; // 설명
            ability.option_Name = row[3]; // 옵션명
            ability.plus_Value = row[4].Split('/'); // 가산 수치. 등급별로 값을 구분하여 삽입
            ability.level = 0;

            abliityContentList.Add(ability); // 배열 변환을 위해 리스트에 저장
        }

        return abliityContentList.ToArray(); // 데이터가 저장된 리스트를 배열로 변환하여 반환
    }

    public Dialogue[] DBImporter_Dialogue(string _DBFileName)
    {
        List<Dialogue> dialogueContentList = new List<Dialogue>(); // 선택 양식 리스트
        TextAsset DBData = Resources.Load<TextAsset>(_DBFileName); // csv 파일 변수 저장

        string[] data = DBData.text.Split(new char[] { '\n' }); // 엔터 단위로 행 구분
                                                                // 항목별 저장 수행
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); // 콤마 단위로 각 항목 구분
            Dialogue dialogue = new Dialogue(); // 선택 지문 객체 생성

            dialogue.ID = row[0]; // ID. 던전 진입 후 처음 마주친 횟수
            dialogue.texts = row[1].Split('/'); // 대사. /를 행구분 값으로 사용하여 삽입

            dialogueContentList.Add(dialogue); // 배열 변환을 위해 리스트에 저장
        }

        return dialogueContentList.ToArray(); // 데이터가 저장된 리스트를 배열로 변환하여 반환
    }

    // 타입별 능력 개수 반환
    public void GetCount(int[] _arr)
    {
        for (int i = 0; i < abilityCountArr.Length; i++)
        {
            _arr[i] = abilityCountArr[i];
        }
    }
}
