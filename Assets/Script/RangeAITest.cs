using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeAITest : MonoBehaviour
{
    //플레이어 발견시 이펙트
    public GameObject FindPlayer;
    public Vector3 FindPlayerOffset = new Vector3(0, 2.2f, 0);

    private Canvas uiCanvas;
    public Image hpBarImage;

    private Transform player;
    public UnityEngine.AI.NavMeshAgent nvAgent;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        nvAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        //0.25초마다 타깃 체크
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            animator.SetBool("isWalk", true);
            nvAgent.destination = player.position;
            float dis = Vector3.Distance(player.position, gameObject.transform.position);
            if (dis <= 2)
            {
                animator.SetBool("isAttack", true);
            }
            else
            {
                animator.SetBool("isAttack", false);
            }
        }
    }

    void UpdateTarget()
    {
        //자신의 위치로부터 10f만큼의 반경의 충돌체를 검사하고 
        Collider[] cols = Physics.OverlapSphere(transform.position, 20f);

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 20f);
    }
}
