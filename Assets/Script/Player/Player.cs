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

    private Dictionary<SHOP_EVENT_TYPE, int> eventPlayDic = new Dictionary<SHOP_EVENT_TYPE, int>();

    public float FullHP { get { return fullHP; } set { fullHP = value; OnPlayerHPUpdated.Invoke(FullHP, currentHP); } }
    public float CurrentHP { get { return currentHP; } set { currentHP = value; OnPlayerHPUpdated.Invoke(FullHP, CurrentHP); } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float PlayerAttackDamage { get { return playerAttackDamage; } set { playerAttackDamage = value; } }
    public float PlayerStrongAttackDamage { get { return playerStrongAttackDamage; } set { playerStrongAttackDamage = value; } }
    public float PlayerFieldAttackDamage { get { return playerFieldAttackDamage; } set { playerFieldAttackDamage = value; } }

    [SerializeField] protected float fullHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float playerAttackDamage;
    [SerializeField] protected float playerStrongAttackDamage;
    [SerializeField] protected float playerFieldAttackDamage;

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
        currentHP = fullHP;
        for (int i = 0; i < debuffOnArray.Length; i++)
        {
            debuffOnArray[i] = false;
        }
    }

    private void Start()
    {
        EventManager.Instance.AddListener(SHOP_EVENT_TYPE.sHPPotion, this);
        OnPlayerHPUpdated.Invoke(FullHP, currentHP);
    }

    public void TakeDamage(float Damage)
    {
        currentHP -= Damage;

        if (currentHP <= 0)
        {
            this.gameObject.GetComponent<PlayerController>().PlayerAnimator.SetTrigger("Dead");
            this.gameObject.GetComponent<PlayerController>().enabled = false;
            Invoke("Die", 3f);
        }
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

    private void Player_Direction_Check() // 왜 만들었더라..?
    {
        bool isMoving = (PlayerMoveDirection != Vector3.zero);
        if (isMoving) { transform.rotation = Quaternion.LookRotation(PlayerMoveDirection); transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); }
    }

    public void Player_MoveSpeed_Multiplier()
    {
        moveSpeed *= 4;
    }

    public void Player_MoveSpeed_Reclaimer()
    {
        moveSpeed /= 4;
    }

    public void AbilityOnStat(int[] indexList)
    {
        DebuffOn(indexList[0]);
        switch (indexList[1])
        {
            // 약공격
            case 0:
                PlayerAttackDamage *= (1f + (indexList[2] * 0.01f));
                break;

            // 강공격
            case 1:
                PlayerStrongAttackDamage *= (1f + (indexList[2] * 0.01f));
                break;

            // 장판 & 돌진공격
            case 2:
                PlayerFieldAttackDamage = PlayerAttackDamage * (indexList[2] * 0.01f);
                break;

            // 이동속도
            case 3:
                MoveSpeed *= (1f + (indexList[2] * 0.01f));
                break;

            default:
                break;
        }
    }

    void DebuffOn(int typeIndex)
    {
        debuffOnArray[typeIndex] = true;
    }

    public void DebuffOff(int typeIndex)
    {
        debuffOnArray[typeIndex] = false;
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
                    CurrentHP += (FullHP * 0.1f);
                    break;
                default:
                    break;
            }
        }
    }

}