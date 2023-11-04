using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerChangeWeapon : MonoBehaviour
{
    public Attack.Weapon crystal_weapon;

    GameObject player;
    public GameObject Axe;
    public GameObject Sword;
    public GameObject WeaponChangeUI;
    public bool Weapon_Change_Available = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Map_ChangeWeapon()
    {
        if (crystal_weapon == Attack.Weapon.Sword)
        {
            player.GetComponent<Attack>().PlayerWeapon = Attack.Weapon.Sword;
            Debug.Log(player.GetComponent<Attack>().PlayerWeapon.ToString());

            var resourceName = "Animation/Sword/LowPolyHumanAnimator";
            var animator = player.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

            Axe.SetActive(false);
            Sword.SetActive(true);
            Debug.Log("플레이어 무기 : 한손검");
        }
        else if (crystal_weapon == Attack.Weapon.Axe)
        {
            player.GetComponent<Attack>().PlayerWeapon = Attack.Weapon.Axe;
            Debug.Log(player.GetComponent<Attack>().PlayerWeapon.ToString());

            var resourceName = "Animation/Axe/Axe Override";
            var animator = player.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

            Axe.SetActive(true);
            Sword.SetActive(false);
            Debug.Log("플레이어 무기 : 배틀액스");
        }

    }
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < 15f)
        {
            WeaponChangeUI.SetActive(true);
            Weapon_Change_Available = true;
            if(Input.GetKeyDown(KeyCode.E))
            {
                Map_ChangeWeapon();
            }
        }
        else
        {
            WeaponChangeUI.SetActive(false);
            Weapon_Change_Available = false;
        }
    }


}
