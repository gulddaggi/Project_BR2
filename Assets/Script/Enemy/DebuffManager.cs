using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 능력 타입별 디버프 구현 클래스.
public class DebuffManager : MonoBehaviour
{
    // 디버프 프리팹 배열. 0 : 물, 1 : 불 ...
    [SerializeField]
    GameObject[] debuffEffects;

    // 적의 초기 속도
    public float originvelocity;

    // 타이머 변수.
    public float time = 0f;
    bool timerOn = false;

    // 타이머 작동 시간
    float targetTime = 0f;

    float drawnDamage = 0f;

    void Start()
    {
        // 초기 속도 초기화
        originvelocity = this.gameObject.GetComponentInParent<NavMeshAgent>().speed;
    }

    private void Update()
    {
        // 디버프 타이머 계산
        if (timerOn)
        {
            time += Time.deltaTime;
            // 적 체력 지속 감소
            this.gameObject.GetComponentInParent<Enemy>().EnemyHP -= drawnDamage;
        }

        if (time > targetTime)
        {
            time = 0f;
            targetTime = 0f;
            drawnDamage = 0f;
            timerOn = false;
        }
    }

    // 물 디버프 적용
    public void WaterDebuffEffectOn(int _index, float _drawnDamage)
    {
        // 적용 디버프 인덱스
        int index = (_index == 3) ? (_index - 1) : 0;

        // 이미 해당 디버프 이펙트가 생성되었는지 확인
        // 모든 자식들에 대해 순회하지만, 최대 6개정도의 자식 수를 예상하여 성능에 문제 없을 것이라 판단
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            // 이미 있을 경우, 이펙트 정지 후 다시 재생
            if (child.name == "SlowDebuff")
            {
                child.GetComponent<ParticleSystem>().Stop();
                child.GetComponent<Debuff>().OnDebuffEnd.Invoke();
                child.GetComponent<ParticleSystem>().Play();
                // 둔화 재적용.
                WaterDebuffSlowOn();
                // 중첩 카운팅
                ++child.GetComponent<Debuff>().count;

                // 디버프 중첩 효과 적용
                if (_drawnDamage != 0f && child.GetComponent<Debuff>().count == 5)
                {
                    child.GetComponent<Debuff>().count = 0;
                    // 익사 적용
                    this.transform.GetChild(i+1).GetComponent<ParticleSystem>().Play();
                    // 익사 데미지 적용
                    WaterDebuffDrawnOn(_drawnDamage);
                }

                return;
            }
            
        }

        // 없을 경우, 새롭게 둔화 이펙트 생성
        GameObject slowDebuff = Instantiate(debuffEffects[index], this.gameObject.transform);
        slowDebuff.transform.parent = this.gameObject.transform;
        slowDebuff.name = "SlowDebuff";
        slowDebuff.GetComponent<ParticleSystem>().Play();
        // 둔화 적용
        WaterDebuffSlowOn();
        Debuff curDebuff = slowDebuff.GetComponent<Debuff>();
        // 중첩 카운팅
        curDebuff.count = 1;
        // 이펙트 종료 시 둔화 해제 함수로 콜백
        curDebuff.OnDebuffEnd.AddListener(WaterDebuffSlowOff);

        // 익사 이펙트 동시에 생성.
        GameObject drawnDebuff = Instantiate(debuffEffects[index + 1], this.gameObject.transform);
        drawnDebuff.name = "DrawnDebuff";
        drawnDebuff.transform.parent = this.gameObject.transform;
    }

    // 둔화 적용
    void WaterDebuffSlowOn()
    {
        // 적의 이동속도를 30% 늦춘다
        this.gameObject.GetComponentInParent<NavMeshAgent>().speed *= 0.7f;
    }

    // 둔화 해제
    void WaterDebuffSlowOff()
    {
        this.gameObject.GetComponentInParent<NavMeshAgent>().speed = originvelocity;
    }

    // 익사 데미지 적용
    void WaterDebuffDrawnOn(float _drawnDamage)
    {
        // Update 호출마다 적용되는 수치로 계산.
        drawnDamage = this.gameObject.GetComponentInParent<Enemy>().EnemyHP * (_drawnDamage * 0.01f) * Time.deltaTime;
        targetTime = 1.5f;
        timerOn = true;
    }
}
