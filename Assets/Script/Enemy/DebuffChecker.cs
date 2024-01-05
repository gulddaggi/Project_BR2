using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffChecker : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer objColor;

    // 디버프 프리팹 배열. 0 : 물, 1 : 불 ...
    [SerializeField]
    GameObject[] debuffEffects;

    float time = 0f;
    bool timerOn = false;
    Color originColor;
    int curDebuffIndex = 0;
    float originvelocity = 3.5f;

    void Start()
    {
        originColor = objColor.material.color;
    }

    void Update()
    {
        // 디버프 타이머 계산
        if (timerOn)
        {
            time += Time.deltaTime;
        }

        if (time > 5f)
        {
            DebuffOff();
        }
    }

    // 디버프 확인. 디버프 배열을 순회하며 인덱스에 맞는 이펙트 활성화
    public void DebuffCheckJS(int[] _debuffArray)
    {
        for (int i = 0; i < _debuffArray.Length; i++)
        {
            Debug.Log("_debuffArray[0]" + _debuffArray[0]);
            if (_debuffArray[i] != 0)
            {
                switch (i)
                {
                    case 0:
                        WaterDebuffOn(_debuffArray[i]);
                        break;

                    case 1:
                        break;
                    
                    default:
                        break;
                }
            }
        }
    }

    // 물 디버프 적용.
    public void WaterDebuffOn(int _value)
    {
        int index = _value - 1;
        // 인덱스 : 0~2
        GameObject waterDebuff = Instantiate(debuffEffects[index], this.gameObject.transform);
        Debug.Log(waterDebuff.name + " 생성");
    }

    // 삭제 예정.
    public void DebuffCheck(int index)
    {
        timerOn = true;
        curDebuffIndex = index;
        switch (index)
        {
            case 0:
                time = 0f;
                objColor.material.color = Color.blue;
                this.gameObject.GetComponentInParent<RangeAITest>().nvAgent.speed *= 0.5f;
                break;
            default:
                break;
        }

    }

    public void DebuffOff()
    {
        time = 0f;
        timerOn = false;
        objColor.material.color = originColor;
        this.gameObject.GetComponentInParent<RangeAITest>().nvAgent.speed = originvelocity;
        //GameObject.Find("Player").GetComponent<Player>().DebuffOff(curDebuffIndex);

    }
}
