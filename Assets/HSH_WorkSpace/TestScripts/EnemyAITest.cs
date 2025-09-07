/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAITest : MonoBehaviour
{
    public Transform target;
    float attackDelay;

    // Start is called before the first frame update
    void Start()
    {
        //enemy = GetComponent<EnemyTest>();
    }

    // Update is called once per frame
    void Update()
    {
        attackDelay -= Time.deltaTime;
        if (attackDelay < 0)
        {
            attackDelay = 0;
        }

        //타겟과 자신의 거리를 확인
        float distance = Vector3.Distance(transform.position, target.position);

        //공격 딜레이가 0일 때, 시야 범위안에 들어올 때
        if (attackDelay = 0 && distance <= enemy.fieldOfVision)
        {
            //타겟 바라보기
            FaceTarget();

            //공격 범위 안에 들어올 때 공격
            if (distance <= enemy.atkRange)
            {
                AttackTarget();
            }
            //공격하지 않을 때는 추적
            else
            {
                MoveToTarget();
            }
        }
    }

    void FaceTarget()
    {

    }

    void AttackTarget()
    {
        Debug.Log("Attack");
    }

    void MoveToTarget()
    {

    }
}
*/