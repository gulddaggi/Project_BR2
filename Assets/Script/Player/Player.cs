using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Script Player가 임시로 DB 역할도 대행

    private Vector3 PlayerMoveDirection;

    public float FullHP { get { return fullHP; } }
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float PlayerAttackDamage { get { return playerAttackDamage; } set { playerAttackDamage = value; } }
    public float PlayerStrongAttackDamage { get { return playerStrongAttackDamage; } }

    [SerializeField] protected float fullHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float playerAttackDamage;
    [SerializeField] protected float playerStrongAttackDamage;

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


}