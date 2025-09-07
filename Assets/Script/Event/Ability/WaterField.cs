using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterField : MonoBehaviour
{
    public Player playerstatus;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("디버프 적용 : " + playerstatus.GetFieldAttackDebuff()[0]);
        //Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
