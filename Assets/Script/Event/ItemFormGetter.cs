using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFormGetter : MonoBehaviour
{
    public List<Transform> objs = new List<Transform>();

    private void Awake()
    {
        // 대화 내용 세팅에 필요한 자식 오브젝트를 리스트에 저장. 순서대로 이름 - 대사.
        for (int i = 0; i < transform.childCount; i++)
        {
            objs.Add(this.transform.GetChild(i));
        }
    }
}
