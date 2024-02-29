using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DB 파일 임포트 클래스
public class DBImporter : MonoBehaviour
{
    // 티어 별 능력 개수 저장 배열
    int[] abilityCountArr = { 0, 0, 0 };

    // 전달받은 파일 명의 능력 DB 파일 임포트를 수행
    public Ability[] DBImport_Ability(string _DBFileName)
    {
        List<Ability> abliityContentList = new List<Ability>(); // 선택 양식 리스트
        TextAsset DBData = Resources.Load<TextAsset>(_DBFileName); // csv 파일 변수 저장

        string[] data = DBData.text.Split(new char[] { '\n' }); // 엔터 단위로 행 구분
        
        // 첫 행에 기록되어있는 티어 별 능력 개수 저장
        string[] row_first = data[0].Split(new char[] { ',' }); // 콤마 단위로 첫 행의 항목 구분
        for (int i = 0; i < abilityCountArr.Length; i++)
        {
            abilityCountArr[i] = int.Parse(row_first[i + 8]);  // DB에 기록되어있는 개수를 전달
        }

        // 항목별 저장 수행
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); // 콤마 단위로 각 항목 구분
            Ability ability = new Ability(); // 선택 지문 객체 생성

            // 객체에 데이터 삽입
            ability.ID = row[0]; // ID
            ability.tier = row[1]; // 티어
            ability.ability_name = row[2]; // 능력명
            ability.description = row[3]; // 설명
            ability.option = row[4]; // 옵션명
            ability.plus_Value = row[5].Split('/'); // 가산 수치. 등급별로 값을 구분하여 삽입
            ability.sub_Description = row[6]; // 부가 설명. 이후에 UI에 적용 필요.
            ability.pre_abilities = row[7].Split('/'); // 선행 능력. UI 표시 없이 시스템 단위에서 사용.
            for (int j = 0; j < ability.pre_abilities.Length; j++)
            {
                Debug.Log(ability.ability_name + "의" + (j + 1) + "번째 선행능력 : " + ability.pre_abilities[j]);
               
            }
            //ability.OnSelected.AddListener(() => ) 여기서 추가해야됨. 딕셔너리에서는 값 수정 불가.

            // 레벨 및 선택 여부 초기화
            ability.level = 0;
            ability.isSelected = false;

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

    public ShopItem[] DBImporter_Merchant(string _DBFileName)
    {
        List<ShopItem> shopItemList = new List<ShopItem>(); // 상점 품목 리스트
        TextAsset DBData = Resources.Load<TextAsset>(_DBFileName); // csv 파일 변수 저장

        string[] data = DBData.text.Split(new char[] { '\n' }); // 엔터 단위로 행 구분

        // 항목별 저장 수행
        for (int i = 1; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' }); // 콤마 단위로 각 항목 구분
            ShopItem shopItem = new ShopItem(); // 선택 지문 객체 생성

            // 객체에 데이터 삽입
            shopItem.item_Name = row[1]; // 이름
            shopItem.description = row[2]; // 설명
            shopItem.option_Name = row[3]; // 적용 옵션
            shopItem.eventType = int.Parse(row[4]); // 이벤트타입
            shopItem.value = row[5]; // 가산 수치. % 단위 사용 여부 구분 필요
            shopItem.turn = row[6]; // 지속 턴 수 
            shopItem.price = int.Parse(row[7]); // 가격

            shopItemList.Add(shopItem); // 배열 변환을 위해 리스트에 저장
        }

        return shopItemList.ToArray(); // 데이터가 저장된 리스트를 배열로 변환하여 반환
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
