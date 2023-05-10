using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Rigidbody PlayerRigid;
    protected Player player;

    [SerializeField] private Vector3 PlayerMoveDirection;

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
    // 닷지 리비전후 벽뜷는 현상 발생. 차후에 해결필요

    #region * Player Input Contextive Functions

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>(); PlayerMoveDirection = new Vector3(input.x, 0f, input.y);
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

    #region * Player Inpit Component Event Functions

    protected void PlayerMove()
    {
        if (PlayerMoveDirection != Vector3.zero)
            { Quaternion LookAngle = Quaternion.LookRotation(PlayerMoveDirection); PlayerRigid.rotation = LookAngle; }
        // 여기까지 인풋값에 따른 플레이어 프리팹 회전
        PlayerRigid.velocity = PlayerMoveDirection * player.MoveSpeed + Vector3.up * PlayerRigid.velocity.y;
    }

    #endregion

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

    private void Update()
    {
        // PlayerMove();
        // 뉴 인풋 시스템이 업데이트문에서는 제대로 작동 안함. 좀 더 알아봐야할 듯
        PlayerMove();
        PlayerAnimation();
        // if (Input.GetKeyDown(KeyCode.Space)) { Basic_Dodge(); }
        Basic_Dodge_Cooltime_Management();
    }

    private void FixedUpdate()
    {
        // is_Player_Attack_Check();
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

    // 

    private void is_Player_Attack_Check()
    {
        if (isAttack == true) { PlayerRigid.velocity = Vector3.zero; }
    }
}
