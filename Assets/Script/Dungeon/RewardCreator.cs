using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardCreator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> rewardPrefs = new List<GameObject>();

    [SerializeField]
    List<GameObject> abilityPrefs = new List<GameObject>();

    // 보상 생성
    public GameObject CreateReward()
    {
        int index = Random.Range(0, rewardPrefs.Count); // 무작위 난수 생성
        GameObject reward;
        //Debug.Log("reward index : " + index);
        if (index == 0) // 0은 능력 
        {
            // 능력 생성 함수 실행.
            reward = CreateAbility(0);
        }
        else
        {
            reward = rewardPrefs[index];
        }

        //rewardprefs.Remove(rewardprefs[index]);
        return reward;
    }

    // CreateReward 오버로딩. 시작 스테이지에서 
    public GameObject CreateReward(int max)
    {
        if (SceneManager.GetActiveScene().name == "BossStage")
        {
            return null;
        }
        int index = Random.Range(0, max); // 무작위 난수 생성
        GameObject reward;
        if (index == 0) // 0은 능력 
        {
            // 능력 생성 함수 실행.
            reward = CreateAbility();
        }
        else
        {
            reward = rewardPrefs[index];
        }

        //rewardprefs.Remove(rewardprefs[index]);
        //rewardprefs.Sort();
        return reward;
    }

    // 테스트용 보상 생성 함수.
    public GameObject CreateReward(int index, bool trigger)
    {
        GameObject reward;
        if (index == 0) // 0은 능력 
        {
            // 능력 생성 함수 실행. 무작위 능력 대상
            //reward = CreateAbility();
            // 특정 능력 대상. 테스트용
            reward = CreateAbility(0);
        }
        else
        {
            reward = rewardPrefs[index];
        }

        return reward;
    }

    // 능력 생성
    private GameObject CreateAbility()
    {
        int index = Random.Range(0, abilityPrefs.Count); // 무작위 난수 생성
        GameObject ability = abilityPrefs[index];
        return ability;
    }

    // 능력 생성. 테스트용
    private GameObject CreateAbility(int testTarget)
    {
        //int index = Random.Range(0, abilityPrefs.Count); // 무작위 난수 생성
        GameObject ability = abilityPrefs[testTarget];
        return ability;
    }
}
