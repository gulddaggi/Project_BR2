using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 선택된 능력 처리 담당 클래스.
public class SelectedAbilityProcessor : MonoBehaviour
{
    // 플레이어 상태 클래스 변수. 수치 변경이 필요한 능력 처리 시 접근.
    [SerializeField]
    Player playerStatus;

    // 능력 선택 이벤트 리스너 함수. 일차적으로 능력 종류 구별.
    public void AbilitySelected(int _typeIndex, int _id) // 능력 종류, 해당 종류의 능력 DB 상 id를 인수로 전달받음.
    {
        Debug.Log("선택된 능력 처리");

        switch (_typeIndex)
        {
            case 0:
                Debug.Log("능력 : 물");
                Ability_Water(_id);
                break;
            case 1:
                Debug.Log("능력 : 불");
                break;
            default:
                break;
        }

    }

    // 물 능력 선택 시 적용.
    void Ability_Water(int _id)
    {
        Debug.Log("능력 : Water, 인덱스 : " + _id);

        switch (_id)
        {
            case 0: // 약공격은 더 높은 피해를 가하고 둔화 효과를 입힌다.
                break;
            case 1:
                break;
            default:
                break;
        }
    }
}
