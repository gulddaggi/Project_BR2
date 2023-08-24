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

    int[] tmpArray;

    [SerializeField]
    Player player;

    public void SetChoiceValue(int[] indexArray)
    {
        tmpArray = new int[3] { indexArray[0], indexArray[1], indexArray[2] };
        typeIndex = tmpArray[0];
        statIndex = tmpArray[1];
        plusValue = tmpArray[2];
    }

    public void ChoiceClicked()
    {
        player.AbilityOnStat(tmpArray);
    }
}
