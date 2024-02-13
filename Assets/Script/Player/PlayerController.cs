using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Rigidbody PlayerRigid;
    protected Player player;
    Attack attack;

    public int DodgeButtonPressedCount;

    [SerializeField] private Vector3 PlayerMoveDirection;

    // 필드 공격 발생 이벤트.
    public UnityEvent OnPlayerFieldAttack;

    // 돌진 공격 판정 오브젝트
    [SerializeField]
    public GameObject dodgeAttackObj;

    [SerializeField] public bool CanMove = true;

    protected Vector3 DodgeVec;
    [SerializeField] float Basic_Dodge_CoolDown;
    [SerializeField] float Basic_Dodge_CoolTime = 2.0f;
    [SerializeField] float Basic_Dodge_Time = 0.3f;
    [SerializeField] bool isDodge;
    [SerializeField] public bool isAttack;

    public bool isAttacked = false;

    // 직렬화문은 디버깅 용도. 실제 릴리스시에는 제거해야 함

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        PlayerRigid = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
        attack = GetComponent<Attack>();
    }


    // !! Player Input은 Invoke Unity Events로 구성해놓았음 !!
    // Send Messages 방식은 차후에 헷갈릴 가능성 UP 

    #region * Player Input Contextive Functions

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (CanMove)
        {
            Vector2 input = context.ReadValue<Vector2>();
            PlayerMoveDirection = new Vector3(input.x, 0f, input.y);
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed) // 닷지 키가 눌렸는지 체크. 여기서부터 닷지 로직 작성
        {
            if (PlayerRigid.velocity != Vector3.zero && Basic_Dodge_CoolDown > Basic_Dodge_CoolTime && PlayerAnimator.GetInteger("DodgeButtonPressedCount") == 0)
            {

                PlayerAnimator.SetInteger("DodgeButtonPressedCount", 1);
                DodgeVec = PlayerMoveDirection;

                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.eulerAngles.y - 90, transform.rotation.z));

                PlayerAnimator.SetTrigger("Basic Dodge");
                player.Player_MoveSpeed_Multiplier();

                isDodge = true;
                Debug.Log("플레이어 기본 회피");

                // PlayerColor.material.color = Color.red; 디버그용
                
                // 돌진 데미지 판정 오브젝트 활성화
                dodgeAttackObj.SetActive(true);

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
            if (attack.isAttack == false)
            {
                // 속력벡터가 0이 아닐 시
                PlayerAnimator.SetTrigger("Run");
            }
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
        // damageField.gameObject.SetActive(false);

        // 플레이어 필드 공격 이벤트 발생.
        OnPlayerFieldAttack.Invoke();

        isDodge = false;
        Basic_Dodge_CoolDown = 0;
        PlayerAnimator.SetInteger("DodgeButtonPressedCount", 0);
    }

    void Basic_Dodge_Cooltime_Management()
    {
        if (isDodge == false)
            Basic_Dodge_CoolDown += Time.deltaTime;
    }

    #endregion



    // 여기서부터는 플레이어 충돌처리

    #region * Player Collision Check

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var damagedata = collision.gameObject.GetComponent<Enemy>(); // 데모용이라 Ghoul.cs로 한 것임! 차후에 Enemy 추상화 이루어지면 수정해야 함!
            player.TakeDamage(damagedata.Damage);

        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttack" && !isAttacked)
        {
            isAttacked = true;
            Enemy enemy;

            if (other.name == "bullet 1")
            {
                GrogonBullet bullet = other.gameObject.GetComponentInParent<GrogonBullet>();
                enemy = bullet.enemy;
                player.TakeDamage((float)bullet.damage);
                Debug.Log("Player Damaged : " + bullet.damage);
            }
            else
            {
                enemy = other.gameObject.GetComponentInParent<Enemy>();
                player.TakeDamage(enemy.Damage);
                Debug.Log("Player Damaged : " + enemy.Damage);
            }

            float value = 0.0f;
            bool isCounterAttackOn = false;

            // 적 공격에 대한 카운터 어택 처리
            // 물 타입 카운터 어택
            if (player.PlayerCounterAbilityDamage != 0f)
            {
                value += player.PlayerCounterAbilityDamage;
                isCounterAttackOn = true;
            }
            // 불 타입 카운터 어택
            if (player.PlayerCounterIgnitionDamage != 0f)
            {
                isCounterAttackOn = true;
                value += 0;
            }

            if (isCounterAttackOn)
            {
                isCounterAttackOn = false;
                enemy.CounterAttacked(value, player);
            }

            isAttacked = false;
        }
    }

    #endregion

}
