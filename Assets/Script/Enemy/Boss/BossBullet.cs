using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    public float bulletSpeed = 1.5f; // 투사체 속도
    public Rigidbody rb;

    // Start is called before the first frame update
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Self_Destroy());
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * bulletSpeed);
    }

    IEnumerator Self_Destroy() // 3초 후에 unabled된 후 풀로 복귀
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
}
