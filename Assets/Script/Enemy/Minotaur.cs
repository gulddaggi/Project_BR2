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
        base.Update();
    }

    protected override void EnemyAttackOn()
    {
        isAttack = true;
        animator.SetBool("isAttack", true);
        nvAgent.enabled = false;
        Invoke("EnemyAttackRangeON", 0.3f);
        Invoke("EnemyAttackOff", 1.3f);
    }

    protected override void EnemyAttackRangeON()
    {
        attackRangeObj.SetActive(true);
        nvAgent.enabled = true;
        delay = 1f;
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