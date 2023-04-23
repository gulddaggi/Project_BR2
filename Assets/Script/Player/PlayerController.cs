using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    protected Rigidbody PlayerRigid;
    protected Player player;
    [SerializeField] private Vector3 PlayerMoveDirection;

    protected Vector3 DodgeVec;
    [SerializeField] float Basic_Dodge_CoolDown;
    [SerializeField] float Basic_Dodge_CoolTime = 2.0f;
    [SerializeField] float Basic_Dodge_Time = 0.3f;
    [SerializeField] bool isDodge;

    // ����ȭ���� ����� �뵵. ���� �������ÿ��� �����ؾ� ��

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        PlayerRigid = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }

    public void OnMoveInput(InputAction.CallbackContext context) { 
        Vector2 input = context.ReadValue<Vector2>(); PlayerMoveDirection = new Vector3(input.x, 0f, input.y); }
    // Player Input ������Ʈ �̺�Ʈ �Լ���

    protected void PlayerMove()
    {
        if (PlayerMoveDirection != Vector3.zero)
            { Quaternion LookAngle = Quaternion.LookRotation(PlayerMoveDirection); PlayerRigid.rotation = LookAngle; }
        // ������� ��ǲ���� ���� �÷��̾� ������ ȸ��
        PlayerRigid.velocity = PlayerMoveDirection * player.MoveSpeed + Vector3.up * PlayerRigid.velocity.y;
    }
    
    protected void PlayerAnimation()
    {
        if (PlayerRigid.velocity != Vector3.zero)
        {
            // �ӷº��Ͱ� 0�� �ƴ� ��
            PlayerAnimator.SetTrigger("Run");
        }
        // ������� �ٴ� �ִϸ��̼� ���� üũ

        if (PlayerRigid.velocity == Vector3.zero)
        {
            PlayerAnimator.SetTrigger("Idle");
        }
    }

    private void Update()
    {
        // PlayerMove();
        // �� ��ǲ �ý����� ������Ʈ�������� ����� �۵� ����. �� �� �˾ƺ����� ��

        if (Input.GetKeyDown(KeyCode.Space)) { Basic_Dodge(); }
        Basic_Dodge_Cooltime_Management();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerAnimation();
    }

    // ���⼭���ʹ� �÷��̾� ȸ�� ����

    void Basic_Dodge()
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
    }

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
}
