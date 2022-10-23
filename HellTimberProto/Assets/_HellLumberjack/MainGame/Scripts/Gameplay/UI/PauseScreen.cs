using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject content;

    public Button resumeButton;
    public Button restartButton;
    public Button backMenuButton;

    private bool active = false;

    void Start()
    {
        SetActivation(false);

        resumeButton.onClick.AddListener(OnResumeButton);
        restartButton.onClick.AddListener(OnRestartButton);
        backMenuButton.onClick.AddListener(OnBackMenu);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Options"))
        {
            SetActivation(!active);
        }
    }

    private void SetActivation(bool active)
    {
        this.active = active;
        content.SetActive(active);
        Time.timeScale = active ? 0 : 1;
    }
    private void OnResumeButton()
    {
        SetActivation(false);
    }

    private void OnBackMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
