using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class Player : MonoBehaviour
{

    public static Player Instance { get { return instance; } }
    public StateMachine stateMachine { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }

    private Vector3 PlayerMoveDirection;
    private static Player instance;

    public float FullHP { get { return fullHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float PlayerAttackDamage { get { return playerAttackDamage; } }
    public float PlayerStrongAttackDamage { get { return playerStrongAttackDamage; } }
    public int DodgeBuffer {  get { return DodgeBuffer; } }

    public object PlayerController { get; internal set; }

    [Header("Player Stat")]

    [SerializeField] protected float fullHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float playerAttackDamage;
    [SerializeField] protected float playerStrongAttackDamage;
    [SerializeField] protected int dodgeBuffer;

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
        if (instance == null)
        {
            instance = this;
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    #region * FSM 처리
    private void InitStateMachine()
    {
        PlayerController controller = GetComponent<PlayerController>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller));
    }
    void Start()
    {
        InitStateMachine();
    }
    void Update()
    {
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }
    #endregion

    public void TakeDamage(float Damage)
    {
        currentHP -= Damage;
    }

   public float PlayerAttack(float EnemyHP)
    {
        float RemainedHP = EnemyHP;
        RemainedHP -= playerAttackDamage;

        return RemainedHP;
    }

    public float PlayerStrongAttack(float EnemyHP) 
    {
        float RemainedHP = EnemyHP;
        RemainedHP -= playerStrongAttackDamage;

        return RemainedHP;    
    }

    private void Player_Direction_Check() // 플레이어 방향 디버깅용
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
}