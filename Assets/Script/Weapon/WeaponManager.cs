using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public DefaultWeaponSystem Weapon { get; private set; } // 플레이어 무기
    public GameObject WeaponObject; // 플레이어 무기 오브젝트
    private List<GameObject> weapons = new List<GameObject>(); // 무기 종류가 많으므로 리스트로 구현
    PlayerController playerController;

    public void SetWeapon(GameObject weapon)
    {
        if (Weapon == null) // 무기를 들고 있지 않는 경우에 무기세팅
        {
            WeaponObject = weapon;
            Weapon = weapon.GetComponent<DefaultWeaponSystem>();
            WeaponObject.SetActive(true);
            playerController.PlayerAnimator.runtimeAnimatorController = Weapon.PlayerWeaponAnimator;
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].Equals(Weapon))
            {
                WeaponObject = weapon;
                WeaponObject.SetActive(true); // 기본적으로 무기는 다 플레이어가 들고 있음. SetActive가 true나 false냐에 따른 차이.
                Weapon = weapon.GetComponent<DefaultWeaponSystem>();
                playerController.PlayerAnimator.runtimeAnimatorController = Weapon.PlayerWeaponAnimator;
                //  Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator; 스테이트 머신 구현 후에 이걸로 수정할것
                continue;
            }
            weapons[i].SetActive(false);
        }
    }
}