using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public RectTransform uiElement;
    public float moveDuration = 1.5f;
    public GameObject pauseMenuCanvas;
    public GameObject pauseButton;

    private void Start()
    {
        uiElement.anchoredPosition = new Vector2(0, Screen.height);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager_JS.Instance.GameIsPaused) { Resume(); }
            else { StartCoroutine(Pausing()); } }
    }

    public void Resume()
    {
        LeanTween.move(uiElement, new Vector2(0, Screen.height), moveDuration).setEase(LeanTweenType.easeOutExpo);
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        GameManager_JS.Instance.GameIsPaused = false;
    }
    public void ButtonPausingOnClickEvent()
    {
        StartCoroutine(Pausing());
    }

    IEnumerator Pausing()
    {
        LeanTween.move(uiElement, new Vector2(0, 0), moveDuration).setEase(LeanTweenType.easeOutExpo);
        pauseButton.SetActive(false);
        yield return new WaitForSeconds(1.2f);
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