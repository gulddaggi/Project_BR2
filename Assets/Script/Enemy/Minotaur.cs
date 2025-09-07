using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minotaur : Enemy
{
    float delay = 0.25f;

    protected override void Start()
    {
        base.Start();
        animator.applyRootMotion = false;

        InvokeRepeating("UpdateTarget", delay, 0.25f);
    }

    protected override void Update()
    {
        if (player != null)
        {
            animator.SetBool("isWalk", true);
            nvAgent.destination = player.position;
            float dis = Vector3.Distance(player.position, gameObject.transform.position);
            if (dis <= EnemyPlayerAttackDistance && isAttack == false)
            {
                EnemyAttackOn();
                animator.SetBool("isAttack", true);
            }
            else
            {
                animator.SetBool("isAttack", false);
            }
        }
    }

    protected override void EnemyAttackOn()
    {
        nvAgent.enabled = false;
        isAttack = true;
        attackRangeObj.SetActive(true);
        nvAgent.enabled = true;
        Invoke("EnemyAttackOff", 1f);
    }

    protected override void EnemyAttackOff()
    {
        nvAgent.enabled = false;
        attackRangeObj.SetActive(false);
        animator.SetBool("isAttack", false);
        Invoke("EnemyAttacRush", AttackDelay);
        delay = 0f;
    }

    protected virtual void EnemyAttacRush()
    {
        nvAgent.enabled = true;
        isAttack = false;
    }
}