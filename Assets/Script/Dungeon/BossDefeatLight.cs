using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeatLight : MonoBehaviour
{
    public Light directionalLight; 
    public float newIntensity = 1.0f;

    void Start()
    {
        // 만약 Directional Light가 설정되지 않았다면, 에러를 출력하고 스크립트를 비활성화합니다.
        if (directionalLight == null)
        {
            Debug.LogError("Directional Light이 할당되지 않았습니다. 인스펙터 체크요망");
            enabled = false;
            return;
        }
        SetNewDiretionalLightIntensity();
    }


    void SetNewDiretionalLightIntensity()
    {
        if( GameManager_JS.Instance.BossKillCount > 0 ) { directionalLight.intensity = 1.0f; }
    }
}
