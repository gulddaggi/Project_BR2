using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public enum BulletSort { smallBullet, BigBullet };

    public float bulletSpeed = 1.5f; // 투사체 속도
    public float BulletDamage = 5.0f;
    public Rigidbody rb;

    // Start is called before the first frame update

    public virtual void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Self_Destroy());
    }

    public virtual void FixedUpdate()
    {
        rb.AddForce(transform.forward * bulletSpeed);
    }

    IEnumerator Self_Destroy() // 3초 후에 unabled된 후 풀로 복귀
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerdata = other.transform.GetComponentInParent<Player>();
            playerdata.CurrentHP -= BulletDamage;
            gameObject.SetActive(false);
        }
        else if(other.tag == "PlayerAttack")
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
}
