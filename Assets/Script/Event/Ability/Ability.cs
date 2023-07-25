using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 새로 사용되는 능력 스크립트
public class Ability : MonoBehaviour
{
    [Tooltip("능력 타입")]
    public string type;

    [Tooltip("능력명")]
    public string ability_name;

    [Tooltip("설명")]
    public string description;

    [Tooltip("적용 옵션")]
    public string option_Name;

    [Tooltip("가산 수치")]
    public string[] plus_Value; // 등급별로 수치가 나눠져 배열로

    [Tooltip("레벨")]
    public int level;

    public void LevelUp()
    {
        level++;
    }

    public void LevelInit()
    {
        level = 0;
    }
}
