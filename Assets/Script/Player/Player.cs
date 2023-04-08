using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Renderer PlayerColor;
    public float PlayerSpeed;

    float hAxis;
    float vAxis;

    [SerializeField] float Basic_Dodge_CoolDown;
    [SerializeField] float Basic_Dodge_CoolTime = 2.0f;
    [SerializeField] float Basic_Dodge_Time = 0.3f;
    [SerializeField] bool isDodge;
    [SerializeField] Vector3 moveVec; // 디버그용
    [SerializeField] Vector3 DodgeVec; // 디버그용

    void Basic_Move() { // 플레이어 기본 움직임
       hAxis = Input.GetAxisRaw("Horizontal");
       vAxis = Input.GetAxisRaw("Vertical");

       moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(isDodge)
            moveVec = DodgeVec;

       transform.position += moveVec * PlayerSpeed * Time.deltaTime;
       
        transform.LookAt(transform.position + moveVec); // 플레이어가 바라보는 방향으로 Rotation 변환               
    }

    void Basic_Dodge() {
        if(moveVec != Vector3.zero && Basic_Dodge_CoolDown > Basic_Dodge_CoolTime) {

            DodgeVec = moveVec;

            PlayerSpeed *= 4;
            isDodge = true;

            Debug.Log("플레이어 기본 회피");
            PlayerColor.material.color = Color.red;

            Invoke("Basic_Dodge_Out", Basic_Dodge_Time);
        }
    }

    void Basic_Dodge_Out() {
        PlayerSpeed *= 0.25f;
        isDodge = false;
        Basic_Dodge_CoolDown = 0f;
        PlayerColor.material.color = Color.white;
    }

    void Basic_Dodge_Cooltime_Management()
    {
        if(isDodge == false)
            Basic_Dodge_CoolDown += Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerColor = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
       Basic_Move();
       if (Input.GetKeyDown(KeyCode.Space)) { Basic_Dodge(); }
       Basic_Dodge_Cooltime_Management();
    }
}
