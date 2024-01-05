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

    void Start()
    {
        // 초기 속도 초기화
        originvelocity = this.gameObject.GetComponentInParent<NavMeshAgent>().speed;
    }

    // 물 디버프 적용
    public void WaterDebuffEffectOn(int _index)
    {
        // 적용 디버프 인덱스
        int index = _index - 1;

        // 이미 해당 디버프 이펙트가 생성되었는지 확인
        // 모든 자식들에 대해 순회하지만, 최대 6개정도의 자식 수를 예상하여 성능에 문제 없을 것이라 판단
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            // 이미 있을 경우, 이펙트 정지 후 다시 재생
            if (child.name == "WaterDebuff")
            {
                Debug.Log("이미 있음");
                child.GetComponent<ParticleSystem>().Stop();
                child.GetComponent<Debuff>().OnDebuffEnd.Invoke();
                child.GetComponent<ParticleSystem>().Play();
                // 둔화 재적용.
                WaterDebuffSlowOn();
                return;
            }
            
        }

        // 없을 경우, 새롭게 이펙트 생성
        GameObject waterDebuff = Instantiate(debuffEffects[index], this.gameObject.transform);
        waterDebuff.name = "WaterDebuff";
        waterDebuff.GetComponent<ParticleSystem>().Play();
        // 둔화 적용
        WaterDebuffSlowOn();
        // 이펙트 종료 시 둔화 해제 함수로 콜백
        waterDebuff.GetComponent<Debuff>().OnDebuffEnd.AddListener(WaterDebuffSlowOff);
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
}
