using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowingBossBullet : BossBullet
{

    GameObject Player;
    private NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        nav.destination = Player.transform.position;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerdata = other.transform.GetComponentInParent<Player>();
            playerdata.CurrentHP -= BulletDamage;
            gameObject.SetActive(false);
        }
    }
}
