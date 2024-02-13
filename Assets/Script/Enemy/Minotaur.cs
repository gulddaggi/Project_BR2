using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minotaur : Enemy
{
    protected override void Start()
    {
        base.Start();
        animator.applyRootMotion = false;
    }

    protected override void EnemyAttackOn()
    {
        isAttack = true;
        animator.SetBool("isAttack", true);
        nvAgent.enabled = false;
        Invoke("EnemyAttackRangeON", 0.3f);
        Invoke("EnemyAttackOff", 2f);
    }

    protected override void EnemyAttackRangeON()
    {
        attackRangeObj.SetActive(true);
        nvAgent.enabled = true;
    }

    protected override void EnemyAttackOff()
    {
        nvAgent.enabled = false;
        attackRangeObj.SetActive(false);
        animator.SetBool("isAttack", false);
        Invoke("EnemyAttacRush", AttackDelay);
    }

    protected virtual void EnemyAttacRush()
    {
        nvAgent.enabled = true;
        isAttack = false;
    }
}