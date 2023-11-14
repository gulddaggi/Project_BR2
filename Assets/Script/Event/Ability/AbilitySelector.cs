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

    // 추출할 인덱스를 지정하여 반환
    public int[] Select(int index, int numOfType)
    {
        // 반환값. 순서대로 추출할 능력, 등급
        int[] numbers = new int[2]{ 0, 0 };

        // 추출 능력 지정. 3가지 타입이 3개의 선택지에 하나씩 나오도록.
        // 테스트 목적으로 줄임. 각 하나씩만 추첨없이 반환.
        numbers[0] = ( index == 0 ? 0 : tmp ) + Random.Range(0, 2);

        // 다음 index에 대해 사용 가능하도록 현재 타입 개수만큼 더함
        tmp += numOfType;

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

        if (index == 2)
        {
            tmp = 0;
            Debug.Log("변수 초기화 : " + tmp);
        }

        return numbers;
    }

}
