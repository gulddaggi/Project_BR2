using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    
    void Start()
    {
        Invoke("ObjectActive", 70f);
        Invoke("DestroyObject", 70f);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    void ObjectActive()
    {
        objectToActivate.SetActive(true);
    }
}
