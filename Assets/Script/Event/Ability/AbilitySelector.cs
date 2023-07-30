using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector : MonoBehaviour
{
    // 등급별 확률
    float[] probs = new float[3]{ 70.0f, 20.0f, 10.0f };

    float total = 100.0f;

    // 추출할 인덱스를 지정하여 반환
    public int[] Select(int index, int numOfType)
    {
        // 반환값. 순서대로 추출할 능력, 등급
        int[] numbers = new int[2]{ 0, 0 };

        // 추출 능력 지정. 3가지 타입이 3개의 선택지에 하나씩 나오도록.
        numbers[0] = (2*index) + Random.Range(0, numOfType);

        //Debug.Log(numOfType + "번째 슬롯의 능력 라인 지정 : " + numbers[0]);

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
