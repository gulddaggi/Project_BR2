using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float moveSpeed;
    public float alphaSpeed;
    public float destroyTime;

    public TextMeshPro textHP;

    Color alpha;

    public int damage;

    private void Start()
    {
        textHP = GetComponent<TextMeshPro>();
        textHP.text = damage.ToString();
        alpha = textHP.color;

        Invoke("DestroyObject", destroyTime);
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        textHP.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
