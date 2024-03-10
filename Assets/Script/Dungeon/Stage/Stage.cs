using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    // 플레이어 좌표
    [SerializeField]
    Transform playerPos;

    // 시작지점 좌표
    [SerializeField]
    Transform startPoint;

    protected virtual void Start()
    {
        // 플레이어를 시작 위치로 이동
        // playerPos.position = startPoint.position; // SetPlayerPos가 존재하여 불필요.
    }

    // 현재 플레이어 위치 반환
    public Transform GetPlayerPos()
    {
        return playerPos;
    }

    // 플레이어를 시작 위치로 이동.
    public void PlayerPosToStart(Transform pos)
    {
        if (playerPos != null)
            playerPos = pos;
        else
        {
            playerPos = GetPlayerPos();
            playerPos = pos;
        }
        playerPos.position = startPoint.position; // 스테이지 시작 시 플레이어 위치 지정
    }
}
