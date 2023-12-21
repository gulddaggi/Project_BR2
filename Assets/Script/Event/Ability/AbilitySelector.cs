using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector : MonoBehaviour
{
    // 등급별 확률
    float[] rankProbs = new float[3] { 70.0f, 25.0f, 5.0f };

    // 플레이어의 능력 선택 이후마다 업데이트
    List<float> randomBoxList = new List<float>();

    float total = 0.0f;

    // 알맞은 line number를 찾아가도록 가산되는 임시 변수
    int tmp = 0;

    int[] curIndex = new int[3];

    int ab_index = 0;

    // 현재 보유 능력 확인을 위한 클래스 변수.
    // 싱글톤 오브젝트의 자식으로 존재. 차후 변수 등록 추가 필요
    [SerializeField]
    AbilityListManager abManager;

    // 추출할 인덱스를 지정하여 반환
    public int[] Select(int _abIndex, int index)
    {
        // 반환값. 순서대로 추출할 능력, 등급
        int[] numbers = new int[2] { 0, 0 };
        ab_index = _abIndex;

        // 가중치 배열 초기화
        if (index == 0)
        {
            SetProbs();
        }

        // 능력 추첨
        numbers[0] = SelectNumber();
        // 능력 등급 추첨
        numbers[1] = SelectRank(numbers[0]);
        if (index == 2) init();


        return numbers;
    }

    // 선행 능력 충족 여부에 따라 가중치 설정.
    void SetProbs()
    {
        for (int i = 0; i < 10; i++)
        {
            float tmp = calcProb(i);
            if (tmp != -1.0f)
            {
                randomBoxList.Add(tmp);
                total += tmp;
            }
        }
    }

    // 가중치 계산. 이후에 상세 수치 조정 필요.
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
        for (int i = 0; i < randomBoxList.Count; i++)
        {
            if (randomBoxList[i] == -1.0f)
            {
                continue;
            }

            if (randomValue <= randomBoxList[i])
            {
                num = i;
                total -= randomBoxList[i];
                randomBoxList.Remove(randomBoxList[i]);
                break;
            }
            else
            {
                randomValue -= randomBoxList[i];
            }
        }
        return num;
    }

    // 능력 등급 추첨
    int SelectRank(int _id)
    {
        float randomValue = Random.value * total;
        int num = 0;

        for (int i = 0; i < rankProbs.Length; i++)
        {
            if (randomValue <= rankProbs[i])
            {
                num = i;
                break;
            }
            else
            {
                randomValue -= rankProbs[i];
            }
        }

        // 현재 추첨된 등급이 기존 등급보다 낮은 등급인지 확인하고 조정.
        num = MinimumRankCheck(num, _id);

        return num;
    }

    // 등급 확인 및 조정.
    int MinimumRankCheck(int _num, int _id)
    {
        int tmp = abManager.AbilityRankCheck(ab_index, _id);
        if (tmp != -1 && tmp > _num)
        {
            Debug.Log("추첨된 능력 등급이 낮아 조정");
            _num = tmp;
        }
        return _num;
    }

    // 능력 중복 확인 배열 초기화
    void init()
    {
        ab_index = 0;
        total = 0.0f;
        return;
    }

    // 선행능력 보유 확인
    bool PreAbitiliyCheck(int _id)
    {
        // DB로부터 선행능력 전달
        string[] cont = EventDBManager.instance.GetPreAbility(ab_index, _id);

        // 선행능력 없음
        if (cont[0] == "")
        {
            return true;
        }
        // 선행능력 있음
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
