using UnityEngine;

public class Minotaur : Enemy
{
    [SerializeField] private float chargeSpeed = 10f; // 달려가는 속도
    [SerializeField] private float chargeDamage = 20f; // 충돌 공격 대미지

    private bool isCharging = false; // 달리기 상태 여부
    private Vector3 chargeDirection; // 플레이어를 향한 방향

    protected override void Start()
    {
        base.Start();
        nav.speed = Movespeed; // 기본 이동 속도 설정
    }

    protected override void Update()
    {
        base.Update();

        if (!isCharging)
        {
            // 플레이어를 인식 범위 안에서 발견하면 충전 모드로 전환
            float distance = Vector3.Distance(Player.transform.position, transform.position);
            if (distance < Enemy_Recognition_Range)
            {
                isCharging = true;
                chargeDirection = (Player.transform.position - transform.position).normalized;
                nav.speed = chargeSpeed;
                nav.SetDestination(transform.position); // 현재 위치에서 출발
            }
        }
        else
        {
            // 충돌 중인지 체크
            if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
            {
                // 플레이어와 충돌했으면 대미지를 주고 충돌 모드 종료
                if (Vector3.Distance(Player.transform.position, transform.position) <= nav.radius)
                {
                    Player.GetComponent<Player>().TakeDamage(chargeDamage);
                    isCharging = false;
                    nav.speed = Movespeed;
                }
                else
                {
                    // 아직 충돌하지 않았으면 플레이어를 향해 이동 계속
                    nav.SetDestination(Player.transform.position);
                }
            }
        }
    }

    protected override void EnemyAttackOn()
    {
        // 상속받은 메소드를 오버라이드하여 Minotaur의 공격 동작 구현 가능
        // 여기서는 Minotaur의 특수한 공격 동작을 추가로 구현할 수 있음
    }
}