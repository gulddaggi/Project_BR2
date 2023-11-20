using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector : MonoBehaviour
{
    // 등급별 확률
    float[] probs = new float[3] { 70.0f, 25.0f, 5.0f };

    float[] probs_ability = new float[10] { 10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f, 10.0f };

    int[] pre_Abil_Check = new int[10] { 0, 0, 0, 0, 1, 0, 0, 1, 1, 1 };

    float total = 100.0f;

    // 알맞은 line number를 찾아가도록 가산되는 임시 변수
    int tmp = 0;

    int[] curIndex = new int[3];

    int ab_index = 0;

    // 현재 보유 능력 확인을 위한 클래스 변수
    [SerializeField]
    AbilityListManager abManager;

    // 추출할 인덱스를 지정하여 반환
    public int[] Select(int _ab_index, int index, int tier1Count)
    {
        // 반환값. 순서대로 추출할 능력, 등급
        int[] numbers = new int[2] { 0, 0 };
        ab_index = _ab_index;

        int tmp = 0;
        //numbers[0] = Random.Range(0, tier1Count);

        while (tmp < index) // 중복 능력 추첨을 막기 위함
        {
            if (numbers[0] == curIndex[tmp])
            {
                tmp = 0;
                // 능력 추첨 및 선행능력 보유 확인
                numbers[0] = SelectNumber();
                if (numbers[0] == -1)
                {
                    numbers[0] = curIndex[tmp];
                }
            }
            else
            {
                ++tmp;
            }
        }

        // 중복 여부 확인을 위해 배열에 추가.
        curIndex[index] = numbers[0];
        if (index == 2) init();

        Debug.Log((index + 1) + "번째 슬롯의 능력 라인 지정 : " + numbers[0]);

        numbers[1] = SelectRank();

        return numbers;
    }

    // 능력 번호 추첨
    int SelectNumber()
    {
        float randomValue = Random.value * total;
        int num = 0;

        // 가중치에 맞는 번호 추첨
        for (int i = 0; i < probs_ability.Length; i++)
        {
            if (randomValue <= probs_ability[i])
            {
                num = i;
                // 선행능력 보유 확인
                if (pre_Abil_Check[i] == 1)
                {
                    if (PreAbitiliyCheck(num))
                    {
                        return num;
                    }
                }
            }
            else
            {
                randomValue -= probs_ability[i];
            }
        }
        
    return num;
    }

    // 능력 등급 추첨
    int SelectRank()
    {
        float randomValue = Random.value * total;
        int num = 0;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomValue <= probs[i])
            {
                num = i;
                break;
            }
            else
            {
                randomValue -= probs[i];
            }
        }

        return num;
    }

    // 능력 중복 확인 배열 초기화
    void init()
    {
        for (int i = 0; i < curIndex.Length; i++)
        {
            curIndex[i] = -1;
        }
        ab_index = 0;
        return;
    }

    // 선행능력 보유 확인
    bool PreAbitiliyCheck(int _id)
    {
        string[] cont = EventDBManager.instance.GetPreAbility(ab_index, _id);

        for (int i = 0; i < cont.Length; i++)
        {
            if (!abManager.AbilityCheck(ab_index, cont[i]))
            {
                return false;
            }
        }
        return true;
    }
}
