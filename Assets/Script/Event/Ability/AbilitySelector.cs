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
        Debug.Log("리스트 초기화");
        for (int i = 0; i < 10; i++)
        {
            calcProb(i);
        }

        /*
        Debug.Log("리스트 가중치 출력");
        Debug.Log("크기 : " + randomBoxList.Count);
        for (int i = 0; i < randomBoxList.Count; i++)
        {
            Debug.Log("인덱스 : " + i + ", " + randomBoxList[i]);
            
        }
        */
    }

    // 가중치 계산. 이후에 상세 수치 조정 필요.
    void calcProb(int _id)
    {
        float ans;

        // 추첨가능 대상. 가중치 부여.
        if (PreAbitiliyCheck(_id))
        {
            ans = 10.0f;
            total += ans;
        }
        // 추첨 불가.
        else
        {
            ans = -1.0f;
        }
        randomBoxList.Add(ans);

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
                //Debug.Log(i + " 인덱스 스킵");
                continue;
            }

            if (randomValue <= randomBoxList[i])
            {
                num = i;
                total -= randomBoxList[i];
                randomBoxList[i] = -1.0f;
                //randomBoxList.Remove(randomBoxList[i]);
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
        float randomValue = Random.value * 100.0f;
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
        // 기존 선택 능력의 등급을 최소치로서 사용
        int minimumRank = abManager.AbilityRankCheck(ab_index, _id);

        if (minimumRank != -1 && minimumRank > _num)
        {
            Debug.Log("추첨된 능력 등급이 낮아 조정");
            _num = minimumRank;
        }
        return _num;
    }

    // 능력 중복 확인 배열 초기화
    void init()
    {
        Debug.Log("능력 추첨 완료. 초기화 진행");
        ab_index = 0;
        total = 0.0f;
        randomBoxList.Clear();
        return;
    }

    // 선행능력 보유 확인. 선행능력이 없거나, 조건 충족할 경우 true를 반환.
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
            //Debug.Log(_id + " 인덱스의 사전 조건 : ");

            /*for (int i = 0; i < cont.Length; i++)
            {
                Debug.Log(cont[i]);
            }*/

            for (int i = 0; i < cont.Length; i++)
            {
                int tmp = int.Parse(cont[i]);

                // 조건 미충족
                if (!abManager.AbilityCheck(ab_index, tmp))
                {
                    //Debug.Log(_id + " 인덱스의 사전 조건" + tmp + "미충족");
                    return false;
                }
                //Debug.Log(_id + " 인덱스의 사전 조건" + tmp + "충족");
            }
        }
        return true;
    }
}
