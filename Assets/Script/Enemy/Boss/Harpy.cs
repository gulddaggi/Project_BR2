using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Harpy : Enemy
{
    public Transform AttackPO1;
    public Transform AttackPO2;

    public GameObject Harpy_point;
    public GameObject Harpy_Big_Projecter;

    float maxHP;

    Vector3 LookVec;
    Vector3 TauntVec;
    [SerializeField] bool isLook;
    [SerializeField] bool isOverdriving = false; // 보스 폭주 패턴 돌입 체크

    public float Boss_Pattern_Waiting_Time = 3.0f;
    public UnityEvent<float, float> OnBossHPUpdated;

    #region * 하피 투사체 오브젝트 풀링

    public BossBullet BossBullet1_Harpy;
    private List<BossBullet> BossBulletPool = new List<BossBullet>();
    private readonly int BossBulletMaxCount = 20;
    private int currentBulletIndex = 0;

    protected override void Start()
    {
        base.Start();
        EnemyHP = 300;
        maxHP = EnemyHP;
        OnBossHPUpdated.Invoke(maxHP, EnemyHP);
        Player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < BossBulletMaxCount; ++i)
        {
            BossBullet bul = Instantiate<BossBullet>(BossBullet1_Harpy);
            bul.gameObject.SetActive(false); 

            BossBulletPool.Add(bul);
            // 투사체 풀 생성 후 setActive(false)
        }
        StartCoroutine("Harpy_Pattern_Management");
        PrimitiveHP = EnemyHP;
        isBoss = true;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if(isLook) // 시선이 플레이어를 향하도록 함
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            LookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(Player.transform.position + LookVec);
        }
        Boss_Overdrive_Check(); // 폭주 패턴 체크
    }

    IEnumerator Harpy_Pattern_Management() // 하피 패턴관리
    {
        yield return new WaitForSeconds(0.1f);

        int RAD_ACT = Random.Range(0, 2); // 패턴 처리 위해서 난수 부여
        Debug.Log(RAD_ACT);

        // 하드모드 패턴 추가시 Range를 늘리는 방향으로 구현 가능. 아니면 break문을 없애서 조건을 늘리던가..


        switch (RAD_ACT)
        {
            case 0:
                StartCoroutine(Harpy_Fire_1());
                break;

            case 1:
                StartCoroutine(Harpy_Fire_2());
                break;

        }
    }

    IEnumerator Harpy_Fire_1() // 무작위 방향으로 투사체 난사
    {

        for (int j = 0; j < BossBulletMaxCount; j++)
        {
            if (!BossBulletPool[currentBulletIndex].gameObject.activeSelf)
            {

                int AttackPosition_Random = Random.Range(1, 2);
                
                if (AttackPosition_Random == 1) BossBulletPool[currentBulletIndex].transform.position = AttackPO1.position;
                if (AttackPosition_Random == 2) BossBulletPool[currentBulletIndex].transform.position = AttackPO2.position;

                // 공격 위치 난수설정

                // 총알 활성화
                BossBulletPool[currentBulletIndex].gameObject.SetActive(true);
                // Debug.Log("Pattern 1 Activated!");

                // 방금 9번째 총알을 발사했다면 다시 0번째 총알을 발사할 준비를 한다.
                if (currentBulletIndex >= BossBulletMaxCount - 1)
                {
                    currentBulletIndex = 0;
                }
                else
                {
                    currentBulletIndex++;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        if (isOverdriving) { yield return new WaitForSeconds(Boss_Pattern_Waiting_Time); } // 일반 패턴
        else{ yield return new WaitForSeconds((2 * Boss_Pattern_Waiting_Time) / 3); } // 폭주 패턴
        StartCoroutine(Harpy_Pattern_Management());
    }

    IEnumerator Harpy_Fire_2() // 중앙에서 일정 거리동안 순간이동하고 큰 투사체 발사
    {
        Debug.Log("Pattern 2 Activated!");
        int ran = Random.Range(0, 360); //랜덤으로 0~360도
        float x = Mathf.Cos(ran * Mathf.Deg2Rad) * 8f; // 정해진 위치에서 5만큼 떨어진 원형 랜덤 방향으로 생성
        float z = Mathf.Sin(ran * Mathf.Deg2Rad) * 8f; // 정해진 위치에서 5만큼 떨어진 원형 랜덤 방향으로 생성

        gameObject.transform.position = new Vector3(0f, 0f, 0f) + new Vector3(x, 0f, z);

        yield return new WaitForSeconds(0.5f);
        GameObject Big_Projecter = Instantiate(Harpy_Big_Projecter, gameObject.transform.position, gameObject.transform.rotation);

        if (isOverdriving) { yield return new WaitForSeconds(Boss_Pattern_Waiting_Time); } // 일반 패턴
        else
        {
            GameObject Big_Projecter_2 = Instantiate(Harpy_Big_Projecter, gameObject.transform.position, gameObject.transform.rotation);
            yield return new WaitForSeconds((2 * Boss_Pattern_Waiting_Time) / 3);
        } // 폭주 패턴
        StartCoroutine(Harpy_Pattern_Management());
    }

    void Boss_Overdrive_Check()
    {
        if (EnemyHP <= PrimitiveHP / 2)
        {
            isOverdriving = true;
            // Debug.Log("하피 폭주 패턴 개시");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 디버프 배열
        int[] debuffArray;

        // 공격 종류에 따른 피격 관련 기능 수행
        if (other.tag == "PlayerAttack")
        {
            Debug.Log("Damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerStackDamage != 0f) stackDamage = playerdata.PlayerStackDamage;
            float damage = playerdata.PlayerAttackDamage;
            debuffArray = playerdata.GetAttackDebuff();

            // 요 두 함수가 특수 공격 게이지 판정
            GameManager_JS.Instance.GuageUpdate(playerdata.PlayerSpecialAttackFillingAmount);
            GameManager_JS.Instance.Guage();

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;
            OnBossHPUpdated.Invoke(maxHP, EnemyHP);

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, stackDamage);
        }
        else if (other.tag == "StrongPlayerAttack")
        {
            Debug.Log("Strongly Damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerStackDamage != 0f) stackDamage = playerdata.PlayerStackDamage;
            float damage = playerdata.PlayerStrongAttackDamage;
            debuffArray = playerdata.GetStAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;
            OnBossHPUpdated.Invoke(maxHP, EnemyHP);

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, stackDamage);
        }
        else if (other.tag == "PlayerDodgeAttack")
        {
            Debug.Log("Dodge damaged!");

            // 플레이어로부터 데미지, 디버프 배열 반환
            Player playerdata = other.transform.GetComponentInParent<Player>();
            // 익사 디버프 여부 확인
            if (playerdata.PlayerStackDamage != 0f) stackDamage = playerdata.PlayerStackDamage;
            float damage = playerdata.PlayerDodgeAttackDamage;
            debuffArray = playerdata.GetDodgeAttackDebuff();

            GameManager_JS.Instance.Guage();

            // 피격 시 넉백
            //StartCoroutine(GetDamaged());

            // 피격 시 체력 감소 계산
            EnemyHP -= damage;
            OnBossHPUpdated.Invoke(maxHP, EnemyHP);

            // 디버프 적용
            debuffChecker.DebuffCheckJS(debuffArray, stackDamage);
        }

        if (EnemyHP <= 0)
        {
            gameObject.SetActive(false);
        }

        IEnumerator GetDamaged()
        {
            SR.material.color = Color.red;
            yield return new WaitForSeconds(0.6f);
            SR.material.color = Color.white;
        }
    }

}
