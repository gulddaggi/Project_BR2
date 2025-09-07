using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Player playerstatus;

    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
