using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{

    // 차후에 추상 클래스로 개조 필요.
    protected Player player;
    [SerializeField] PlayerController playercontroller;
    public Animator PlayerAnimator;
    public float Strong_Attack_Hold_Time;
    [SerializeField] float Strong_Attack_Hold_Time_Waiting = 2.0f;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        // PlayerAttack();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // 공격키가 눌렸는지 체크
        {
            /* if (Input.GetMouseButton(0)) {
                Strong_Attack_Hold_Time += Time.deltaTime;
                // else { PlayerAnimator.SetTrigger("Weak Attack");  }
            }
            if(Strong_Attack_Hold_Time > Strong_Attack_Hold_Time_Waiting && Input.GetMouseButtonUp(0)) { PlayerAnimator.SetTrigger("Strong Attack"); Strong_Attack_Hold_Time = 0; }        

            else if(Strong_Attack_Hold_Time < Strong_Attack_Hold_Time_Waiting && Input.GetMouseButtonUp(0)) { Strong_Attack_Hold_Time = 0;
            } */
            PlayerAnimator.SetTrigger("Basic Attack");
            StartCoroutine(AttackDelay());
        }
    }

    public IEnumerator AttackDelay()
    {
        player.Player_MoveSpeed_Reclaimer();
        // playercontroller.isAttack = true;
        yield return new WaitForSeconds(1.0f);
        // playercontroller.isAttack = false;
        player.Player_MoveSpeed_Multiplier();
    }
}
