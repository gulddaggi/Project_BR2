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

    // 현재 적에게 적용시킬 수 있는 디버프 확인 배열
    // 각 인덱스는 속성. 값은 0(적용안함), 1(1티어), 2(1티어 업그레이드), 3(2티어)
    private int[] attackDebuffArray = { 0, 0, 0, 0, 0 };
    private int[] stAttackDebuffArray = { 0, 0, 0, 0, 0 };
    private int[] fieldAttackDebuffArray = { 0, 0, 0, 0, 0 };
    private int[] dodgeAttackDebuffArray = { 0, 0, 0, 0, 0 };

    public float FullHP { get { return fullHP; } set { fullHP = value; OnPlayerHPUpdated.Invoke(FullHP, currentHP); } }
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

    [SerializeField] protected float fullHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float playerAttackDamage;
    [SerializeField] protected float playerStrongAttackDamage;
    [SerializeField] protected float playerFieldAttackDamage;
    [SerializeField] protected float playerDodgeAttackDamage;

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

   public float[] PlayerAttack(float EnemyHP)
    {
        float[] returnArray = new float[2] { EnemyHP, -1f};

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

    public float PlayerStrongAttack(float EnemyHP)
    {
        float RemainedHP = EnemyHP;
        RemainedHP -= playerStrongAttackDamage;

        return RemainedHP;    
    }

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

    /* private void Player_Direction_Check() // 왜 만들었더라..?
    {
        bool isMoving = (PlayerMoveDirection != Vector3.zero);
        if (isMoving) { transform.rotation = Quaternion.LookRotation(PlayerMoveDirection); transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); }
    } */

    public void Player_MoveSpeed_Multiplier()
    {
        moveSpeed *= 4;
    }

    public void Player_MoveSpeed_Reclaimer()
    {
        moveSpeed /= 4;
    }

    public void SetPlayerDamage()
    {

    }

    // 능력 선택에 따른 약공격 디버프 값 변경. SelectedAbilityProcessor에서 사용.
    public void SetDebuffToAttack(int _index, int _value)
    {
        if (attackDebuffArray[_index] < _value)
        {
            Debug.Log("디버프 갱신");
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
        if (stAttackDebuffArray[_index] < _value)
        {
            stAttackDebuffArray[_index] = _value;
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

    // 던전 입장시마다 효과가 발동되는 능력 적용. 해당 함수를 GameManager_JS의 OnStageChanged 이벤트에 AddListner
    public void AddTurnBasedAbility(int _index, int _value)
    {
        // 우선 각 능력별 하나씩 존재하여 switch로 구현
        switch (_index)
        {
            case 0:
                GameManager_JS.Instance.OnStageChanged.AddListener(() => ExchangeWater(_value));
                break;

            default:
                break;
        }
    }

    // 등가교환(물) 능력
    private void ExchangeWater(int _value)
    {
        CurrentHP = CurrentHP + (CurrentHP * (_value * 0.01f));
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

            // 3턴 동안 스테이지에 입장할 때마다 최대 체력의 10%를 회복한다.
            case SHOP_EVENT_TYPE.sHPPotion:
                Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sHPPotion.ToString());
                if (eventPlayDic.ContainsKey(SHOP_EVENT_TYPE.sHPPotion))
                {
                    eventPlayDic[SHOP_EVENT_TYPE.sHPPotion] += 3;
                    Debug.Log("턴수 증가 : " + eventPlayDic[SHOP_EVENT_TYPE.sHPPotion]);
                }
                else
                {
                    eventPlayDic.Add(SHOP_EVENT_TYPE.sHPPotion, 3);
                    Debug.Log("새로 추가 : " + eventPlayDic[SHOP_EVENT_TYPE.sHPPotion]);
                }
                break;
            
            default:
                break;
        }

    }

    public void TurnBasedEventOn()
    {
        foreach (SHOP_EVENT_TYPE item in eventPlayDic.Keys)
        {
            switch (item)
            {
                case SHOP_EVENT_TYPE.sHPReinforce:
                    break;
                case SHOP_EVENT_TYPE.sWeaponReinforce:
                    break;
                case SHOP_EVENT_TYPE.sHPPotion:
                    Debug.Log("이벤트 발생 : " + SHOP_EVENT_TYPE.sHPPotion.ToString());
                    Debug.Log("가산 : " + (FullHP * 0.1f));
                    CurrentHP += (FullHP * 0.1f);
                    break;
                default:
                    break;
            }
        }
    }
}