using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityChoice : MonoBehaviour
{
    // 능력의 타입 인덱스. 보상 정령에 따라 다름.
    // 0 : 물
    [SerializeField]
    int typeIndex;

    // 적용되는 능력.
    // 0 : 약공격, 1 : 강공격, 2 : 장판 & 돌진공격 , 3 : 이동속도
    [SerializeField]
    int statIndex;

    // 가산 수치.
    [SerializeField]
    int plusValue;

    int[] tmpArray = new int[3];

    [SerializeField]
    Player player;

    public void SetChoiceValue(int[] indexArray)
    {
        typeIndex = indexArray[0];
        statIndex = indexArray[1];
        plusValue = indexArray[2];

        tmpArray[0] = typeIndex;
        tmpArray[1] = statIndex;
        tmpArray[2] = plusValue;
    }

    public void ChoiceClicked()
    {
        //List<int> indexList = new List<int> { typeIndex, statIndex, plusValue };
        Debug.Log(tmpArray[1]);
        //Player playerStat = GameObject.Find("Player").GetComponent<Player>();
        player.AbilityOnStat(tmpArray);
    }
}
