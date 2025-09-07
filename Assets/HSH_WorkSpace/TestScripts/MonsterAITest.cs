using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAITest : MonoBehaviour
{
    private Transform monster;
    private Transform player;
    private NavMeshAgent nvAgent;

    // Start is called before the first frame update
    void Start()
    {
        monster = gameObject.GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        nvAgent = gameObject.GetComponent<NavMeshAgent>();
        nvAgent.destination = player.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
