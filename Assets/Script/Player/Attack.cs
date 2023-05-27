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

    public Vector3 MouseDirection { get; private set; }

    [SerializeField] GameObject AttackRange_Demo;

    private void Start()
    {
        PlayerRigid = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {

    }

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

    public IEnumerator AttackDelay()
    {
        player.Player_MoveSpeed_Reclaimer();
        // playercontroller.isAttack = true;
        yield return new WaitForSeconds(1.0f);
        // playercontroller.isAttack = false;
        player.Player_MoveSpeed_Multiplier();
    }

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

    void AttackStart()
    {
        PlayerAnimator.applyRootMotion = true;
        player.AttackManagement_Start();
        AttackRange_Demo.SetActive(true);
        Debug.Log("Start");
    }

    void AttackEnd()
    {
        PlayerAnimator.applyRootMotion = false;
        player.AttackManagement_End();
        AttackRange_Demo.SetActive(false);
        Debug.Log("End");
    }

}
