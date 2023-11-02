using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeWeapon : MonoBehaviour
{
    GameObject player;
    public GameObject Axe;
    public GameObject Sword;
    float dir;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void Map_ChangeWeapon()
    {
        if (gameObject.tag == "Sword")
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
        else if (gameObject.tag == "Axe")
        {
            player.GetComponent<Attack>().PlayerWeapon = Attack.Weapon.Sword;
            Debug.Log(player.GetComponent<Attack>().PlayerWeapon.ToString());

            var resourceName = "Animation/Axe/Axe Override";
            var animator = player.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

            Axe.SetActive(true);
            Sword.SetActive(false);
            Debug.Log("플레이어 무기 : 배틀액스");
        }

    }
    private void OnTriggerEnter(Collision other)

    {
        Debug.Log("범위 내 플레이어 인식");
        if (other.gameObject.CompareTag("Player"))
        {
            Map_ChangeWeapon();
        }
    }

}
