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
        Debug.Log("reward index : " + index);
        GameObject reward = rewardprefs[index];
        //rewardprefs.Remove(rewardprefs[index]);
        return reward;
    }

    public GameObject CreateReward(int max)
    {
        int index = Random.Range(0, max); // 무작위 난수 생성
        GameObject reward = rewardprefs[index];
        //rewardprefs.Remove(rewardprefs[index]);
        //rewardprefs.Sort();
        return reward;
    }

}
