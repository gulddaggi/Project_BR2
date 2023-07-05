using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCreator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> rewardprefs = new List<GameObject>();

    public GameObject CreateReward()
    {
        int index = Random.Range(0, rewardprefs.Count); // 무작위 난수 생성
        //Debug.Log("reward index : " + index);
        if (index == 0) // 0은 능력 
        {
            // 능력 생성 함수 실행.
            this.gameObject.GetComponent<AbilityCreator>().AbilityCrate();
        }
        GameObject reward = rewardprefs[index];
        //rewardprefs.Remove(rewardprefs[index]);
        return reward;
    }

    // 오버로딩 함수. 시작 스테이지에서 
    public GameObject CreateReward(int max)
    {
        int index = Random.Range(0, max); // 무작위 난수 생성
        GameObject reward = rewardprefs[index];
        //rewardprefs.Remove(rewardprefs[index]);
        //rewardprefs.Sort();
        return reward;
    }

}
