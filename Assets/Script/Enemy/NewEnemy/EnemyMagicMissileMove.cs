using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagicMissileMove : MonoBehaviour
{
    public LivingEntity targetEntity;//공격 대상

    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    private SphereCollider sphCollider;
    public GameObject[] Detached;
    private float lastCollisionEnterTime;
    private float collisionDealy = 0.1f;

    //매직 미사일 고정 데미지, 
    public float damage;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphCollider = GetComponent<SphereCollider>();
        EnemyShaman enemyShaman = GameObject.Find("Enemy_Goblin Shaman").GetComponent<EnemyShaman>();
        damage = enemyShaman.damage;

        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity); //Quaternion.identity 회전 없음
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);   //ParticleSystem의 main.duration, 기본 시간인듯, duration은 따로 값을 정할 수 있음
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }

        Destroy(gameObject, 5);
    }

    //매직 미사일 이동 기능
    void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = transform.forward * speed; //타겟팅 대상이 없을 때 매직 미사일은 전방으로 날아간다
        }
    }

    private void Update()
    {
        OnSphereCollider();
    }

    void OnCollisionEnter(Collision collision)  //매직미사일이 충돌했을 경우
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            speed = 0;

            //상대방의 LivingEntity 타입 가져오기, 데미지를 적용하기 위한 준비
            LivingEntity attackTarget = collision.gameObject.GetComponent<LivingEntity>();

            Debug.Log("충돌한 오브젝트의 레이어" + collision.gameObject.layer + "충돌한 시간" + lastCollisionEnterTime);

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point + contact.normal * hitOffset;

            if (hit != null)
            {
                var hitInstance = Instantiate(hit, pos, rot);
                if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
                else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
                else { hitInstance.transform.LookAt(contact.point + contact.normal); }

                var hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                }
            }
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {
                    detachedPrefab.transform.parent = null;
                }
            }
            Destroy(gameObject);

            //데미지 처리
            attackTarget.OnDamage(damage);
            Debug.Log("현재 데미지" + damage);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            speed = 0;

            Debug.Log("충돌한 오브젝트의 레이어" + collision.gameObject.layer + "충돌한 시간" + lastCollisionEnterTime);

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point + contact.normal * hitOffset;

            if (hit != null)
            {
                var hitInstance = Instantiate(hit, pos, rot);
                if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
                else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
                else { hitInstance.transform.LookAt(contact.point + contact.normal); }

                var hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else
                {
                    var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitInstance, hitPsParts.main.duration);
                }
            }
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {
                    detachedPrefab.transform.parent = null;
                }
            }
            Destroy(gameObject);
        }

        else
        {
            sphCollider.enabled = false;
            Debug.Log("충돌한 오브젝트의 레이어" + collision.gameObject.layer + "충돌한 시간" + lastCollisionEnterTime);
        }
    }

    void OnSphereCollider()
    {
        if (lastCollisionEnterTime + collisionDealy < Time.time)
        {
            sphCollider.enabled = true;
            lastCollisionEnterTime = Time.time;
            Debug.Log("콜라이더 켜짐");
        }
    }
}