using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator PlayerAnimator;
    // Renderer PlayerColor; 디버깅 지표용
    public float PlayerSpeed;

    [SerializeField] float hAxis;
    [SerializeField] float vAxis;

    [SerializeField] float Basic_Dodge_CoolDown;
    [SerializeField] float Basic_Dodge_CoolTime = 2.0f;
    [SerializeField] float Basic_Dodge_Time = 0.3f;
    [SerializeField] bool isDodge;
    [SerializeField] Vector3 moveVec; // 디버그용
    [SerializeField] Vector3 DodgeVec; // 디버그용

    void Basic_Move() { // 플레이어 무브먼트

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(isDodge)
            moveVec = DodgeVec; // 회피 도중에 방향 바꾸지 못하게 픽스

        transform.position += moveVec * PlayerSpeed * Time.deltaTime;

        // 여기까지 플레이어 기본 움직임

        transform.LookAt(transform.position + moveVec); 
        
        // 플레이어가 바라보는 방향으로 Rotation 변환.    

        if( hAxis != 0 || vAxis != 0 ) {

            // Debug.Log(transform.eulerAngles.y + 45); // 각도 확인용 디버그로그
            PlayerAnimator.SetTrigger("Run");
            // transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.eulerAngles.y + 45, transform.rotation.z));
        }

        // 여기까지 뛰는 애니메이션 관련

        if( hAxis == 0 && vAxis == 0 )      {
            PlayerAnimator.SetTrigger("Idle");          
        }  
         
    }

    void Basic_Dodge() {
        if(moveVec != Vector3.zero && Basic_Dodge_CoolDown > Basic_Dodge_CoolTime) {

            DodgeVec = moveVec;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.eulerAngles.y - 90, transform.rotation.z));
            PlayerAnimator.SetTrigger("Basic Dodge"); 
            PlayerSpeed *= 4;
            isDodge = true;

            Debug.Log("플레이어 기본 회피");
            // PlayerColor.material.color = Color.red; 
            Invoke("Basic_Dodge_Out", Basic_Dodge_Time);
        }
    }

    void Basic_Dodge_Out() {
        PlayerSpeed *= 0.25f;
        isDodge = false;
        Basic_Dodge_CoolDown = 0;
        // Basic_Dodge_CoolDown = 0f;
        // PlayerColor.material.color = Color.white;
    }

    void Basic_Dodge_Cooltime_Management()
    {
        if(isDodge == false)
            Basic_Dodge_CoolDown += Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        // PlayerColor = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
       Basic_Move();
       if (Input.GetKeyDown(KeyCode.Space)) { Basic_Dodge(); }
       Basic_Dodge_Cooltime_Management();
    }
}
