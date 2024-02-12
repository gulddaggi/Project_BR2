using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurRangeAITest : RangeAITest
{
    // 플레이어 태그와 접촉한 후 대기 시간
    public float waitTime = 1f;
    // 플레이어와 접촉했는지 여부를 나타내는 플래그
    private bool playerDetected = false;
    //돌진 속도
    public float rush = 20f;

    [SerializeField] float EnemyPlayerAttackDistance = 3;

    void Start()
    {
        nvAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        InvokeRepeating("UpdateTarget", 0f, 1f);
    }

    void Update()
    {
        if (player != null)
        {
            animator.SetBool("isWalk", true);
            nvAgent.destination = player.position;
            float dis = Vector3.Distance(player.position, gameObject.transform.position);
            if (dis <= EnemyPlayerAttackDistance)
            {
                animator.SetBool("isAttack", true);
            }
            else
            {
                animator.SetBool("isAttack", false);
            }
        }
    }

    // 플레이어와 접촉 시 호출됩니다.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어와 접촉 시 속도를 0으로 설정합니다.
            nvAgent.speed = 0f;
            playerDetected = true;
            // 일정 시간 후에 다시 속도를 설정합니다.
            StartCoroutine(ResumeSpeedAfterDelay());
        }
    }

    // 대기 시간이 지난 후에 다시 속도를 설정합니다.
    IEnumerator ResumeSpeedAfterDelay()
    {
        yield return new WaitForSeconds(waitTime);
        if (!playerDetected)
        {
            // 플레이어를 더 이상 감지하지 않으면 속도를 다시 설정합니다.
            nvAgent.speed = rush;
        }
    }

    // 플레이어와 접촉이 끝날 때 호출됩니다.
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
}
