using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코인 획득 텍스트 이동 수행.
public class TextMove : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.Translate(Vector3.up * Time.deltaTime);
    }
}
