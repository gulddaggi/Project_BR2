using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfSpider : Enemy
{
    public override void Start()
    {
        base.Start();
    }

    IEnumerator GetDamaged()
    {
        SR.material.color = Color.red;

        yield return new WaitForSeconds(0.6f);
        SR.material.color = Color.white;
        isHit = false;

    }
}
    