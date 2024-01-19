using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{

    public enum Weapon { Sword, Axe, Bow };
    public Weapon PlayerWeapon = Weapon.Axe;

    // 차후에 추상 클래스로 개조 필요.



    protected Player player;
    PlayerController playercontroller;

    public Animator PlayerAnimator;
    public Rigidbody PlayerRigid;
    public Transform bulletSpawnPoint;

    [SerializeField] bool AttackAvailable = true;
<<<<<<< Updated upstream
    // [SerializeField] float AttackDelay = 1.5f;
=======

    [SerializeField] public bool isAttack = false;
>>>>>>> Stashed changes

    [Header("플레이어 공격 딜레이 / 무기마다 체크")]
    public WeaponAttackDelay PlayerAttackDelay;
    [System.Serializable]
    public class WeaponAttackDelay
    {
        public float[] AttackDelay;
    }

    [Header("하기 요소들은 플레이어의 공격 범위를 지정함.")]
    [Header("공격 범위를 수정하려면 해당 요소들을 바꿔끼우면 됨.")]
    [Header("플레이어 무기 태그 번호는 AnimationEventEffect.cs를 참조할 것.")]

    public DamameRange[] Weapon_Damage_Range;
    [System.Serializable]
    public class DamameRange
    {
        public GameObject[] WeaponDamageRange;
        // public Transform[] RangeInstantiatePosition;
        // public float[] DestroyAfter;
        // public bool[] UseLocalPosition;
    }

    public Vector3 MouseDirection { get; private set; }

<<<<<<< Updated upstream
=======
    #region * 입력 버퍼 관련 변수

    [Header("입력 버퍼")]
    public int inputBufferSize = 2;
    private Queue<AttackState> inputBuffer = new Queue<AttackState>();
    public int BufferCount;

    private struct InputEvent
    {
        public float timestamp;
        public Vector3 direction;
    }
    #endregion

    public SpecialAttack[] specialAttack;
    [System.Serializable]
    public class SpecialAttack
    {
        public GameObject SpecialAttackPrefab;
        public GameObject SpecialAttackRange;

        public float DestroyAfter;
        public bool UseLocalPosition;

        public float SpecialAttackRangeInitTime;
        public float SpecialAttackRangeDisableTime;
    }

    [Header("여기서부터는 원거리 공격(활, 수리검)을 관리")]
    public GameObject arrowPrefab; 
    public Transform arrowSpawnPoint; 
    public float arrowSpeed = 40f;

>>>>>>> Stashed changes
    private void Start()
    {
        PlayerRigid = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
<<<<<<< Updated upstream
=======
        playerController = GetComponent<PlayerController>();

        GameManager_JS.Instance.GetGuage();
>>>>>>> Stashed changes
    }

    #region * 마우스 위치 받아오기 및 레이캐스팅
    protected Vector3 GetMouseWorldPosition() // 마우스 위치 받아오기
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 5f); // 레이케스트 비주얼 디버깅

        if (Physics.Raycast(ray, out RaycastHit HitInfo, Mathf.Infinity))
        {
            Vector3 target = HitInfo.point;
            Vector3 myPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            target.Set(target.x, 0f, target.z);
            return 100 * (target - myPosition).normalized; // 정규화
        }

        return Vector3.zero;
    }
    #endregion

    #region * New Input System Invoke Events 관련 코드. !!잘못 건들면 인풋시스템 다 망가짐!!
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && Time.timeScale != 0 && SceneManager.GetActiveScene().name != "HomeScene") // 공격키가 눌렸는지 체크
        {
            MouseDirection = GetMouseWorldPosition();
            transform.LookAt(MouseDirection);
            // Debug.Log(MouseDirection);

            if (AttackAvailable)
            {
<<<<<<< Updated upstream
=======
                isAttack = true;
                buttonPressedCount++;
                PlayerAnimator.SetInteger("ButtonPressedCount", buttonPressedCount);
                if (currentAttackState == AttackState.Idle && buttonPressedCount < 3)
                {
                    AddComboInput(AttackState.FirstAttack);
                    // 활 / 수리검 관련
                    if (GameManager_JS.Instance.PlayerWeaponCheck() == 2 || GameManager_JS.Instance.PlayerWeaponCheck() == 3)
                    {
                        ProjectileManagement(MouseDirection);
                    }
                }
>>>>>>> Stashed changes
                PlayerAnimator.SetTrigger("OnCloseAttackCombo");
            }

            // StartCoroutine(AttackDelay());
        }
    }
    #endregion

    #region * 극초기 스냅샷 당시 공격 딜레이 관련 레거시 코드.
    /* 
    public IEnumerator AttackDelay()
    {
        player.Player_MoveSpeed_Reclaimer();
        // playercontroller.isAttack = true;
        yield return new WaitForSeconds(1.0f);
        // playercontroller.isAttack = false;
        player.Player_MoveSpeed_Multiplier();
    }
    */
    #endregion

    #region * 콤보 공격 레거시 코드. 코루틴헬퍼 지원 종료에 따라 사용불가

    /* 
     
    void IntCheck()
    {
        // 콤보 공격이 계속 되는지 체크하는 boolean 변수
        isContinueComboAttack = false;

        // 콤보 어택이 이어지지 않는다면, 코루틴을 종료하기 위해 IEnumerator 변수 사용
        COR_CheckComboAttack = CheckComboAttack();

        StartCoroutine(COR_CheckComboAttack);

        // === Local Function ===
        IEnumerator CheckComboAttack()
        {
            // 공격 버튼이 눌렸는지 체크
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            // 눌렸다면, boolean 변수를 True로 바꾼다.
            isContinueComboAttack = true;
        }
    }

    void IntOut()
    {
        // boolean 변수가 false 라면,
        if (!isContinueComboAttack)
            PlayerAnimator.applyRootMotion = false;
            EndAnimation();    // 애니메이션을 종료시킨다.
    }

    void EndAnimation() {
        isAttack = false;
        PlayerAnimator.SetTrigger("Idle");
        Debug.Log("Animation ENDED");
    }

    */

    #endregion

    #region * 이 코드들은 차후에 Attack.cs 추상화 작업시에 사용할 것.

    /*
    void AttackStart()
    {
        PlayerAnimator.applyRootMotion = true;
        // player.AttackManagement_Start();
        AttackRange_Demo.SetActive(true);
        Debug.Log("Start");
    }

    void AttackEnd()
    {
        PlayerAnimator.applyRootMotion = false;
        // player.AttackManagement_End();
        AttackRange_Demo.SetActive(false);
        Debug.Log("End");
    }
    */

    #endregion

    #region * Combo Attack Manage

    void FirstAttack_Sword_Start()
    {
        // PlayerAnimator.applyRootMotion = true;
        ManageAttackRange(0, true);
        // Debug.Log("First Combo Start");
    }
    void FirstAttack_Sword_End()
    {
        // PlayerAnimator.applyRootMotion = false;
        // player.AttackManagement_Start();
        ManageAttackRange(0, false);
        StartCoroutine(ManageAttackDelay());
        Debug.Log("First Combo End");
    }
    void SecondAttack_Sword_Start()
    {
        // PlayerAnimator.applyRootMotion = true;
        ManageAttackRange(0, false);
        ManageAttackRange(1, true);
        // Debug.Log("Second Combo Start");
    }
    void SecondAttack_Sword_End()
    {
        // PlayerAnimator.applyRootMotion = false;

        ManageAttackRange(1, false);
        StartCoroutine(ManageAttackDelay());
        Debug.Log("Second Combo End");
    }
    void ThirdAttack_Sword_Start()
    {
        PlayerAnimator.applyRootMotion = true;
        ManageAttackRange(1, false);
        ManageAttackRange(2, true);
        // Debug.Log("Third Combo Start");
    }
    void ThirdAttack_Sword_End()
    {
        PlayerAnimator.applyRootMotion = false;
        ManageAttackRange(2, false);
        StartCoroutine(ManageAttackDelay());
        Debug.Log("Third Combo End");
    }

    public void ManageAttackRange(int ComboNum, bool able)
    {
        // Debug.Log("Player Attack!");
        Weapon_Damage_Range[GameManager_JS.Instance.PlayerWeaponCheck()].WeaponDamageRange[ComboNum].SetActive(able);
    }

    IEnumerator ManageAttackDelay()
    {
        AttackAvailable = false;
        PlayerAnimator.SetBool("AttackAvailable", false);
        yield return new WaitForSeconds(PlayerAttackDelay.AttackDelay[GameManager_JS.Instance.PlayerWeaponCheck()]);
        PlayerAnimator.SetBool("AttackAvailable", true);
        AttackAvailable = true;
    }

    #endregion
<<<<<<< Updated upstream
=======

    void TransitionToState(AttackState nextState)
    {
        switch (nextState)
        {
            case AttackState.Idle:
                break;
            case AttackState.FirstAttack:
                // FirstAttack_Sword_Start();
                break;
            case AttackState.SecondAttack:
                // SecondAttack_Sword_Start();
                break;
            case AttackState.ThirdAttack:
                // ThirdAttack_Sword_Start();
                break;
            default:
                break;
        }

        // 현재 상태 업데이트
        currentAttackState = nextState;
    }

    void OnComboEnd()
    {
        switch (currentAttackState)
        {
            case AttackState.FirstAttack:
                TransitionToState(AttackState.Idle);
                break;
            case AttackState.SecondAttack:
                TransitionToState(AttackState.Idle);
                break;
            case AttackState.ThirdAttack:
                TransitionToState(AttackState.Idle);
                break;
            default:
                break;
        }
        buttonPressedCount = 0;
        PlayerAnimator.SetInteger("ButtonPressedCount", 0);
        isAttack = false;
    }
    private void Update()
    {
        ManageInputBuffer();
        BufferCount = inputBuffer.Count;
        // ProjectileManagement();
    }

    // 입력 버퍼 관리 함수
    private void ManageInputBuffer()
    {
        while (inputBuffer.Count > inputBufferSize)
        {
            inputBuffer.Dequeue();
        }

        if (AttackAvailable)
        {
            Vector3 mouseDirection = GetMouseWorldPosition();

        }
    }

    private void AddComboInput(AttackState attackState)
    {
        inputBuffer.Enqueue(attackState);
    }

    private bool CheckComboInput()
    {
        return Mouse.current.leftButton.isPressed;
    }

    // 입력버퍼 체크섬
    private bool CheckInputBufferValidity(int bufferIndex)
    {
        if (bufferIndex >= 0 && bufferIndex < inputBuffer.Count)
        {
            return true;
        }
        return false;
    }

    // 입력 버퍼 처리
    private void ProcessBufferedInput()
    {
        if (inputBuffer.Count > 0)
        {
            AttackState nextAttackState = inputBuffer.Dequeue();

            // 차후 콤보공격후 후처리 필요할 시 사용할 것
            switch (nextAttackState)
            {
                default:
                    break;
            }
        }
    }

    void ProjectileManagement(Vector3 targetPosition)
    {
        
            if (GameManager_JS.Instance.PlayerWeaponCheck() == 2 && AttackAvailable)
            {
                GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
                Vector3 shootDirection = (targetPosition - arrowSpawnPoint.position).normalized;

                // 화살 로테이션 / Y좌표 보정
                Quaternion arrowRotation = Quaternion.LookRotation(Vector3.up, shootDirection);
                arrow.transform.rotation = arrowRotation;
                arrow.transform.position = new Vector3(arrow.transform.position.x, 2f, arrow.transform.position.z);

                arrow.GetComponent<Rigidbody>().AddForce(shootDirection * arrowSpeed, ForceMode.Impulse);
        }
            else if (GameManager_JS.Instance.PlayerWeaponCheck() == 3 && AttackAvailable)
            {
                // 차후에 수리검 실장시 추가
            }
    }
>>>>>>> Stashed changes
}
