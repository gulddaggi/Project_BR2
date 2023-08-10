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
    public Rigidbody PlayerRigid;
    public Transform bulletSpawnPoint;

    public Vector3 MouseDirection { get; private set; }

    // [SerializeField] GameObject AttackRange_Demo; 차후 추상화 작업시에 주석 풀 것.

    #region * 주의! 해당 섹터 내 코드들은 데모용으로 작성됨. 당 변수들은 차후에 추상화 작업시에 자식 클래스로의 이동이 필요.

    [SerializeField] GameObject AttackRange_Demo_1;
    [SerializeField] GameObject AttackRange_Demo_2;
    [SerializeField] GameObject AttackRange_Demo_3;

    #endregion

    private void Start()
    {
        PlayerRigid = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
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
        if (context.performed) // 공격키가 눌렸는지 체크
        {
            MouseDirection = GetMouseWorldPosition();
            transform.LookAt(MouseDirection);
            // Debug.Log(MouseDirection);


            PlayerAnimator.SetTrigger("OnCloseAttackCombo");

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

    #region * 해당 섹터 코드들은 차후에 SwordAttack.cs로 이동 필요. 데모이므로 여기에 총체적으로 작성.

    void FirstAttack_Sword_Start()
    {
        PlayerAnimator.applyRootMotion = true;
        // player.AttackManagement_Start();
        var bulletGo = ObjectPoolManager.instance.Pool.Get();
        bulletGo.transform.position = this.bulletSpawnPoint.position;
        //Debug.Log("First Combo Start");
    }
    void FirstAttack_Sword_End()
    {
        PlayerAnimator.applyRootMotion = false;
        // player.AttackManagement_Start();
         AttackRange_Demo_1.SetActive(false);
        // Debug.Log("First Combo End");
    }
    void SecondAttack_Sword_Start()
    {
        PlayerAnimator.applyRootMotion = true;
        // player.AttackManagement_Start();
         AttackRange_Demo_1.SetActive(false);
         AttackRange_Demo_2.SetActive(true);
        // Debug.Log("Second Combo Start");
    }
    void SecondAttack_Sword_End()
    {
        PlayerAnimator.applyRootMotion = false;
        // player.AttackManagement_Start();
         AttackRange_Demo_2.SetActive(false);
        // Debug.Log("Second Combo End");
    }
    void ThirdAttack_Sword_Start()
    {
        PlayerAnimator.applyRootMotion = true;
         AttackRange_Demo_2.SetActive(false);
        // player.AttackManagement_Start();
         AttackRange_Demo_3.SetActive(true);
        // Debug.Log("Third Combo Start");
    }
    void ThirdAttack_Sword_End()
    {
        PlayerAnimator.applyRootMotion = false;
        // player.AttackManagement_Start();
         AttackRange_Demo_3.SetActive(false);
        // Debug.Log("Third Combo End");
    }

    #endregion
}
