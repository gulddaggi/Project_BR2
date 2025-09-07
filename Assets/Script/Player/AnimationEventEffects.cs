using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventEffects : MonoBehaviour {
    

    GameObject player;

    [Header("무기 번호 태그는 스크립트를 참조할 것! 요소의 번호가 무기의 태그")]
    [Header("1. 한손검(Sword) -- 0")]
    [Header("2. 배틀액스(Axe) -- 1")]
    [Header("2. 활(Bow) -- 2")]

    /* 무기 태그 번호 
     * 1. 한손검(Sword) -- 0
     * 2. 배틀액스(Axe) -- 1
     * 3. 활(Bow) -- 2 
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
    }
            
    public void InstantiateEffect(int EffectNumber)
    {
        Transform StartPositionRotation;
        GameObject AnimationEffects;

        StartPositionRotation = Positions[GameManager_JS.Instance.PlayerWeaponCheck()].EffectInstantiatePosition[EffectNumber];
        AnimationEffects = Positions[GameManager_JS.Instance.PlayerWeaponCheck()].Effect[EffectNumber];
        
        if (Positions == null || Positions.Length <= EffectNumber)
        {
            Debug.LogError("Incorrect effect number or effect is null");
        }
        
        var instance = Instantiate(AnimationEffects, StartPositionRotation.position, StartPositionRotation.rotation);

        Debug.Log("Effect Instantiated...");

        if (Positions[GameManager_JS.Instance.PlayerWeaponCheck()].UseLocalPosition[EffectNumber])
        {
            instance.transform.parent = StartPositionRotation.transform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = new Quaternion();
        }
        Destroy(instance, Positions[GameManager_JS.Instance.PlayerWeaponCheck()].DestroyAfter[EffectNumber]);
    }

}
