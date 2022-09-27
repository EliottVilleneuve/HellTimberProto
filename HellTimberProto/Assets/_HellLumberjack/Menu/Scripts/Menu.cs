using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Button playButton;
    public Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButton);
        quitButton.onClick.AddListener(OnQuitButton);
    }

    private void OnQuitButton()
    {
        Application.Quit();
    }

    private void OnPlayButton()
    {
        SceneManager.LoadScene("HellLumberjack");
    }
}
