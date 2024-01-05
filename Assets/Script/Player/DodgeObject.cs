using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeObject : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("콜라이더 on");
        Invoke("ObjOff", 0.5f);
    }

    void ObjOff()
    {
        Debug.Log("콜라이더 off");
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
