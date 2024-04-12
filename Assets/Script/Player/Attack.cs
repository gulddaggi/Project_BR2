using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{

    public enum Weapon { Sword, Axe, Bow, Shuriken };
    public enum AttackState { Idle, FirstAttack, SecondAttack, ThirdAttack };
    public AttackState currentAttackState = AttackState.Idle;

    public Weapon PlayerWeapon = Weapon.Axe;

    // 차후에 추상 클래스로 개조 필요.
    protected Player player;
    PlayerController playerController;

    public Animator PlayerAnimator;
    public Rigidbody PlayerRigid;
    public Transform bulletSpawnPoint;

    public int buttonPressedCount;

    [SerializeField] bool AttackAvailable = true;
    // [SerializeField] float AttackDelay = 1.5f;
    [SerializeField] public bool isAttack = false;

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
    }

    public Vector3 MouseDirection { get; private set; }

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
    public GameObject shurikenPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 40f;

    private void Start()
    {
        PlayerRigid = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        playerController = GetComponent<PlayerController>();
        GameManager_JS.Instance.GetGuage();
    }

    #region * 마우스 위치 받아오기 및 레이캐스팅
    protected Vector3 GetMouseWorldPosition() // 마우스 위치 받아오기
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        // Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 5f); // 레이캐스트 비주얼 디버깅

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
        if (context.performed && Time.timeScale != 0 && SceneManager.GetActiveScene().name != "HomeScene")
        {
            MouseDirection = GetMouseWorldPosition();
            transform.LookAt(MouseDirection);

            if (AttackAvailable)
            {
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
                PlayerAnimator.SetTrigger("OnCloseAttackCombo");
                ProcessBufferedInput();
            }
        }
    }

    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Special!");
        if (context.performed && GameManager_JS.Instance.attackGuage.isSpecialReady && SceneManager.GetActiveScene().name != "HomeScene")
        {
            Debug.Log("Special Attack");

            // 여기서는 근거리 무기의 Special Attack 판정!
            // 원거리 공격은 ProjectileManagement()에서 따로 판정함! 
            if (GameManager_JS.Instance.PlayerWeaponCheck() == 0 || GameManager_JS.Instance.PlayerWeaponCheck() == 1)
            {

                var instance = Instantiate(specialAttack[GameManager_JS.Instance.PlayerWeaponCheck()].SpecialAttackPrefab, gameObject.transform.position, gameObject.transform.rotation);
                if (specialAttack[GameManager_JS.Instance.PlayerWeaponCheck()].UseLocalPosition)
                {
                    instance.transform.parent = gameObject.transform;
                    instance.transform.localPosition = Vector3.zero;
                    instance.transform.localRotation = new Quaternion();
                }
                Destroy(instance, specialAttack[GameManager_JS.Instance.PlayerWeaponCheck()].DestroyAfter);

                GameManager_JS.Instance.attackGuage.isSpecialReady = false;
                GameManager_JS.Instance.InitGuage();
                StartCoroutine(SpecialAttackRange());
            }
            else
            {
                ProjectileManagement_SpecialAttack(GameManager_JS.Instance.PlayerWeaponCheck());
            }
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

    IEnumerator SpecialAttackRange()
    {
        yield return new WaitForSeconds(specialAttack[GameManager_JS.Instance.PlayerWeaponCheck()].SpecialAttackRangeInitTime);
        specialAttack[GameManager_JS.Instance.PlayerWeaponCheck()].SpecialAttackRange.SetActive(true);
        yield return new WaitForSeconds(specialAttack[GameManager_JS.Instance.PlayerWeaponCheck()].SpecialAttackRangeDisableTime);
        specialAttack[GameManager_JS.Instance.PlayerWeaponCheck()].SpecialAttackRange.SetActive(false);
    }

    void SpecialAttack_Reclaim()
    {
        specialAttack[GameManager_JS.Instance.PlayerWeaponCheck()].SpecialAttackRange.SetActive(false);
    }

    #region * Combo Attack Manage

    void FirstAttack_Sword_Start()
    {
        // PlayerAnimator.applyRootMotion = true;
        ManageAttackRange(0, true);
        TransitionToState(AttackState.FirstAttack);
        ProcessBufferedInput();
        // Debug.Log("First Combo Start");
    }
    void FirstAttack_Sword_End()
    {
        // PlayerAnimator.applyRootMotion = false;
        // player.AttackManagement_Start();
        ManageAttackRange(0, false);

        StartCoroutine(ManageAttackDelay());
        OnComboEnd();

        inputBuffer.Clear();
        Debug.Log("First Combo End");
    }
    void SecondAttack_Sword_Start()
    {
        // PlayerAnimator.applyRootMotion = true;
        ManageAttackRange(0, false);
        ManageAttackRange(1, true);

        TransitionToState(AttackState.SecondAttack);
        ProcessBufferedInput();

        buttonPressedCount = 0;

        // Debug.Log("Second Combo Start");
    }
    void SecondAttack_Sword_End()
    {
        PlayerAnimator.applyRootMotion = false;

        ManageAttackRange(1, false);

        StartCoroutine(ManageAttackDelay());
        OnComboEnd();

        inputBuffer.Clear();
        Debug.Log("Second Combo End");
    }
    void ThirdAttack_Sword_Start()
    {
        PlayerAnimator.applyRootMotion = true;

        ManageAttackRange(1, false);
        ManageAttackRange(2, true);

        TransitionToState(AttackState.ThirdAttack);
        ProcessBufferedInput();

        buttonPressedCount = 0;

        // Debug.Log("Third Combo Start");
    }
    void ThirdAttack_Sword_End()
    {
        PlayerAnimator.applyRootMotion = false;
        ManageAttackRange(2, false);

        OnComboEnd();
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
            arrow.SetActive(true);
            arrow.GetComponent<PlayerProjectile>().player = player;
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
            GameObject shuriken = Instantiate(shurikenPrefab, arrowSpawnPoint.position, Quaternion.identity);
            shuriken.SetActive(true);
            shuriken.GetComponent<PlayerProjectile>().player = player;
            AttackAvailable = false;
            Vector3 shootDirection = (targetPosition - arrowSpawnPoint.position).normalized;

            // 화살 로테이션 / Y좌표 보정
            Quaternion arrowRotation = Quaternion.LookRotation(Vector3.up, shootDirection);
            shuriken.transform.rotation = arrowRotation;
            shuriken.transform.position = new Vector3(shuriken.transform.position.x, 2f, shuriken.transform.position.z);

            shuriken.GetComponent<Rigidbody>().AddForce(shootDirection * arrowSpeed, ForceMode.Impulse);
        }
    }

    void ProjectileManagement_SpecialAttack(int PlayerWeapon)
    {
        MouseDirection = GetMouseWorldPosition();
        transform.LookAt(MouseDirection);

        GameManager_JS.Instance.attackGuage.isSpecialReady = false;
        GameManager_JS.Instance.InitGuage();
        StartCoroutine(SpecialAttackRange());

        if (PlayerWeapon == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
                arrow.SetActive(true);
                arrow.GetComponent<PlayerProjectile>().player = player;

                // 각도 조절
                float angle = (i - 1) * 30f; // -30, 0, 30
                Vector3 shootDirection = Quaternion.Euler(0, angle, 0) * (MouseDirection - arrowSpawnPoint.position).normalized;

                Quaternion arrowRotation = Quaternion.LookRotation(Vector3.up, shootDirection);
                arrow.transform.rotation = arrowRotation;
                arrow.transform.position = new Vector3(arrow.transform.position.x, 2f, arrow.transform.position.z);

                arrow.GetComponent<Rigidbody>().AddForce(shootDirection * arrowSpeed, ForceMode.Impulse);
            }
        }

        if (PlayerWeapon == 3)
        {
            /*
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(SpawnShurikenDelayed(MouseDirection, i * 0.2f));
            }
            AttackAvailable = true;
            */

            // Enemy 리팩토링으로 인한 임시 코드

            for (int i = 0; i < 9; i++)
            {
                GameObject shuriken = Instantiate(shurikenPrefab, arrowSpawnPoint.position, Quaternion.identity);
                shuriken.SetActive(true);
                shuriken.GetComponent<PlayerProjectile>().player = player;

                // 각도 조절
                float angle = (i - 1) * 40f;
                Vector3 shootDirection = Quaternion.Euler(0, angle, 0) * (MouseDirection - arrowSpawnPoint.position).normalized;

                Quaternion arrowRotation = Quaternion.LookRotation(Vector3.up, shootDirection);
                shuriken.transform.rotation = arrowRotation;
                shuriken.transform.position = new Vector3(shuriken.transform.position.x, 2f, shuriken.transform.position.z);

                shuriken.GetComponent<Rigidbody>().AddForce(shootDirection * arrowSpeed, ForceMode.Impulse);
            }
        }
    }

    IEnumerator SpawnShurikenDelayed(Vector3 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject shuriken = Instantiate(shurikenPrefab, arrowSpawnPoint.position, Quaternion.identity);

        Vector3 shootDirection = (targetPosition - arrowSpawnPoint.position).normalized;

        // 수리검 로테이션 / Y좌표 보정
        Quaternion shurikenRotation = Quaternion.LookRotation(Vector3.up, shootDirection);
        shuriken.transform.rotation = shurikenRotation;
        shuriken.transform.position = new Vector3(shuriken.transform.position.x, 2f, shuriken.transform.position.z);

        shuriken.GetComponent<Rigidbody>().AddForce(shootDirection * arrowSpeed, ForceMode.Impulse);
    }

}
