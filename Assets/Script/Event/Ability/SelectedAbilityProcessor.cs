using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 선택된 능력 수치 및 디버프 적용 담당 클래스.
public class SelectedAbilityProcessor : MonoBehaviour
{
    // 플레이어 상태 클래스 변수. 수치 변경이 필요한 능력 처리 시 접근.
    [SerializeField]
    Player playerStatus;

    // 현재 처리할 능력 객체 변수
    Ability_ListComponent selectedAbility;

    // 플레이어 컨트롤러 클래스.
    // 돌진, 필드 공격 등 플레이어 조작과 관련된 능력 발생 이벤트에 AddListener 하기 위해 필요.
    [SerializeField]
    PlayerController playerController;

    // 필드 공격 오브젝트
    [SerializeField]
    GameObject fieldAttackObj;

    // 불의 가호 오브젝트
    [SerializeField]
    GameObject flameFieldObj;

    // 강화 전 기존 데미지 저장 배열. 전투 시작 후 일정시간 강화 수치 복구용으로 사용.
    float[] originDamages;

    // 처리할 능력 전달받는 함수. 일차적으로 능력 종류 구별.
    public void AbilitySelected(Ability_ListComponent _seledtedAb) // 능력 종류, 해당 종류의 능력 DB 상 id를 인수로 전달받음.
    {
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
                Ability_Flame(id);
                break;
        }

    }

    // 물 능력 선택 시 적용.
    void Ability_Water(int _id)
    {
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
                playerStatus.PlayerStrongAttackDamage = curValue * calcValue;

                break;

            // 물의 축복(돌진) : 돌진 시 부딪히는 적에게 피해를 가하고 둔화를 입힌다.
            // 디버프 : 둔화, 적용 수치 : PlayerDodgeDamage
            case 2:
                // 디버프 적용
                playerStatus.SetDebuffToDodgeAttack(0, 1);

                // 수치 적용
                if (playerStatus.PlayerDodgeAttackDamage == 0f)
                {
                    playerStatus.PlayerDodgeAttackDamage = 2f;
                }
                curValue = playerStatus.PlayerDodgeAttackDamage;
                calcValue = selectedAbility.plus_Value * 0.01f + 1f;
                playerStatus.PlayerDodgeAttackDamage = curValue * calcValue;

                // 플레이어 컨트롤러 관련 이벤트에 AddListener.

                break;

            // 등가 교환(물) : 모든 공격력이 10% 감소한다. 이후 매 스테이지 진입 시 체력의 일부를 회복한다.
            // 적용 수치 : 모든 물 능력 데미지 변수
            case 3:
                // 수치 적용. %단위 값을 매개변수로 입력
                playerStatus.SetPlayerAllDamage(-10.0f);

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
                playerStatus.PlayerStackDamage = selectedAbility.plus_Value;
                break;

            // 물의 가호 : 전투 시작 후 10초 동안 모든 공격이 강해진다.
            // 적용 수치 : 모든 물 능력 데미지 변수
            case 5:
                // 강화 전 기존 수치 저장
                originDamages = new float[] { playerStatus.PlayerAttackDamage, playerStatus.PlayerStrongAttackDamage, playerStatus.PlayerFieldAttackDamage, playerStatus.PlayerDodgeAttackDamage };

                // 수치 적용
                calcValue = selectedAbility.plus_Value * 0.01f + 1f;

                // 능력 적용.
                // 던전 입장시마다 효과가 발동되는 물 타입 능력. 관련 이벤트에 콜백 함수를 AddListener
                GameManager_JS.Instance.OnStageChanged.AddListener(() => ReinforceDamage(calcValue));
                break;

            // 습지 생성 : 이동 속도가 증가하며 돌진 후 2초 동안 적을 둔화시키는 습지를 생성한다.
            // 디버프 : 둔화, 적용 수치 : PlayerFieldAttackDamage
            case 6:
                // 디버프 적용
                playerStatus.SetDebuffToFieldAttack(0, 1);

                // 수치 적용
                if (playerStatus.PlayerFieldAttackDamage == 0)
                {
                    playerStatus.PlayerFieldAttackDamage = 1.5f;
                }
                curValue = playerStatus.PlayerFieldAttackDamage;
                calcValue = selectedAbility.plus_Value * 0.01f + 1f;
                playerStatus.PlayerFieldAttackDamage = curValue * calcValue;

                // 능력 적용.
                // 플레이어 컨트롤러 관련 이벤트에 AddListener.
                playerController.OnPlayerFieldAttack.AddListener(() => WaterField());
                break;

            // 정령 결속 강화(물) : 모든 공격 피해가 증가하며 둔화와 익사 효과가 빙결 효과로 강화된다.
            // 디버프 : 빙결, 적용 수치 : 모든 물 능력 데미지 변수
            case 7:
                // 디버프 적용. 선행능력이므로 이후 8, 9에서는 디버프 적용 진행하지 않음.
                playerStatus.SetDebuffToAttack(0, 3);
                playerStatus.SetDebuffToStAttack(0, 3);
                playerStatus.SetDebuffToDodgeAttack(0, 3);
                playerStatus.SetDebuffToFieldAttack(0, 3);

                // 수치 적용
                calcValue = selectedAbility.plus_Value * 0.01f + 1f;

                if (playerStatus.PlayerFieldAttackDamage == 0)
                {
                    playerStatus.PlayerFieldAttackDamage = 1.5f;
                }

                playerStatus.PlayerAttackDamage = playerStatus.PlayerAttackDamage * calcValue;
                playerStatus.PlayerStrongAttackDamage = playerStatus.PlayerStrongAttackDamage * calcValue;
                playerStatus.PlayerFieldAttackDamage = playerStatus.PlayerFieldAttackDamage * calcValue;
                playerStatus.PlayerDodgeAttackDamage = playerStatus.PlayerDodgeAttackDamage * calcValue;

                break;

            // 얼음 갑옷 : 적에게 공격받을 시 적에게 피해를 가하고 적에게 빙결 효과를 입힌다.
            // 디버프 : 빙결, 적용 수치 : PlayerCounterDamage
            case 8:
                // 수치 적용
                calcValue = selectedAbility.plus_Value * 0.01f + 1f;

                if (playerStatus.PlayerCounterAbilityDamage == 0)
                {
                    playerStatus.PlayerCounterAbilityDamage = 10f;
                }

                playerStatus.PlayerCounterAbilityDamage = playerStatus.PlayerCounterAbilityDamage * calcValue;
                break;

            // 빙결 처형 : 빙결 효과를 입은 적의 체력이 20% 이하인 경우 적을 바로 처치한다. 주변의 적에게 빙결 효과를 입힌다.
            // 디버프 : 빙결, 빙결장판, 처형
            case 9:
                playerStatus.SetExcutionAbility(0, true);
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

    // 물의 가호 능력
    private void ReinforceDamage(float _value)
    {
        // 수치 강화
        playerStatus.PlayerAttackDamage = playerStatus.PlayerAttackDamage * _value;
        playerStatus.PlayerStrongAttackDamage = playerStatus.PlayerStrongAttackDamage * _value;
        playerStatus.PlayerFieldAttackDamage = playerStatus.PlayerFieldAttackDamage * _value;
        playerStatus.PlayerDodgeAttackDamage = playerStatus.PlayerDodgeAttackDamage * _value;

        // 10초 후 복구
        Invoke("RestoreDamage", 10f);
    }

    // 물의 가호 능력으로 인한 강화 수치 복구
    private void RestoreDamage()
    {
        playerStatus.PlayerAttackDamage = originDamages[0];
        playerStatus.PlayerStrongAttackDamage = originDamages[1];
        playerStatus.PlayerFieldAttackDamage = originDamages[2];
        playerStatus.PlayerDodgeAttackDamage = originDamages[3];
    }

    // 습지 생성 능력
    private void WaterField()
    {
        GameObject obj = Instantiate(fieldAttackObj, gameObject.transform.parent.position, Quaternion.identity);
        obj.GetComponent<WaterField>().playerstatus = this.GetComponentInParent<Player>();
    }

    // 불 능력 선택 시 적용.
    void Ability_Flame(int _id)
    {
        float curValue, calcValue;

        switch (_id)
        {
            // 불의 축복(약) : 약공격은 적에게 화상 효과를 입힌다.
            // 디버프 : 화상, 적용 수치 : playerAttackBurnDamage
            case 0:                
                // 디버프 적용
                playerStatus.SetDebuffToAttack(1, 1);

                // 수치 적용
                curValue = selectedAbility.plus_Value;
                playerStatus.PlayerAttackBurnDamage = curValue;
                break;

            // 불의 축복(강) : 강공격은 적에게 화상 효과를 입힌다.
            // 디버프 : 화상, 적용 수치 : playerStrongAttackBurnDamamge
            case 1:
                // 디버프 적용
                playerStatus.SetDebuffToStAttack(1, 1);

                // 수치 적용
                curValue = selectedAbility.plus_Value;
                playerStatus.PlayerStrongAttackBurnDamamge = curValue;
                break;

            // 불의 축복(돌진) : 돌진 시 부딪히는 적에게 화상 효과를 입힌다.
            // 디버프 : 화상, 적용 수치 : playerDodgeAttackBurnDamage
            case 2:
                // 디버프 적용
                playerStatus.SetDebuffToDodgeAttack(1, 1);

                // 수치 적용
                if (playerStatus.PlayerDodgeAttackDamage == 0f)
                {
                    playerStatus.PlayerDodgeAttackDamage = 2f;
                }

                curValue = selectedAbility.plus_Value;
                playerStatus.PlayerDodgeAttackBurnDamage = curValue;
                break;

            // 등가 교환(불) : 최대 체력이 10% 감소한다. 이후 모든 피해가 증가한다.
            // 적용 수치 : 모든 불 능력 데미지 변수
            case 3:
                // 수치 적용.
                // 체력 감소
                playerStatus.FullHP = playerStatus.FullHP - playerStatus.FullHP * 0.1f;
                // 모든 피해 증가
                curValue = selectedAbility.plus_Value;
                playerStatus.SetPlayerAllDamage(curValue);
                break;

            // 화염 침식 : 화상이 5회 중첩될 경우 적에게 파열 효과를 입힌다.
            // 디버프 : 화상 + 5중첩 시 파열, 적용 수치 : stackDamageArray[1]
            case 4:
                // 디버프 적용
                playerStatus.SetDebuffToAttack(1, 2);
                playerStatus.SetDebuffToStAttack(1, 2);
                playerStatus.SetDebuffToDodgeAttack(1, 2);
                playerStatus.SetDebuffToFieldAttack(1, 2);

                // 수치 적용.
                playerStatus.SetStackDamage(1, selectedAbility.plus_Value);
                break;

            // 불의 가호 : 전투 시작 후 10초 동안 주변 적에게 초당 피해를 입힌다.
            // 적용 수치 : PlayerFireBlessingDamage
            case 5:
                curValue = selectedAbility.plus_Value;
                GameManager_JS.Instance.OnStageChanged.AddListener(() => FlameAbilityFieldOn(curValue));
                break;

            // 화염 지대 생성 : 이동 속도가 증가하며 돌진 후 2초 동안 적에게 화상 효과를 입히는 화염 지대를 생성한다.
            // 디버프 : 화상, 적용 수치 : playerFieldAttackBurnDamage
            case 6:
                break;

            // 정령 결속 강화(불) : 모든 공격 피해가 증가하며 화상과 파열 효과가 점화 효과로 강화된다.
            // 디버프 : 점화, 적용 수치 : 모든 불 능력 데미지
            case 7:
                break;

            // 업화 갑옷 : 적에게 공격받을 시 적에게 점화 효과를 입힌다.
            // 디버프 : 빙결, 적용 수치 : PlayerCounterDamage
            case 8:
                break;

            // 화염 처형 : 점화 효과를 입은 적의 체력이 25% 이하인 경우 적을 바로 처치하며 주변 적에게 점화 효과를 입힌다.
            // 디버프 : 점화, 처형, 처형 시 점화 전파
            case 9:
                break;

            default:
                break;
        }
    }

    // 불의 가호 능력
    void FlameAbilityFieldOn(float _damage)
    {
        flameFieldObj.SetActive(true);
        flameFieldObj.GetComponent<FlameAbilityField>().damage = _damage;
    }
}
