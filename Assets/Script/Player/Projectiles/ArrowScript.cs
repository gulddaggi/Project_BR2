using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : PlayerProjectile
{
    protected override void Start()
    {
        base.Start();


        if (GameManager_JS.Instance.curStage.ToString().StartsWith("Boss"))
        {
            FindObjectOfType<Harpy>().destroyProjectileDelegate += DestroyProjectile;
        }
        else
        {
            FindObjectOfType<Enemy>().destroyProjectileDelegate += DestroyProjectile;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other); 
    }

    void DestroyProjectile(GameObject projectile)
    {
        if (projectile == gameObject)
        {
            // Arrow 파괴 코드
            Destroy(gameObject);
        }
    }
}