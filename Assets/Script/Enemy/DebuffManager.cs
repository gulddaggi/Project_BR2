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

    // 능력 타입 별 디버프 이펙트를 자식으로 갖는 오브젝트 배열.
    [SerializeField]
    GameObject[] debuffObjs;

    // 적의 초기 속도
    public float originvelocity;

    [SerializeField]
    private LayerMask layerMask;

    bool isIgnitionStackOn = false;

    bool isSlowOn = false;
    bool isWaterStackOn = false;

    void Start()
    {
        // 초기 속도 초기화
        originvelocity = this.gameObject.GetComponentInParent<NavMeshAgent>().speed;
    }

    // 물 디버프 적용
    public void WaterDebuffEffectOn(int _index)
    {
        // 적용 디버프 인덱스
        int index = (_index == 3) ? (_index - 1) : 0;

        // 적용 타입 이펙트의 부모 오브젝트
        GameObject waterParent = debuffObjs[0];

        // 인덱스에 따른 디버프 재생.
        if (index == 0)
        {
            Debug.Log("공격 인식");
            Transform slowEffectTransform = waterParent.transform.GetChild(0);
            // 둔화 적용
            if (!isSlowOn)
            {
                WaterDebuffSlowOn();
                // 이펙트 종료 시 둔화 해제 함수로 콜백
                slowEffectTransform.GetComponent<Debuff>().OnDebuffEnd.AddListener(WaterDebuffSlowOff);
            }

            slowEffectTransform.GetComponent<ParticleSystem>().Stop();
            slowEffectTransform.GetComponent<ParticleSystem>().Play();
            ++slowEffectTransform.GetComponent<Debuff>().count;

            // 디버프 중첩 효과 적용
            if (slowEffectTransform.GetComponent<Debuff>().count == 5 && _index == 2 && !isWaterStackOn)
            {
                isWaterStackOn = true;
                // 이펙트 재생
                slowEffectTransform.GetComponent<Debuff>().count = 0;
                Transform drawnEffectTransform = waterParent.transform.GetChild(1);
                drawnEffectTransform.GetComponent<ParticleSystem>().Stop();
                drawnEffectTransform.GetComponent<ParticleSystem>().Play();

                // 데미지 적용
                WaterDebuffStackOn(1.5f);
            }
        }
        else
        {
            Transform freezeEffectTransform = waterParent.transform.GetChild(2);
            // 둔화 적용
            if (!isSlowOn)
            {
                WaterDebuffSlowOn();
                // 이펙트 종료 시 둔화 해제 함수로 콜백
                freezeEffectTransform.GetComponent<Debuff>().OnDebuffEnd.AddListener(WaterDebuffSlowOff);
            }

            freezeEffectTransform.GetComponent<ParticleSystem>().Stop();
            freezeEffectTransform.GetComponent<ParticleSystem>().Play();
            ++freezeEffectTransform.GetComponent<Debuff>().count;
            
            // 디버프 중첩 효과 적용
            if (freezeEffectTransform.GetComponent<Debuff>().count == 5 && _index == 3 && !isWaterStackOn)
            {
                isWaterStackOn = true;
                // 이펙트 재생
                freezeEffectTransform.GetComponent<Debuff>().count = 0;
                Transform freezeStackEffectTransform = waterParent.transform.GetChild(3);
                freezeStackEffectTransform.GetComponent<ParticleSystem>().Stop();
                freezeStackEffectTransform.GetComponent<ParticleSystem>().Play();

                // 데미지 적용
                StackDamageOn(0, 1.5f);
            }
        }
    }

    // 불 디버프 적용
    public void FlameDebuffEffectOn(int _index)
    {
        // 적용 디버프 인덱스
        int index = (_index == 3) ? (_index - 1) : 0;

        // 적용 타입 이펙트의 부모 오브젝트
        GameObject flameParent = debuffObjs[1];

        // 인덱스에 따른 디버프 재생.
        if (index == 0)
        {
            Transform burnEffectTransform = flameParent.transform.GetChild(index);
            burnEffectTransform.GetComponent<ParticleSystem>().Stop();
            burnEffectTransform.GetComponent<ParticleSystem>().Play();
            ++burnEffectTransform.GetComponent<Debuff>().count;

            // 디버프 중첩 효과 적용
            if (burnEffectTransform.GetComponent<Debuff>().count >= 5 && _index == 2)
            {
                // 이펙트 재생
                burnEffectTransform.GetComponent<Debuff>().count = 0;
                Transform burstEffectTransform = flameParent.transform.GetChild(index + 1);
                burstEffectTransform.GetComponent<ParticleSystem>().Stop();
                burstEffectTransform.GetComponent<ParticleSystem>().Play();

                // 데미지 적용
                StackDamageOn(1, 3f);
            }
        }
        else
        {
            Transform IgnitionEffectTransform = flameParent.transform.GetChild(index);
            IgnitionEffectTransform.GetComponent<ParticleSystem>().Stop();
            IgnitionEffectTransform.GetComponent<ParticleSystem>().Play();

            if (!isIgnitionStackOn)
            {
                isIgnitionStackOn = true;
                Invoke("FlameIgnitionStackEffectOn", 3f);
            }
        }
    }

    // 불 타입 능력 처형 이펙트
    public void FlameExcutionEffectOn()
    {
        // 주변 적에게 디버프 적용
        Collider[] cols = Physics.OverlapSphere(this.gameObject.transform.position, 3f, layerMask);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == "Enemy")
            {
                cols[i].GetComponentInChildren<DebuffManager>().FlameDebuffEffectOn(3);
            }
        }

        // 처형 이펙트 재생
        Transform excutionEffectTransform = debuffObjs[1].transform.GetChild(4);
        excutionEffectTransform.SetParent(null);
        excutionEffectTransform.GetComponent<ParticleSystem>().Play();

    }

    // 점화 이펙트 재생 3초 후 재생되는 이펙트
    void FlameIgnitionStackEffectOn()
    {
        GameObject flameParent = debuffObjs[1];
        Transform ignitionStackEffectTransform = flameParent.transform.GetChild(3);
        ignitionStackEffectTransform.GetComponent<ParticleSystem>().Stop();
        ignitionStackEffectTransform.GetComponent<ParticleSystem>().Play();

        // 데미지 적용
        StackDamageOn(1, 1f);
        isIgnitionStackOn = false;
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
        Transform excutionEffectTransform = debuffObjs[0].transform.GetChild(4);
        excutionEffectTransform.transform.SetParent(null);
        excutionEffectTransform.GetComponent<ParticleSystem>().Play();

    }


    // 둔화 적용
    void WaterDebuffSlowOn()
    {
        isSlowOn = true;
        // 적의 이동속도를 30% 늦춘다
        this.gameObject.GetComponentInParent<NavMeshAgent>().speed *= 0.7f;
    }

    // 둔화 해제
    void WaterDebuffSlowOff()
    {
        isSlowOn = false;
        this.gameObject.GetComponentInParent<NavMeshAgent>().speed = originvelocity;
    }

    // 중첩 데미지 적용
    void WaterDebuffStackOn(float _targetTime)
    {
        Debug.Log("1");
        this.gameObject.GetComponentInParent<Enemy>().SetStackDamageOn(_targetTime);
        isWaterStackOn = false;
    }

    void StackDamageOn(int _index, float _targetTime)
    {
        this.gameObject.GetComponentInParent<Enemy>().SetStackDamageOn(_index, _targetTime);
        isWaterStackOn = false;
    }
}
