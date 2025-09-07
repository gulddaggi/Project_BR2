using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectManager : MonoBehaviour
{
    public GameObject InitialHitEffectPrefab;
    public GameObject hitEffectPrefab; // 차후 능력에 따라 피격 이펙트 변경 가능성이 있으므로 동적 배열로 설계됨
    public float hitEffectDuration = 1.0f;
    int HitPrefabNum = 0;

    private void Start()
    {
        HitPrefabNum = GameManager_JS.Instance.PlayerAbility + 1;
        Debug_PlayerAbilityCheck();
        SetHitEffect();
    }

    public void ShowHitEffect(Vector3 position, int hitPrefabNum)
    {
        Debug.Log(string.Format("적 피격 이펙트"));
        GameObject initalHitEffectInstance = Instantiate(InitialHitEffectPrefab, position, Quaternion.identity);
        Destroy(initalHitEffectInstance, hitEffectDuration);

        GameObject hitEffectInstance = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectInstance, hitEffectDuration);
    }

    void Debug_PlayerAbilityCheck()
    {
        if (HitPrefabNum == 0)
        {
            Debug.Log("플레이어 능력을 받아오는 데 실패했습니다. 디버그용 공격 이펙트 개시");
        }
        if (HitPrefabNum == 1)
        {
            Debug.Log("플레이어 능력 : 물");
        }
        if (HitPrefabNum == 2)
        {
            Debug.Log("플레이어 능력 : 불");
        }
    }

    void SetHitEffect()
    {
        var DefaultResourceName = "Effect/Default_Hit_Effect";
        InitialHitEffectPrefab = Resources.Load<GameObject>(DefaultResourceName);
        var resourceName = "Effect/Hit_Fire";
        hitEffectPrefab = Resources.Load<GameObject>(resourceName);
    }
}