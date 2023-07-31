using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        float[] tmpArray = new float[2] { 0f, 0f };

        if (other.tag == "PlayerAttack")
        {
            Debug.Log("Damaged!");
            var playerdata = other.transform.GetComponentInParent<Player>();
            tmpArray = playerdata.PlayerAttack(EnemyHP);
            EnemyHP = tmpArray[0];
            debuffChecker.DebuffCheck((int)tmpArray[1]);
        }
        else if (other.tag == "StrongPlayerAttack")
        {
            Debug.Log("Strongly Damaged!");
            var playerdata = other.transform.GetComponentInParent<Player>();
            EnemyHP = (playerdata.PlayerStrongAttack(EnemyHP));
            tmpArray = playerdata.PlayerAttack(EnemyHP);
            EnemyHP = tmpArray[0];
            debuffChecker.DebuffCheck((int)tmpArray[1]);
        }

        if (EnemyHP <= 0)
        {
            enemySpawner.EnemyDead();
            gameObject.SetActive(false);

        }
    }
}
