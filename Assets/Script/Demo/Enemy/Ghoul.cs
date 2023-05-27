using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghoul : MonoBehaviour
{

    public Transform Player;
    Animator GhoulAnimator;
    Rigidbody GhoulRigid;
    public float Movespeed;
    private NavMeshAgent nav;
    public bool Screamed = false;

    // Start is called before the first frame update
    void Start()
    {
        GhoulRigid = GetComponent<Rigidbody>();
        GhoulAnimator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Track_Player();
        Ghoul_Anim_Manage();
    }

    void Track_Player()
    {
        float distance = Vector3.Distance(Player.position, transform.position);
        if (distance < 15f)
        {
            StartCoroutine(Nav_Ghoul());
        }
        else
        {
            GhoulRigid.velocity = Vector3.zero;
        }

    }

    IEnumerator Nav_Ghoul()
    {
        transform.LookAt(Player.position);
        if (Screamed == false)
        {
            GhoulAnimator.SetTrigger("Scream");
            Screamed = true;
            yield return new WaitForSeconds(2.0f);
        }
        nav.destination = Player.position;

    }

    void Ghoul_Anim_Manage()
    {
        if (GhoulRigid.velocity.normalized != Vector3.zero)
        {
            // 속력벡터가 0이 아닐 시
            GhoulAnimator.SetTrigger("Track");
        }
        else if (GhoulRigid.velocity == Vector3.zero)
        {
            GhoulAnimator.SetTrigger("Idle");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerAttack")
        {
            Debug.Log("Ghoul Damaged!");

        }
    }
}
