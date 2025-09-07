using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon_Demo : MonoBehaviour
{
    GameObject player;
    public GameObject Axe;
    public GameObject Sword;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    public void ButtonClick_Sword() 
    {
        player.GetComponent<Attack>().PlayerWeapon = Attack.Weapon.Sword;
        Debug.Log(player.GetComponent<Attack>().PlayerWeapon.ToString());

        var resourceName = "Animation/Sword/LowPolyHumanAnimator";
        var animator = player.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

        Axe.SetActive(false);
        Sword.SetActive(true);
    }

    public void ButtonClick_Axe() //버튼 클릭 이벤트에 대한 함수를 만들어 준다.
    {
        player.GetComponent<Attack>().PlayerWeapon = Attack.Weapon.Sword;
        Debug.Log(player.GetComponent<Attack>().PlayerWeapon.ToString());

        var resourceName = "Animation/Axe/Axe Override";
        var animator = player.gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(resourceName);

        Axe.SetActive(true);
        Sword.SetActive(false);
    }


}
