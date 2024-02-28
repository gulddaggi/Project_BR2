using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IListener
{
    public UnityEvent<float, float> OnPlayerHPUpdated;

    // Script Player가 임시로 DB 역할도 대행

    private Vector3 PlayerMoveDirection;

    bool[] debuffOnArray = new bool[6];

    bool isPlayerDead = false;

    private Dictionary<SHOP_EVENT_TYPE, int> eventPlayDic = new Dictionary<SHOP_EVENT_TYPE, int>();

    private List<int> eventPlayList = new List<int>{ 0, 0, 0, 0, 0, 0 };

    // 현재 적에게 적용시킬 수 있는 디버프 확인 배열
    // 각 인덱스는 속성. 값은 0(적용안함), 1(1티어), 2(1티어 업그레이드), 3(2티어)
    private int[] attackDebuffArray = { 0, 0, 0, 0, 0 };
    private int[] stAttackDebuffArray = { 0, 0, 0, 0, 0 };
    private int[] fieldAttackDebuffArray = { 0, 0, 0, 0, 0 };
    private int[] dodgeAttackDebuffArray = { 0, 0, 0, 0, 0 };
    private int[] counterAttackDebuffArray = { 0, 0, 0, 0, 0 };
    private float[] excutionPercentArray = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    private float[] stackDamageArray = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

    // 불 타입 데미지

    public float FullHP { 
        get { return fullHP; } 
        set { 
            fullHP = value;
            if (fullHP < CurrentHP)
            {
                CurrentHP = fullHP;
            }
            OnPlayerHPUpdated.Invoke(FullHP, currentHP); 
        } 
    }
    public float CurrentHP {
        get { return currentHP; }
        set {
            currentHP = value;
            if (currentHP < 0)
            {
                currentHP = 0;
                BeforeDie();
            }
            else if (currentHP > FullHP) currentHP = FullHP;
            OnPlayerHPUpdated.Invoke(FullHP, CurrentHP);
        }
    }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float PlayerAttackDamage { get { return playerAttackDamage; } set { playerAttackDamage = value; } }
    public float PlayerStrongAttackDamage { get { return playerStrongAttackDamage; } set { playerStrongAttackDamage = value; } }
    public float PlayerFieldAttackDamage { get { return playerFieldAttackDamage; } set { playerFieldAttackDamage = value; } }
    public float PlayerDodgeAttackDamage { get { return playerDodgeAttackDamage; } set { playerDodgeAttackDamage = value; } }
    public float PlayerCounterAbilityDamage { get { return playerCounterAbilityDamage; } set { playerCounterAbilityDamage = value; } }

    public float PlayerStackDamage { get { return playerStackDamage; } set { playerStackDamage = value; } } // 단위 : %. 적 체력의 PlayerDrawnDamage(%) 만큼 데미지 적용.

    public float PlayerSpecialAttackFillingAmount { get { return playerSpecialAttackFillingAmount; } set { playerSpecialAttackFillingAmount = value; } }

    // 불 타입 능력 변수 프로퍼티
    public float PlayerAttackBurnDamage { get { return playerAttackBurnDamage; } set { playerAttackBurnDamage = value; } }
    public float PlayerStrongAttackBurnDamamge { get { return playerStrongAttackBurnDamamge; } set { playerStrongAttackBurnDamamge = value; } }
    public float PlayerFieldAttackBurnDamage { get { return playerFieldAttackBurnDamage; } set { playerFieldAttackBurnDamage = value; } }
    public float PlayerDodgeAttackBurnDamage { get { return playerDodgeAttackBurnDamage; } set { playerDodgeAttackBurnDamage = value; } }
    public float PlayerBurstDamage { get { return playerBurstDamage; } set { playerBurstDamage = value; } }
    public float PlayerFireBlessingDamage { get { return playerFireBlessingDamage; } set { playerFireBlessingDamage = value; } }
    public float PlayerCounterIgnitionDamage { get { return playerCounterIgnitionDamage; } set { playerCounterIgnitionDamage = value; } }
    public float PlayerExcutionPercent { get { return playerExcutionPercent; } set { playerExcutionPercent = value; } }

    [SerializeField] protected float fullHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float playerAttackDamage;
    [SerializeField] protected float playerStrongAttackDamage;
    [SerializeField] protected float playerFieldAttackDamage;
    [SerializeField] protected float playerDodgeAttackDamage;
    [SerializeField] protected float playerStackDamage;
    [SerializeField] protected float playerCounterAbilityDamage;
    [SerializeField] protected float playerSpecialAttackFillingAmount;

    [SerializeField] protected float playerAttackBurnDamage;
    [SerializeField] protected float playerStrongAttackBurnDamamge;
    [SerializeField] protected float playerFieldAttackBurnDamage;
    [SerializeField] protected float playerDodgeAttackBurnDamage;
    [SerializeField] protected float playerBurstDamage;
    [SerializeField] protected float playerFireBlessingDamage;
    [SerializeField] protected float playerCounterIgnitionDamage;
    [SerializeField] protected float playerExcutionPercent = -1.0f;

    // 모든 데미지 일괄 계산 함수.
    public void SetPlayerAllDamage(float _value)
    {
        PlayerAttackDamage = PlayerAttackDamage + (PlayerAttackDamage * _value * 0.01f);
        PlayerStrongAttackDamage = PlayerStrongAttackDamage + (PlayerStrongAttackDamage * _value * 0.01f);
        PlayerFieldAttackDamage = PlayerFieldAttackDamage + (PlayerFieldAttackDamage * _value * 0.01f);
        PlayerDodgeAttackDamage = PlayerDodgeAttackDamage + (PlayerDodgeAttackDamage * _value * 0.01f);
    }

    public void PlayerStat(float fullHP, float currentHP, float moveSpeed, float playerAttackDamage, float playerStrongAttackDamage)
    {
        this.fullHP = fullHP;
        this.currentHP = currentHP;
        this.moveSpeed = moveSpeed;
        this.playerAttackDamage = playerAttackDamage;
        this.playerStrongAttackDamage = playerStrongAttackDamage;
    }

    private void Awake()
    {
        isPlayerDead = false;
        currentHP = fullHP;
        for (int i = 0; i < debuffOnArray.Length; i++)
        {
            debuffOnArray[i] = false;
        }
    }

    private void Start()
    {
        EventManager.Instance.AddListener(SHOP_EVENT_TYPE.sHPPotion, this);
        EventManager.Instance.AddListener(SHOP_EVENT_TYPE.sHPReinforce, this);
        EventManager.Instance.AddListener(SHOP_EVENT_TYPE.sWeaponReinforce, this);
        EventManager.Instance.AddListener(SHOP_EVENT_TYPE.sSpeedReinforce, this);
        EventManager.Instance.AddListener(SHOP_EVENT_TYPE.sHPToCoin, this);
        EventManager.Instance.AddListener(SHOP_EVENT_TYPE.sAllReinforce, this);
        OnPlayerHPUpdated.Invoke(FullHP, currentHP);
    }

    public void TakeDamage(float Damage)
    {
        CurrentHP -= Damage;

        if (CurrentHP <= 0 && !isPlayerDead)
        {
            BeforeDie();
        }
    }

    public void BeforeDie()
    {
        isPlayerDead = true;
        this.gameObject.GetComponent<PlayerController>().PlayerAnimator.SetTrigger("Dead");
        this.gameObject.GetComponent<PlayerController>().enabled = false;
        Invoke("Die", 2.5f);
    }

    void Die()
    {
        GameManager_JS.Instance.InitStage();
    }

    // 삭제 예정.
    public float[] PlayerAttack(float EnemyHP)
    {
        float[] returnArray = new float[2] { EnemyHP, -1f };

        // 체력 계산
        returnArray[0] -= playerAttackDamage;

        // 디버프 확인
        for (int i = 0; i < debuffOnArray.Length; i++)
        {
            if (debuffOnArray[i] == true)
            {
                returnArray[1] = (float)i;
            }
        }

        return returnArray;
    }

    // 삭제 예정.
    public float PlayerStrongAttack(float EnemyHP)
    {
        float RemainedHP = EnemyHP;
        RemainedHP -= playerStrongAttackDamage;

        return RemainedHP;
    }

    // 삭제 예정.
    public float[] PlayerDodgeAttack(float EnemyHp)
    {
        float[] returnArray = new float[2] { EnemyHp, -1f };

        // 체력 계산
        returnArray[0] -= PlayerFieldAttackDamage;

        // 디버프 확인
        for (int i = 0; i < debuffOnArray.Length; i++)
        {
            if (debuffOnArray[i] == true)
            {
                returnArray[1] = (float)i;
            }
        }

        return returnArray;
    }

    public void Player_MoveSpeed_Multiplier()
    {
        moveSpeed *= 4;
    }

    public void Player_MoveSpeed_Reclaimer()
    {
        moveSpeed /= 4;
    }

    // 능력 선택에 따른 약공격 디버프 값 변경. SelectedAbilityProcessor에서 사용.
    public void SetDebuffToAttack(int _index, int _value)
    {
        if (attackDebuffArray[_index] < _value)
        {
            attackDebuffArray[_index] = _value;
        }
    }

    // 능력 선택에 따른 강공격 디버프 값 변경. SelectedAbilityProcessor에서 사용.
    public void SetDebuffToStAttack(int _index, int _value)
    {
        if (stAttackDebuffArray[_index] < _value)
        {
            stAttackDebuffArray[_index] = _value;
        }
    }

    // 능력 선택에 따른 필드공격 디버프 값 변경. SelectedAbilityProcessor에서 사용.
    public void SetDebuffToFieldAttack(int _index, int _value)
    {
        if (fieldAttackDebuffArray[_index] < _value)
        {
            fieldAttackDebuffArray[_index] = _value;
        }
    }

    // 능력 선택에 따른 대쉬공격 디버프 값 변경. SelectedAbilityProcessor에서 사용.
    public void SetDebuffToDodgeAttack(int _index, int _value)
    {
        if (dodgeAttackDebuffArray[_index] < _value)
        {
            dodgeAttackDebuffArray[_index] = _value;
        }
    }

    // 능력 선택에 따른 카운터 활성화. SelectedAbilityProcessor에서 사용.
    public void SetDebuffToCounterAttack(int _index, int _value)
    {
        counterAttackDebuffArray[_index] = _value;
    }

    // 능력 선택에 따른 처형 활성화. SelectedAbilityProcessor에서 사용.
    public void SetExcutionAbility(int _index, float _value)
    {
        excutionPercentArray[_index] = _value;
    }

    public void SetStackDamage(int _index, float _value)
    {
        stackDamageArray[_index] = _value;
    }

    // 플레이어에게 약공격으로 피격된 적이 디버프를 확인
    public int[] GetAttackDebuff()
    {
        return attackDebuffArray;
    }

    // 플레이어에게 강공격으로 피격된 적이 디버프를 확인
    public int[] GetStAttackDebuff()
    {
        return stAttackDebuffArray;
    }

    // 플레이어에게 필드공격으로 피격된 적이 디버프를 확인
    public int[] GetFieldAttackDebuff()
    {
        return fieldAttackDebuffArray;
    }

    // 플레이어에게 대쉬공격으로 피격된 적이 디버프를 확인
    public int[] GetDodgeAttackDebuff()
    {
        return dodgeAttackDebuffArray;
    }

    // 플레이어에게 모든 공격에 대해 피격된 적이 처형 디버프를 확인
    public float[] GetExecutionAbilityArray()
    {
        return excutionPercentArray;
    }

    public float[] GetStackDamageArray()
    {
        return stackDamageArray;
    }

    public int[] GetCounterAttackDebuffArray()
    {
        return counterAttackDebuffArray;
    }

    public void EventOn(SHOP_EVENT_TYPE sEventType, Component from, object _param = null)
    {
        switch (sEventType)
        {
            // 최대 체력을 20만큼 늘려준다.
            case SHOP_EVENT_TYPE.sHPReinforce:
                Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sHPReinforce.ToString());
                FullHP += 20f;
                break;

            // 약공격의 피해량이 25% 증가한다.
            case SHOP_EVENT_TYPE.sWeaponReinforce:
                Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sWeaponReinforce.ToString());
                playerAttackDamage *= 1.25f;
                break;

            // 3턴 동안 스테이지에 입장할 때마다 최대 체력의 10%를 회복한다. 지속 턴수 3
            case SHOP_EVENT_TYPE.sHPPotion:
                Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sHPPotion.ToString());
                eventPlayList[(int)SHOP_EVENT_TYPE.sHPPotion] += 3;
                Debug.Log("턴수 증가 : " + eventPlayList[(int)SHOP_EVENT_TYPE.sHPPotion]);
                break;

            // 이동속도가 20% 증가한다. 지속 턴수 5
            case SHOP_EVENT_TYPE.sSpeedReinforce:
                Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sSpeedReinforce.ToString());

                break;

            // 현재 체력이 10 줄어들고 20~40 코인을 획득한다.
            case SHOP_EVENT_TYPE.sHPToCoin:
                Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sHPToCoin.ToString());

                break;

            // 모든 공격의 피해량이 15% 증가한다. 지속 턴수 3
            case SHOP_EVENT_TYPE.sAllReinforce:
                Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sAllReinforce.ToString());

                break;

            default:
                break;
        }

    }

    public void TurnBasedEventOn(int _index)
    {
        if (eventPlayList[_index] > 0)
        {
            switch ((SHOP_EVENT_TYPE)_index)
            {
                case SHOP_EVENT_TYPE.sHPReinforce:
                    break;
                case SHOP_EVENT_TYPE.sWeaponReinforce:
                    break;
                case SHOP_EVENT_TYPE.sHPPotion:
                    Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sHPPotion.ToString());
                    Debug.Log("가산 : " + (FullHP * 0.1f));
                    CurrentHP += (FullHP * 0.1f);
                    --eventPlayList[_index];
                    Debug.Log("인덱스 감소. 현재 : " + eventPlayList[_index]);
                    break;
                case SHOP_EVENT_TYPE.sSpeedReinforce:
                    break;
                case SHOP_EVENT_TYPE.sHPToCoin:
                    break;
                case SHOP_EVENT_TYPE.sAllReinforce:
                    break;
                default:
                    break;
            }
        }
    }
}