using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    Attack attack;
    public GameObject player;
    public GameObject Arrow;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (player != null)
        {
            var bullet = ObjectPoolManager.instance.Pool.Get();

            // 플레이어의 회전을 수정하지 않고 저장
            Quaternion playerRotation = player.transform.rotation;

            // 플레이어의 Y 회전을 사용하여 화살의 회전을 설정
            bullet.transform.rotation = Quaternion.Euler(90f, playerRotation.eulerAngles.y, 0f);
            bullet.transform.position = gameObject.transform.position;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            var bullet = ObjectPoolManager.instance.Pool.Get();
        }
    }
}
