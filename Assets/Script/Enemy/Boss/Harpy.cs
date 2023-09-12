using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpy : Enemy
{
    public Transform AttackPO1;
    public Transform AttackPO2;

    public GameObject Harpy_point;
    public GameObject Harpy_Big_Projecter;

    Vector3 LookVec;
    Vector3 TauntVec;
    [SerializeField] bool isLook;

    #region * 하피 투사체 오브젝트 풀링

    public BossBullet BossBullet1_Harpy;
    private List<BossBullet> BossBulletPool = new List<BossBullet>();
    [SerializeField] private int BossBullet_Count = 7;
    private readonly int BossBulletMaxCount = 10;
    private int currentBulletIndex;

    public override void Start()
    {
        for (int i = 0; i < BossBulletMaxCount; ++i)
        {
            BossBullet bul = Instantiate<BossBullet>(BossBullet1_Harpy);
            bul.gameObject.SetActive(false); 

            BossBulletPool.Add(bul);
            // 투사체 풀 생성 후 setActive(false)
        }
        StartCoroutine("Harpy_Pattern_Management");
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
    }

    IEnumerator Harpy_Pattern_Management() // 하피 패턴관리
    {
        yield return new WaitForSeconds(0.1f);

        int RAD_ACT = Random.Range(0, 5); // 패턴 처리 위해서 난수 부여
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

            case 2:
                StartCoroutine(Harpy_Fire_1());
                break;

            case 3:
                StartCoroutine(Harpy_Fire_1());
                break;

            case 4:
                StartCoroutine(Harpy_Fire_1());
                break;
        }
    }

    IEnumerator Harpy_Fire_1() // 무작위 방향으로 투사체 난사
    {

        for (int j = 0; j < BossBullet_Count; j++)
        {
            if (!BossBulletPool[currentBulletIndex].gameObject.activeSelf)
            {

                int AttackPosition_Random = Random.Range(1, 3);

                if (AttackPosition_Random == 1) {
                    BossBulletPool[currentBulletIndex].transform.position = AttackPO1.position;
                }
                else if (AttackPosition_Random == 2)
                {
                    BossBulletPool[currentBulletIndex].transform.position = AttackPO2.position;
                } // 공격 위치 난수설정

                // 총알 활성화
                BossBulletPool[currentBulletIndex].gameObject.SetActive(true);

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
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3.0f);
        Harpy_Pattern_Management();
    }

    IEnumerator Harpy_Fire_2() // 중앙에서 일정 거리동안 순간이동하고 큰 투사체 발사
    {
        int ran = Random.Range(0, 360); //랜덤으로 0~360도
        float x = Mathf.Cos(ran * Mathf.Deg2Rad) * 5f; // 정해진 위치에서 5만큼 떨어진 원형 랜덤 방향으로 생성
        float z = Mathf.Sin(ran * Mathf.Deg2Rad) * 5f; // 정해진 위치에서 5만큼 떨어진 원형 랜덤 방향으로 생성

        Vector3 pos = transform.position + new Vector3(x, 2.58f, z); 

        gameObject.transform.position = Harpy_point.transform.position + new Vector3(x, 2.58f, z);

        yield return new WaitForSeconds(0.5f);
        GameObject Big_Projecter = Instantiate(Harpy_Big_Projecter, gameObject.transform.position, gameObject.transform.rotation);

        yield return new WaitForSeconds(3.0f);
        Harpy_Pattern_Management();
    }
}
