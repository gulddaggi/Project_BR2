using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector : MonoBehaviour
{
    // 등급별 확률
    float[] probs = new float[3] { 70.0f, 25.0f, 5.0f };

    float[] probs_ability = new float[10] { 15.0f, 15.0f, 10.0f, 10.0f, 5.0f, 5.0f, 3.0f, 3.0f, 2.0f, 2.0f };

    //int[] pre_Abil_Check = new int[10] { 0, 0, 0, 0, 1, 0, 0, 1, 1, 1 };

    // 플레이어의 능력 선택 이후마다 업데이트
    float[,] randomBox = new float[4, 10];

    float total = 0.0f;

    // 알맞은 line number를 찾아가도록 가산되는 임시 변수
    int tmp = 0;

    int[] curIndex = new int[3];

    int ab_index = 0;

    // 현재 보유 능력 확인을 위한 클래스 변수
    [SerializeField]
    AbilityListManager abManager;

    // 추출할 인덱스를 지정하여 반환
    public int[] Select(int _ab_index, int index)
    {
        // 반환값. 순서대로 추출할 능력, 등급
        int[] numbers = new int[2] { 0, 0 };
        ab_index = _ab_index;

        // 가중치 배열 초기화
        if (index == 0)
        {
            SetProbs();
        }

        int tmp = 0;
        numbers[0] = SelectNumber();

        /*
        while (tmp < index) // 중복 능력 추첨을 막기 위함
        {
            if (numbers[0] == curIndex[tmp])
            {
                tmp = 0;
                numbers[0] = SelectNumber();
            }
            else
            {
                ++tmp;
            }
        }

        // 중복 여부 확인을 위해 배열에 추가.
        curIndex[index] = numbers[0];
        */
        if (index == 2) init();

        Debug.Log((index + 1) + "번째 슬롯의 능력 라인 지정 : " + numbers[0]);

        numbers[1] = SelectRank();

        return numbers;
    }

    // 선행 능력 충족 여부에 따라 가중치 설정.
    void SetProbs()
    {
        Debug.Log("가중치 초기화");
        for (int i = 0; i < 10; i++)
        {
            randomBox[ab_index, i] = calcProb(i);
            total += randomBox[ab_index, i];
        }
    }

    // 가중치 계산
    float calcProb(int _id)
    {
        float ans;

        if (PreAbitiliyCheck(_id))
        {
            ans = 10.0f;
        }
        else
        {
            ans = -1.0f;
        }

        return ans;
    }

    // 능력 번호 추첨
    int SelectNumber()
    {
        float randomValue = Random.value * total;

        int num = 0;

        // 가중치에 맞는 번호 추첨
        for (int i = 0; i < randomBox.GetLength(0); i++)
        {
            if (randomBox[ab_index, i] == -1.0f)
            {
                continue;
            }

            if (randomValue <= randomBox[ab_index, i])
            {
                num = i;
                total -= randomBox[ab_index, i];
                randomBox[ab_index, i] = -1.0f;
            }
            else
            {
                randomValue -= randomBox[ab_index, i];
            }
        }
        Debug.Log("추첨 번호" + num);
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
        //for (int i = 0; i < curIndex.Length; i++)
        //{
       //    curIndex[i] = -1;
        //}
        ab_index = 0;
        total = 0.0f;
        return;
    }

    // 선행능력 보유 확인
    bool PreAbitiliyCheck(int _id)
    {
        // DB로부터 선행능력 전달
        string[] cont = EventDBManager.instance.GetPreAbility(ab_index, _id);

        if (cont[0] == "")
        {
            return true;
        }
        else
        {
            for (int i = 0; i < cont.Length; i++)
            {
                int tmp = 0;
                int.Parse(cont[i]);
                if (!abManager.AbilityCheck(ab_index, tmp))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
