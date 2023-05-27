using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Script Player가 임시로 DB 역할도 대행

    private Vector3 PlayerMoveDirection;

    public float FullHP { get { return fullHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] protected float fullHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float moveSpeed;

    float temp;

    public void PlayerStat(float fullHP, float currentHP, float moveSpeed)
    {
        this.fullHP = fullHP;
        this.currentHP = currentHP;
        this.moveSpeed = moveSpeed;
    }

    private void Player_Direction_Check()
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

    public void AttackManagement_Start()
    {
        temp = moveSpeed;
        moveSpeed = 0;
    }

    public void AttackManagement_End()
    {
        moveSpeed = temp;
    }


}