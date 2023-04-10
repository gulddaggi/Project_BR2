using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator PlayerAnimator;
    public float Strong_Attack_Hold_Time;
    [SerializeField] float Strong_Attack_Hold_Time_Waiting = 2.0f;

    // Update is called once per frame
    void Update()
    {
        PlayerAttack();
    }

    void PlayerAttack() {
        /* if (Input.GetMouseButton(0)) {
            Strong_Attack_Hold_Time += Time.deltaTime;
            // else { PlayerAnimator.SetTrigger("Weak Attack");  }
        }
        if(Strong_Attack_Hold_Time > Strong_Attack_Hold_Time_Waiting && Input.GetMouseButtonUp(0)) { PlayerAnimator.SetTrigger("Strong Attack"); Strong_Attack_Hold_Time = 0; }        
        
        else if(Strong_Attack_Hold_Time < Strong_Attack_Hold_Time_Waiting && Input.GetMouseButtonUp(0)) { Strong_Attack_Hold_Time = 0;
        } */
        if (Input.GetMouseButtonUp(0)) {
            PlayerAnimator.SetTrigger("Basic Attack"); 
        }
    }
}
