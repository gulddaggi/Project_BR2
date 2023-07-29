using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using CharacterController;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Rigidbody PlayerRigid;
    public Player player;

    [SerializeField] public Vector3 PlayerMoveDirection;

    protected Vector3 DodgeVec;
    [SerializeField] float Basic_Dodge_CoolDown;
    [SerializeField] float Basic_Dodge_CoolTime = 2.0f;
    [SerializeField] float Basic_Dodge_Time = 0.3f;
    [SerializeField] bool isDodge;
    [SerializeField] public bool isAttack;

    // 직렬화문은 디버깅 용도. 실제 릴리스시에는 제거해야 함

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        PlayerRigid = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }


    // !! Player Input은 Invoke Unity Events로 구성해놓았음 !!
    // Send Messages 방식은 차후에 헷갈릴 가능성 UP 

    #region * Player Input Contextive Functions

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        PlayerMoveDirection = new Vector3(input.x, 0f, input.y);
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed) // 닷지 키가 눌렸는지 체크. 여기서부터 닷지 로직 작성
        {
            if (PlayerRigid.velocity != Vector3.zero && Basic_Dodge_CoolDown > Basic_Dodge_CoolTime)
            {
                DodgeVec = PlayerMoveDirection;
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.eulerAngles.y - 90, transform.rotation.z));
                PlayerAnimator.SetTrigger("Basic Dodge");
                player.Player_MoveSpeed_Multiplier();
                isDodge = true;

                Debug.Log("플레이어 기본 회피");
                // PlayerColor.material.color = Color.red; 디버그용
                Invoke("Basic_Dodge_Out", Basic_Dodge_Time);
            }
        }
    }

    #endregion


    // Player Input 컴포넌트 이벤트 함수용

    #region * Player Input Component Event Functions

    protected void PlayerMove()
    {
        if (PlayerMoveDirection != Vector3.zero)
            { Quaternion LookAngle = Quaternion.LookRotation(PlayerMoveDirection); PlayerRigid.rotation = LookAngle; }
        // 여기까지 인풋값에 따른 플레이어 프리팹 회전
        PlayerRigid.velocity = PlayerMoveDirection * player.MoveSpeed + Vector3.up * PlayerRigid.velocity.y;
    }

    #endregion


    // 플레이어 애니메이션 함수

    #region * 플레이어 애니메이션 Perpetuity 관련 함수

    protected void PlayerAnimation()
    {
        if (PlayerRigid.velocity != Vector3.zero)
        {
            // 속력벡터가 0이 아닐 시
            PlayerAnimator.SetTrigger("Run");
        }
        // 여기까지 뛰는 애니메이션 관련 체크
        if (PlayerRigid.velocity == Vector3.zero) { PlayerAnimator.SetTrigger("Idle"); }
    }

    #endregion


    // 업데이트문에서 움직임-애니메이션-닷지까지 전부 체크

    private void Update()
    {
        PlayerMove();
        PlayerAnimation();
        // if (Input.GetKeyDown(KeyCode.Space)) { Basic_Dodge(); }
        Basic_Dodge_Cooltime_Management();
    }
    public void LookAt(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            transform.rotation = targetAngle;
        }
    }


    // 여기서부터는 플레이어 회피 관련

    #region * Player Basic Dodge

    // 하기 함수는 Old Input System 당시 레거시 닷지 함수

    /* void Dodge()
    {
        if (PlayerRigid.velocity != Vector3.zero && Basic_Dodge_CoolDown > Basic_Dodge_CoolTime)
        {

            DodgeVec = PlayerMoveDirection;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.eulerAngles.y - 90, transform.rotation.z));
            PlayerAnimator.SetTrigger("Basic Dodge");
            player.Player_MoveSpeed_Multiplier();
            isDodge = true;

            Debug.Log("플레이어 기본 회피");
            // PlayerColor.material.color = Color.red; 
            Invoke("Basic_Dodge_Out", Basic_Dodge_Time);
        }
    } */

    void Basic_Dodge_Out()
    {
        player.Player_MoveSpeed_Reclaimer();
        isDodge = false;
        Basic_Dodge_CoolDown = 0;
    }

    void Basic_Dodge_Cooltime_Management()
    {
        if (isDodge == false)
            Basic_Dodge_CoolDown += Time.deltaTime;
    }

    #endregion



    // 여기서부터는 플레이어 충돌처리

    #region * Player Collision Check

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var damagedata = collision.gameObject.GetComponent<Ghoul>(); // 데모용이라 Ghoul.cs로 한 것임! 차후에 Enemy 추상화 이루어지면 수정해야 함!
            player.TakeDamage(damagedata.Damage);

        }
    }

    #endregion

}
