using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    public float bulletSpeed = 1.0f;
    public Rigidbody rb;

    // Start is called before the first frame update
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * bulletSpeed);
    }

    IEnumerator Self_Destroy()
    {
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
}
