using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Debuff : MonoBehaviour
{
    // 디버프 이펙트 종료 시 발생하는 이벤트
    public UnityEvent OnDebuffEnd;

    // 중첩 카운팅 변수. 둔화, 빙결 모두 중첩 존재.
    public int count = 0;

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<ParticleSystem>().isStopped)
        {
            OnDebuffEnd.Invoke();
            count = 0;
        }

        if (count > 5)
        {
            count = 5;
        }
    }
}
