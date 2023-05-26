using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceGetter : MonoBehaviour
{

    public Button[] choices;

    void Start()
    {
        choices = GetComponentsInChildren<Button>();
    }
}
