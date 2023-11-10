using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector : MonoBehaviour
{
    // 등급별 확률
    float[] probs = new float[3]{ 70.0f, 25.0f, 5.0f };

    float total = 100.0f;

    // 알맞은 line number를 찾아가도록 가산되는 임시 변수
    int tmp = 0;

    int[] curIndex = new int[3];

    // 추출할 인덱스를 지정하여 반환
    public int[] Select(int index, int tier1Count)
    {
        // 반환값. 순서대로 추출할 능력, 등급
        int[] numbers = new int[2]{ 0, 0 };

        // 선행능력 충족 여부를 확인. 보유 능력 확인 기능 구현 이후 추가.


        int tmp = 0;
        numbers[0] = Random.Range(0, tier1Count);
        // 우선은 1티어 능력 중에서만 추첨하도록 구현.
        while (tmp < index) // 중복 능력 추첨을 막기 위함
        {
            if (numbers[0] == curIndex[tmp])
            {
                tmp = 0;
                numbers[0] = Random.Range(0, tier1Count);
            }
            else
            {
                ++tmp;
            }
        }

        curIndex[index] = numbers[0];

        Debug.Log((index+1) + "번째 슬롯의 능력 라인 지정 : " + numbers[0]);

        float randomValue = Random.value * total;

        // 등급 지정.
        for (int i = 0; i < probs.Length; i++)
        {
            if (randomValue <= probs[i])
            {
                numbers[1] = i;
            }
            else
            {
                randomValue -= probs[i];
            }
        }

        return numbers;
    }
}
