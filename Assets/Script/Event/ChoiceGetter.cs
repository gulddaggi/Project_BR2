using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceGetter : MonoBehaviour
{
    public List<Transform> choices = new List<Transform>();

    void Awake()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            choices.Add(this.transform.GetChild(i));
        }
    }
}
