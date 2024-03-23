using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string sceneName; // 인스펙터에서 설정할 씬 이름
    public void ChangeScene()
    {
        SceneLoader.LoadScene(sceneName); 
    }
}