using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceGetter : MonoBehaviour
{
    public List<Transform> choices = new List<Transform>();

    void Awake()
    {
        // 자식 오브젝트를 리스트에 저장. 0은 Title.
        for (int i = 0; i < transform.childCount; i++)
        {
            choices.Add(this.transform.GetChild(i));
        }
    }

    private void OnEnable()
    {
        for (int i = 1; i < choices.Count; i++)
        {
            choices[i].GetChild(4).GetComponent<Text>().color = Color.black;
            choices[i].GetChild(5).gameObject.SetActive(false);
        }
    }
}
