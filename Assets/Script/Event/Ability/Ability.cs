using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 새로 사용되는 능력 스크립트
public class Ability : MonoBehaviour
{
    // 능력 DB 상 ID
    public string ID;

    // 능력 티어
    public string tier;

    // 능력명
    public string ability_name;

    // 능력 설명
    public string description;

    // 능력치 적용 옵션
    public string option;

    // 가산 수치
    public string[] plus_Value; // 등급별로 수치가 나눠져 배열로 선언

    // 부가 설명. 부여되는 상태이상 효과 설명
    public string sub_Description;

    // 선행 능력
    public string[] pre_abilities;

    // 능력 보유 여부
    public bool isSelected = false;

    // 레벨
    public int level;

    // 단위
    public string unit;

    public string plus_Value_Done;

    public string rank;

    public void LevelUp()
    {
        level++;
    }

    public void LevelInit()
    {
        level = 0;
    }

    // 능력 선택 시 발생하는 이벤트.
    public UnityEvent OnSelected;


}
