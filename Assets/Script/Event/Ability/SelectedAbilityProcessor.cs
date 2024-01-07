using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 선택된 능력 처리 담당 클래스.
public class SelectedAbilityProcessor : MonoBehaviour
{
    // 플레이어 상태 클래스 변수. 수치 변경이 필요한 능력 처리 시 접근.
    [SerializeField]
    Player playerStatus;

    // 현재 처리할 능력 객체 변수
    Ability_ListComponent selectedAbility;

    // 디버프 프리팹 배열
    [SerializeField]
    ParticleSystem[] debuffEffects;

    // 처리할 능력 전달받는 함수. 일차적으로 능력 종류 구별.
    public void AbilitySelected(Ability_ListComponent _seledtedAb) // 능력 종류, 해당 종류의 능력 DB 상 id를 인수로 전달받음.
    {
        Debug.Log("선택된 능력 처리");

        selectedAbility = _seledtedAb;

        int typeIndex = selectedAbility.indexArr[0];
        int id = selectedAbility.indexArr[1];

        // 능력 타입 구별
        switch (typeIndex)
        {
            // 물
            case 0:
                Ability_Water(id);
                break;

            // 불
            case 1:
                break;
        }

    }

    // 물 능력 선택 시 적용.
    void Ability_Water(int _id)
    {
        Debug.Log("능력 : Water, 인덱스 : " + _id);
        float curValue, calcValue;

        switch (_id)
        {
            // 물의 축복(약) : 약공격은 더 높은 피해를 가하고 둔화 효과를 입힌다.
            // 디버프 : 둔화(1티어), 적용 수치 : PlayerAttackDamage
            case 0:
                // 디버프 적용
                playerStatus.SetDebuffToAttack(0, 1);
                
                // 수치 적용
                curValue = playerStatus.PlayerAttackDamage;
                calcValue = selectedAbility.plus_Value * 0.01f + 1f;
                Debug.Log(curValue + " X " + calcValue + " = " + curValue * calcValue);
                playerStatus.PlayerAttackDamage = curValue * calcValue;
                
                break;

            // 물의 축복(강) : 강공격은 더 높은 피해를 가하고 둔화 효과를 입힌다.
            // 디버프 : 둔화, 적용 수치 : PlayerStrongAttackDamage
            case 1:
                // 디버프 적용
                playerStatus.SetDebuffToStAttack(0, 1);

                // 수치 적용
                curValue = playerStatus.PlayerStrongAttackDamage;
                calcValue = selectedAbility.plus_Value * 0.01f + 1f;
                Debug.Log(curValue + " X " + calcValue + " = " + curValue * calcValue);
                playerStatus.PlayerStrongAttackDamage = curValue * calcValue;

                break;

            // 물의 축복(돌진) : 돌진 시 부딪히는 적에게 피해를 가하고 둔화를 입힌다.
            // 디버프 : 둔화, 적용 수치 : PlayerDodgeDamage
            case 2:
                // 디버프 적용
                playerStatus.SetDebuffToDodgeAttack(0, 1);
                if (playerStatus.PlayerDodgeAttackDamage == 0f)
                {
                    playerStatus.PlayerDodgeAttackDamage = 2f;
                }

                // 수치 적용
                curValue = playerStatus.PlayerDodgeAttackDamage;
                calcValue = selectedAbility.plus_Value * 0.01f + 1f;
                playerStatus.PlayerDodgeAttackDamage = curValue * calcValue;
                Debug.Log(curValue + " X " + calcValue + " = " + curValue * calcValue);
                break;

            // 등가 교환(물) : 모든 공격력이 10% 감소한다. 이후 매 스테이지 진입 시 체력의 일부를 회복한다.
            // 적용 수치 : 모든 물 능력 데미지 변수
            case 3:
                // 수치 적용. %단위 값을 매개변수로 입력
                playerStatus.SetPlayerAllDamage(10.0f);

                // 능력 적용.
                // 던전 입장시마다 효과가 발동되는 물 타입 능력. 관련 이벤트에 콜백 함수를 AddListener
                GameManager_JS.Instance.OnStageChanged.AddListener(() => ExchangeWater(selectedAbility.plus_Value));
                break;

            // 수압 증가 : 둔화가 5번 중첩될 경우 적을 익사시킨다.
            // 디버프 : 둔화 + 5중첩 시 익사, 적용 수치 : PlayerDrowningDamage(생성 필요)
            case 4:
                // 디버프 적용
                playerStatus.SetDebuffToAttack(0, 2);
                playerStatus.SetDebuffToStAttack(0, 2);
                playerStatus.SetDebuffToDodgeAttack(0, 2);
                playerStatus.SetDebuffToFieldAttack(0, 2);

                // 수치 적용. 가산 수치 그대로 적용하여, 이후 해당 디버프 적용 시 적 체력의 PlayerDrawnDamage(%)만큼 체력 감소.
                playerStatus.PlayerDrawnDamage = selectedAbility.plus_Value;
                break;

            // 물의 가호 : 전투 시작 후 10초 동안 모든 공격이 강해진다.
            // 적용 수치 : 모든 물 능력 데미지 변수
            case 5:
                break;

            // 습지 생성 : 이동 속도가 증가하며 돌진 후 2초 동안 적을 둔화시키는 습지를 생성한다.
            // 디버프 : 둔화, 적용 수치 : PlayerFieldAttackDamage
            case 6:
                break;

            // 정령 결속 강화(물) : 모든 공격 피해가 증가하며 둔화와 익사 효과가 빙결 효과로 강화된다.
            // 디버프 : 둔화 + 5중첩 시 익사, 적용 수치 : 모든 물 능력 데미지 변수
            case 7:
                break;

            // 얼음 갑옷 : 적에게 공격받을 시 적에게 피해를 가하고 적에게 빙결 효과를 입힌다.
            // 디버프 : 빙결, 적용 수치 : PlayerCounterDamage
            case 8:
                break;

            // 빙결 처형 : 빙결 효과를 입은 적의 체력이 20% 이하인 경우 적을 바로 처치한다. 주변의 적에게 빙결 효과를 입힌다.
            // 디버프 : 빙결, 빙결장판, 처형
            case 9:
                break;

            default:
                break;
        }
    }

    // 등가교환(물) 능력
    private void ExchangeWater(float _value)
    {
        float curValue = playerStatus.CurrentHP;
        float calcValue = _value * 0.01f + 1f;
        playerStatus.CurrentHP = curValue  * calcValue;
    }
}
