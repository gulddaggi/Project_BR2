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

    [SerializeField]
    private LayerMask layerMask;

    void Start()
    {
        // 초기 속도 초기화
        originvelocity = this.gameObject.GetComponentInParent<NavMeshAgent>().speed;
    }

    private void Update()
    {

    }

    // 물 디버프 적용
    public void WaterDebuffEffectOn(int _index)
    {
        // 적용 디버프 인덱스
        int index = (_index == 3) ? (_index - 1) : 0;

        // 적용 디버프 이름
        string effectName;

        // 인덱스에 따른 생성 및 재생 대상 디버프 이름 지정.
        if (index == 0)
        {
            effectName = "SlowDebuff";
        }
        else
        {
            effectName = "FreezeDebuff";
        }

        // 이미 해당 디버프 이펙트가 생성되었는지 확인
        // 모든 자식들에 대해 순회하지만, 최대 6개정도의 자식 수를 예상하여 성능에 문제 없을 것이라 판단
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            // 이미 있을 경우, 이펙트 정지 후 다시 재생
            if (child.name == effectName)
            {
                child.GetComponent<ParticleSystem>().Stop();
                child.GetComponent<Debuff>().OnDebuffEnd.Invoke();
                child.GetComponent<ParticleSystem>().Play();
                // 둔화 재적용. 둔화는 빙결 능력에도 포함되어있음.
                WaterDebuffSlowOn();
                // 중첩 카운팅
                ++child.GetComponent<Debuff>().count;

                // 디버프 중첩 효과 적용
                if (this.gameObject.GetComponentInParent<Enemy>().totalStackDamage != 0f 
                    && child.GetComponent<Debuff>().count == 5)
                {
                    child.GetComponent<Debuff>().count = 0;
                    // 중첩 이펙트 적용
                    this.transform.GetChild(i + 1).GetComponent<ParticleSystem>().Play();
                    // 중첩 데미지 적용
                    WaterDebuffStackOn(1.5f);
                }

                return;
            }
        }

        // 없을 경우, 새롭게 이펙트 생성
        if (index == 0)
        {
            // 둔화, 익사 이펙트 생성
            GameObject slowDebuff = Instantiate(debuffEffects[index], this.gameObject.transform);
            slowDebuff.transform.parent = this.gameObject.transform;
            slowDebuff.name = effectName;
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
        else
        {
            // 빙결, 빙결 스택 이펙트 생성
            // 둔화, 익사 이펙트 생성
            GameObject freezeDebuff = Instantiate(debuffEffects[index], this.gameObject.transform);
            freezeDebuff.transform.parent = this.gameObject.transform;
            freezeDebuff.name = effectName;
            freezeDebuff.GetComponent<ParticleSystem>().Play();
            // 둔화 적용
            WaterDebuffSlowOn();
            Debuff curDebuff = freezeDebuff.GetComponent<Debuff>();
            // 중첩 카운팅
            curDebuff.count = 1;
            // 이펙트 종료 시 둔화 해제 함수로 콜백
            curDebuff.OnDebuffEnd.AddListener(WaterDebuffSlowOff);

            // 익사 이펙트 동시에 생성.
            GameObject freezeStackDebuff = Instantiate(debuffEffects[index + 1], this.gameObject.transform);
            freezeStackDebuff.name = "FreezeStackDebuff";
            freezeStackDebuff.transform.parent = this.gameObject.transform;
        }
    }

    public void WaterExcutionEffectOn()
    {
        // 주변 적에게 디버프 적용
        Collider[] cols= Physics.OverlapSphere(this.gameObject.transform.position, 3f, layerMask);
        for (int i = 0; i < cols.Length; i++)
        {
            Debug.Log(cols[i].gameObject.name + "의 스택 증가");
            cols[i].GetComponentInChildren<DebuffManager>().WaterDebuffEffectOn(3);
        }
        GameObject excutionEffect = Instantiate(debuffEffects[4], this.gameObject.transform);
        excutionEffect.transform.SetParent(null);

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

    // 중첩 데미지 적용
    void WaterDebuffStackOn(float _targetTime)
    {
        this.gameObject.GetComponentInParent<Enemy>().SetStackDamageOn(_targetTime);
    }
}
