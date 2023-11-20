using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WeaponUpdater: MonoBehaviour
{

    GameObject player;
    public GameObject Axe;
    public GameObject Sword;


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
            player.GetComponent<AnimationEventEffects>().playerweapon = Attack.Weapon.Sword;

            Debug.Log(GameManager_JS.Instance.playerWeapon.ToString());

            var resourceName = "Player/Sword/Player Animator";
            var animator = player.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

            Axe.SetActive(false);
            Sword.SetActive(true);
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

            Axe.SetActive(true);
            Sword.SetActive(false);
            Debug.Log("플레이어 무기 : 배틀액스");

        }
    }
}