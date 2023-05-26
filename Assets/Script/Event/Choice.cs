using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice : MonoBehaviour
{
    [Tooltip("ID")]
    public string ID;


    [Tooltip("능력명")]
    public string choice_Name;

    [Tooltip("설명")]
    public string description;

    [Tooltip("가산 수치")]
    public string plus_Value;
}
