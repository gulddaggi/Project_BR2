using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventEffects : MonoBehaviour {
    

    GameObject player;
    public Attack.Weapon playerweapon;

    [Header("무기 번호 태그는 스크립트를 참조할 것! 요소의 번호가 무기의 태그")]
    [Header("1. 한손검(Sword) -- 0")]
    [Header("2. 배틀액스(Axe) -- 1")]

    /* 무기 태그 번호 
     * 1. 한손검(Sword) -- 0
     * 2. 배틀액스(Axe) -- 1
     * 3. 활(Bow) -- 2 추후 추가 
     */

    /*
    public EffectInfo[] Effects;

    [System.Serializable]

    public class EffectInfo
    {
        public GameObject Effect;
        public float DestroyAfter = 10;
        public bool UseLocalPosition = true;
    }
    */

    public Position[] Positions;

    [System.Serializable]
    public class Position
    {
        public GameObject[] Effect;
        public Transform[] EffectInstantiatePosition;
        public float[] DestroyAfter;
        public bool[] UseLocalPosition;
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerweapon = player.GetComponent<Attack>().PlayerWeapon;
    }
            
    public void InstantiateEffect(int EffectNumber)
    {
        Transform StartPositionRotation;
        GameObject AnimationEffects;

        StartPositionRotation = Positions[PlayerWeaponCheck()].EffectInstantiatePosition[EffectNumber];
        AnimationEffects = Positions[PlayerWeaponCheck()].Effect[EffectNumber];

        if (Positions == null || Positions.Length <= EffectNumber)
        {
            Debug.LogError("Incorrect effect number or effect is null");
        }

        var instance = Instantiate(AnimationEffects, StartPositionRotation.position, StartPositionRotation.rotation);
        Debug.Log("Effect Instantiate...");

        if (Positions[PlayerWeaponCheck()].UseLocalPosition[EffectNumber])
        {
            instance.transform.parent = StartPositionRotation.transform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = new Quaternion();
        }
        Destroy(instance, Positions[PlayerWeaponCheck()].DestroyAfter[EffectNumber]);
    }

    public int PlayerWeaponCheck()
    {
        if(playerweapon == Attack.Weapon.Sword)
        {
            Debug.Log("[플레이어 이펙트 콘솔] : 플레이어 무기 체크 -> 한손검[태그번호  : 0]");
            return 0;
        }
        else if(playerweapon == Attack.Weapon.Axe)
        {
            Debug.Log("[플레이어 이펙트 콘솔] : 플레이어 무기 체크 -> 배틀액스[태그번호  : 1]");
            return 1;
        }

        return 0;
    }
}
