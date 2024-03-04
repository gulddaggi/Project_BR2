using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적에게 적용될 디버프 확인 클래스.
public class DebuffChecker : MonoBehaviour
{
    // 디버프 생성 클래스 변수
    [SerializeField]
    DebuffManager debuffManager;

    // 디버프 확인. 디버프 배열을 순회하며 인덱스에 맞는 이펙트 활성화
    public void DebuffCheckJS(int[] _debuffArray)
    {
        for (int i = 0; i < _debuffArray.Length; i++)
        {
            if (_debuffArray[i] != 0)
            {
                switch (i)
                {
                    case 0:
                        debuffManager.WaterDebuffEffectOn(_debuffArray[i]);
                        break;

                    case 1:
                        debuffManager.FlameDebuffEffectOn(_debuffArray[i]);
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }

    // 디버프 확인. 디버프 배열을 순회하며 인덱스에 맞는 이펙트 활성화
    public void DebuffCheckJS(int[] _debuffArray, float[] _excutionArray)
    {
        for (int i = 0; i < _debuffArray.Length; i++)
        {
            if (_debuffArray[i] != 0.0f)
            {
                float enemyHP = this.GetComponent<Enemy>().EnemyHP;
                float fullHP = this.GetComponent<Enemy>().FullHP;
                switch (i)
                {
                    case 0:
                        if (_excutionArray[i] > 0.0f && (fullHP * _excutionArray[i]) >= enemyHP)
                        {
                            // 처형 이펙트 재생 및 디버프 전염
                            debuffManager.WaterExcutionEffectOn();
                            // 사망 처리
                            this.GetComponent<Enemy>().Dead();
                        }
                        else
                        {
                            debuffManager.WaterDebuffEffectOn(_debuffArray[i]);
                        }
                        break;

                    case 1:
                        if (_excutionArray[i] > 0.0f && (fullHP * _excutionArray[i]) >= enemyHP)
                        {
                            // 처형 이펙트 재생 및 디버프 전염
                            debuffManager.FlameExcutionEffectOn();
                            // 사망 처리
                            this.GetComponent<Enemy>().Dead();
                        }
                        else
                        {
                            debuffManager.FlameDebuffEffectOn(_debuffArray[i]);
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
