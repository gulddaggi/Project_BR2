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

    // ����ȭ���� ����� �뵵. ���� �������ÿ��� �����ؾ� ��

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        PlayerRigid = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }

    // !! Player Input�� Invoke Unity Events�� �����س����� !!
    // Send Messages ����� ���Ŀ� �򰥸� ���ɼ� UP 
    // ���� �������� ������ ���� �߻�. ���Ŀ� �ذ��ʿ�

    #region * Player Input Contextive Functions

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>(); PlayerMoveDirection = new Vector3(input.x, 0f, input.y);
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed) // ���� Ű�� ���ȴ��� üũ. ���⼭���� ���� ���� �ۼ�
        {
            if (PlayerRigid.velocity != Vector3.zero && Basic_Dodge_CoolDown > Basic_Dodge_CoolTime)
            {
                DodgeVec = PlayerMoveDirection;
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.eulerAngles.y - 90, transform.rotation.z));
                PlayerAnimator.SetTrigger("Basic Dodge");
                player.Player_MoveSpeed_Multiplier();
                isDodge = true;

                Debug.Log("�÷��̾� �⺻ ȸ��");
                // PlayerColor.material.color = Color.red; ����׿�
                Invoke("Basic_Dodge_Out", Basic_Dodge_Time);
            }
        }
    }

    #endregion

    // Player Input ������Ʈ �̺�Ʈ �Լ���

    #region * Player Inpit Component Event Functions

    protected void PlayerMove()
    {
        if (PlayerMoveDirection != Vector3.zero)
            { Quaternion LookAngle = Quaternion.LookRotation(PlayerMoveDirection); PlayerRigid.rotation = LookAngle; }
        // ������� ��ǲ���� ���� �÷��̾� ������ ȸ��
        PlayerRigid.velocity = PlayerMoveDirection * player.MoveSpeed + Vector3.up * PlayerRigid.velocity.y;
    }

    #endregion

    protected void PlayerAnimation()
    {
        if (PlayerRigid.velocity != Vector3.zero)
        {
            // �ӷº��Ͱ� 0�� �ƴ� ��
            PlayerAnimator.SetTrigger("Run");
        }
        // ������� �ٴ� �ִϸ��̼� ���� üũ
        if (PlayerRigid.velocity == Vector3.zero) { PlayerAnimator.SetTrigger("Idle"); }
    }

    private void Update()
    {
        // PlayerMove();
        // �� ��ǲ �ý����� ������Ʈ�������� ����� �۵� ����. �� �� �˾ƺ����� ��
        PlayerMove();
        PlayerAnimation();
        // if (Input.GetKeyDown(KeyCode.Space)) { Basic_Dodge(); }
        Basic_Dodge_Cooltime_Management();
    }

    private void FixedUpdate()
    {
        // is_Player_Attack_Check();
    }

    // ���⼭���ʹ� �÷��̾� ȸ�� ����

    #region * Player Basic Dodge

    // �ϱ� �Լ��� Old Input System ��� ���Ž� ���� �Լ�

    /* void Dodge()
    {
        if (PlayerRigid.velocity != Vector3.zero && Basic_Dodge_CoolDown > Basic_Dodge_CoolTime)
        {

            DodgeVec = PlayerMoveDirection;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.eulerAngles.y - 90, transform.rotation.z));
            PlayerAnimator.SetTrigger("Basic Dodge");
            player.Player_MoveSpeed_Multiplier();
            isDodge = true;

            Debug.Log("�÷��̾� �⺻ ȸ��");
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
