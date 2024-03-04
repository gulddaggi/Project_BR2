using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfSpider : Enemy
{
    protected override void Start()
    {
        base.Start();
        animator.applyRootMotion = false;
    }

    protected override void Update()
    {
        base.Update();

        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    IEnumerator GetDamaged()
    {
        SR.material.color = Color.red;

        yield return new WaitForSeconds(0.6f);
        SR.material.color = Color.white;
        isHit = false;

    }
}
    