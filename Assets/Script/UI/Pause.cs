using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject pauseButton;

    private void Start()
    {
        pauseMenuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager_JS.Instance.GameIsPaused) { Resume(); }
            else { Pausing(); } }
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        GameManager_JS.Instance.GameIsPaused = false;
    }

    public void Pausing()
    {
        pauseMenuCanvas.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        GameManager_JS.Instance.GameIsPaused = true;
    }

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }

    public void Config()
    {
        Debug.Log("미구현");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}