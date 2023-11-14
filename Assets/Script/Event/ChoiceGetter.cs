using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceGetter : MonoBehaviour
{
    public List<Transform> choices = new List<Transform>();

    [SerializeField]
    AbilityListManager aLManager;

    void Awake()
    {
        // 자식 오브젝트를 리스트에 저장. 0은 Title.
        for (int i = 0; i < transform.childCount; i++)
        {
            choices.Add(this.transform.GetChild(i));
        }
    }
}
