using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeDemo : MonoBehaviour
{

    public float attackDamage;
    public float StrongAttackDamage;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerAttackDamage();
    }


    void GetPlayerAttackDamage()
    {

    }
}
