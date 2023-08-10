using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    void Start()
    {
        StartCoroutine(Self_Destruct());
    }

    IEnumerator Self_Destruct()
    {
        yield return new WaitForSeconds(1.0f);
        Pool.Release(this.gameObject);
    }
}