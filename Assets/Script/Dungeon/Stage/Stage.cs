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
        playerPos.position = startPoint.position;
    }

    public Transform GetPlayerPos()
    {
        return playerPos;
    }

    public void SetPlayerPos(Transform pos)
    {
        playerPos = pos;
        playerPos.position = startPoint.position; // 스테이지 시작 시 플레이어 위치 지정
    }
}
