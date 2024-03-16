using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destory : MonoBehaviour
{
    public float delay = 2f;

    void Start()
    {
        // delay 초 후에 자신을 파괴
        Destroy(gameObject, delay);
    }
}
