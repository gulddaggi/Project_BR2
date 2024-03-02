using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectManager : MonoBehaviour
{
    public GameObject[] hitEffectPrefab; // 차후 능력에 따라 피격 이펙트 변경 가능성이 있으므로 동적 배열로 설계됨
    public float hitEffectDuration = 1.0f;

    private void Start()
    {
        if (hitEffectPrefab == null)
        {
            Debug.LogError("Hit Effect Prefab is not Allocated.");
            enabled = false; 
        }
    }

    public void ShowHitEffect(Vector3 position, int hitPrefabNum)
    {
        Debug.Log(string.Format("적 피격 이펙트"));
        GameObject hitEffectInstance = Instantiate(hitEffectPrefab[hitPrefabNum], position, Quaternion.identity);
        Destroy(hitEffectInstance, hitEffectDuration);
    }
}