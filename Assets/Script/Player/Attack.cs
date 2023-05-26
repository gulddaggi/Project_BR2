using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{

    // ���Ŀ� �߻� Ŭ������ ���� �ʿ�.
    protected Player player;
    [SerializeField] PlayerController playercontroller;
    public Animator PlayerAnimator;
    public float Strong_Attack_Hold_Time;
    [SerializeField] float Strong_Attack_Hold_Time_Waiting = 2.0f;

    public Vector3 MouseDirection { get; private set; }


    private void Start()
    {
        player = GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    protected Vector3 GetMouseWorldPosition() // ���콺 ��ġ �޾ƿ���
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 5f); // �����ɽ�Ʈ ���־� �����

        if (Physics.Raycast(ray, out RaycastHit HitInfo, Mathf.Infinity))
        {
            Vector3 target = HitInfo.point;
            Vector3 myPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            target.Set(target.x, 0f, target.z);
            return 100 * (target - myPosition).normalized; // ����ȭ
        }

        return Vector3.zero;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) // ����Ű�� ���ȴ��� üũ
        {
            MouseDirection = GetMouseWorldPosition();
            transform.LookAt(MouseDirection);
            Debug.Log(MouseDirection);

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
