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

    public WeaponChangeEffect[] weaponChangeEffect;
    [System.Serializable]
    public class WeaponChangeEffect
    {
        public GameObject ChangeEffect;
        public float DestroyAfter;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Map_CrystalManage()
    {
        player.GetComponent<WeaponUpdater>().WeaponChange(crystal_weapon);
        InstantiateWeaponChangeEffect();
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
                Map_CrystalManage();
            }
        }
        else
        {
            WeaponChangeUI.SetActive(false);
            Weapon_Change_Available = false;
        }
    }

    void InstantiateWeaponChangeEffect()
    {
        var instance = Instantiate(weaponChangeEffect[GameManager_JS.Instance.PlayerWeaponCheck()].ChangeEffect, player.transform.position, player.transform.rotation);
        instance.transform.parent = player.transform;
        instance.transform.localPosition = Vector3.zero;
        Destroy(instance, weaponChangeEffect[GameManager_JS.Instance.PlayerWeaponCheck()].DestroyAfter);
    }


}
