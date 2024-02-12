using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minotaur : Enemy
{
    bool cnt = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (player != null)
        {
            animator.SetBool("isAttack", true);
            nvAgent.destination = player.position;
            float dis = Vector3.Distance(player.position, gameObject.transform.position);
            if (dis <= EnemyPlayerAttackDistance && isAttack == false && cnt == false)
            {
                cnt = true;
                EnemyAttackOn();
            }
            else
            {
                //animator.SetBool("isAttack", false);
                cnt = false;
            }
        }
    }

    protected override void EnemyAttackOn()
    {
        isAttack = true;
        animator.SetBool("isAttack", true);
        nvAgent.enabled = false;
        Invoke("EnemyAttackRangeON", 2.3f);
        Invoke("EnemyAttackOff", 3.3f);
    }

    protected override void EnemyAttackOff()
    {
        nvAgent.enabled = false;
        attackRangeObj.SetActive(false);
        isAttack = false;
        animator.SetBool("isAttack", false);
        Invoke("EnemyAttackRangeON", AttackDelay);
    }

    protected override void EnemyAttackRangeOff()
    {
        attackRangeObj.SetActive(true);
        nvAgent.enabled = true;
    }

    protected virtual void EnemyAttacRush()
    {
        nvAgent.enabled = true;
    }

    protected override void UpdateTarget()
    {
        //자신의 위치로부터 10f만큼의 반경의 충돌체를 검사하고 
        Collider[] cols = Physics.OverlapSphere(transform.position, range);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                //반경 내에 플레이어가 존재할 경우 추적
                if (cols[i].tag == "Player")
                {
                    //Debug.Log("Enemy find Target");
                    player = cols[i].gameObject.transform;
                }
            }
        }
        else
        {
            //Debug.Log("Enemy lost Target");
            animator.SetBool("isAttack", false);
            player = null;
        }
    }
}