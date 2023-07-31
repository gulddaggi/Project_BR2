using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAI : MonoBehaviour
{
    Transform target;
    float enemyMoveSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //0.25초마다 타깃 체크
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * enemyMoveSpeed * Time.deltaTime);
        }
    }

    void UpdateTarget()
    {
        //자신의 위치로부터 10f만큼의 반경의 충돌체를 검사하고 
        Collider[] cols = Physics.OverlapSphere(transform.position, 10f);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                //반경 내에 플레이어가 존재할 경우 추적
                if (cols[i].tag == "Player")
                {
                    Debug.Log("Enemy find Target");
                    target = cols[i].gameObject.transform;
                }
            }
        }
        else
        {
            Debug.Log("Enemy lost Target");
            target = null;
        }
    }
}
