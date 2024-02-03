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
    public void DebuffCheckJS(int[] _debuffArray, float _stackDamage)
    {
        for (int i = 0; i < _debuffArray.Length; i++)
        {
            if (_debuffArray[i] != 0)
            {
                switch (i)
                {
                    case 0:
                        debuffManager.WaterDebuffEffectOn(_debuffArray[i], _stackDamage);
                        break;

                    case 1:
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }

    // 디버프 확인. 디버프 배열을 순회하며 인덱스에 맞는 이펙트 활성화
    public void DebuffCheckJS(int[] _debuffArray, bool[] _excutionArray, float _stackDamage)
    {
        for (int i = 0; i < _debuffArray.Length; i++)
        {
            if (_debuffArray[i] != 0)
            {
                switch (i)
                {
                    case 0:
                        if (_excutionArray[i])
                        {
                            // 처형 이펙트 재생 및 디버프 전염
                            
                            // 사망 처리
                        }
                        else
                        {
                            debuffManager.WaterDebuffEffectOn(_debuffArray[i], _stackDamage);
                        }
                        break;

                    case 1:
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
