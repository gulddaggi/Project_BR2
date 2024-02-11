using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WeaponUpdater: MonoBehaviour
{

    GameObject player;
    GameObject Shield;

    public Weapon_Updater weaponUpdater;
    [System.Serializable]
    public class Weapon_Updater
    {
        public GameObject[] Weapons;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        WeaponChange(GameManager_JS.Instance.playerWeapon);
    }

    public void WeaponChange(Attack.Weapon playerWeapon)
    {
        if(playerWeapon == Attack.Weapon.Sword) // Sword 
        {
            GameManager_JS.Instance.playerWeapon = Attack.Weapon.Sword;

            Debug.Log(GameManager_JS.Instance.playerWeapon.ToString());

            var resourceName = "Player/Sword/Player Animator";
            var animator = player.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

            Weapon_Deactivator();
            weaponUpdater.Weapons[1].SetActive(true);
            weaponUpdater.Weapons[0].SetActive(true);

            Debug.Log("플레이어 무기 : 한손검");
        }
        else if(playerWeapon == Attack.Weapon.Axe)
        {
            // player.GetComponent<Attack>().PlayerWeapon = Attack.Weapon.Axe;
            // player.GetComponent<AnimationEventEffects>().playerweapon = Attack.Weapon.Axe;
            GameManager_JS.Instance.playerWeapon = Attack.Weapon.Axe;
            Debug.Log(GameManager_JS.Instance.playerWeapon.ToString());

            var resourceName = "Player/Axe/Axe Override";
            var animator = player.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

            Weapon_Deactivator();
            weaponUpdater.Weapons[2].SetActive(true);
            Debug.Log("플레이어 무기 : 배틀액스");

        }
        else if (playerWeapon == Attack.Weapon.Bow)
        {
            // player.GetComponent<Attack>().PlayerWeapon = Attack.Weapon.Axe;
            // player.GetComponent<AnimationEventEffects>().playerweapon = Attack.Weapon.Axe;
            GameManager_JS.Instance.playerWeapon = Attack.Weapon.Bow;
            Debug.Log(GameManager_JS.Instance.playerWeapon.ToString());

            var resourceName = "Player/Bow/Bow Override";
            var animator = player.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

            Weapon_Deactivator();
            weaponUpdater.Weapons[3].SetActive(true);
            Debug.Log("플레이어 무기 : 활");

        }
        else if (playerWeapon == Attack.Weapon.Shuriken)
        {
            // player.GetComponent<Attack>().PlayerWeapon = Attack.Weapon.Axe;
            // player.GetComponent<AnimationEventEffects>().playerweapon = Attack.Weapon.Axe;
            GameManager_JS.Instance.playerWeapon = Attack.Weapon.Shuriken;
            Debug.Log(GameManager_JS.Instance.playerWeapon.ToString());

            var resourceName = "Player/Shuriken/Shuriken Override";
            var animator = player.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

            Weapon_Deactivator();
            weaponUpdater.Weapons[4].SetActive(true);
            Debug.Log("플레이어 무기 : 수리검");

        }
    }

    void Weapon_Deactivator()
    {
        for(int i = 0; i < weaponUpdater.Weapons.Length; i++)
        {
            weaponUpdater.Weapons[i].SetActive(false);
        }
    }
}