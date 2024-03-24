using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string[] sceneNameArray = { "DungeonScene_JSTest", "HomeScene" }; // 인스펙터에서 설정할 씬 이름
    public void ChangeScene(int _sceneNum)
    {
        SceneManager.LoadScene(sceneNameArray[_sceneNum]); // 씬을 로드하는 함수 호출
    }
}