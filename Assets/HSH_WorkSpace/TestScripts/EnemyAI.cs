using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    private Transform monster;
    private Transform player;
    private UnityEngine.AI.NavMeshAgent nvAgent;

    // Start is called before the first frame update
    void Start()
    {
        monster = gameObject.GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        nvAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nvAgent.destination = player.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
