using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenScript : PlayerProjectile
{
    protected override void Start()
    {
        base.Start();

        // FindObjectOfType<Enemy>().destroyProjectileDelegate += DestroyProjectile;
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
