using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기존 능력 스크립트. 변경 이후에 삭제 예정.
[System.Serializable]
public class Choice : MonoBehaviour
{
    [Tooltip("능력명")]
    public string choice_Name;

    [Tooltip("설명")]
    public string description;

    [Tooltip("적용 옵션")]
    public string option_Name;

    [Tooltip("가산 수치")]
    public string plus_Value;
}
