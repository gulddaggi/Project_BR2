using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
