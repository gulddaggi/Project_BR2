using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 불 타입 능력 불의 가호 클래스
public class FlameAbilityField : MonoBehaviour
{
    int count = 0;
    public float damage = 0.0f;

    [SerializeField]
    LayerMask layerMask;

    private void OnEnable()
    {
        FieldOn();
    }

    private void FieldOn()
    {
        Collider[] cols = Physics.OverlapSphere(this.gameObject.transform.parent.position, 10f, layerMask);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == "Enemy")
            {
                cols[i].GetComponent<Enemy>().playerdata = this.GetComponentInParent<Player>();
                cols[i].GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        if (count >= 10)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            ++count;
            Invoke("FieldOn", 1f);
        }
    }

    private void OnDisable()
    {
        count = 0;
        damage = 0.0f;
    }
}
