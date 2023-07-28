using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefaultWeaponSystem : MonoBehaviour
{
    public RuntimeAnimatorController PlayerWeaponAnimator { get { return playerWeaponAnimator; } }

    public float WeaponAttackDamage { get { return weaponAttackDamage; } }
    public float WeaponAttackSpeed { get { return weaponAttackSpeed; } }
    public int WeaponComboCount { get; set; }

    [Header("플레이어 무기 데이터")]
    [SerializeField] protected string weaponName;
    [SerializeField] protected RuntimeAnimatorController playerWeaponAnimator;
    [SerializeField] protected float weaponAttackDamage;
    [SerializeField] protected float weaponAttackSpeed;

    public void SetWeaponData(string weapon_name, float attackDamage, float attackSpeed)
    {
        this.weaponName = weapon_name;
        this.weaponAttackDamage = attackDamage;
        this.weaponAttackSpeed = attackSpeed;
    }


    // 여기서부터 스테이트 머신 구현 후 공격 관련 스크립트 이월해야 함.

}
