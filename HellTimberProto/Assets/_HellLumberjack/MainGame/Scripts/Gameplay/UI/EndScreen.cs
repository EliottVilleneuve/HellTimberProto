using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HellLumber;

public class EndScreen : MonoBehaviour
{
    public EnemyWaveSpawner enemyWaveSpawner;
    public AvatarHealth avatarHealth;
    public GameObject content;

    public Button restartButton;
    public Button backMenuButton;

    void Start()
    {
        content.SetActive(false);

        if (enemyWaveSpawner != null) enemyWaveSpawner.OnWon += OnEnd;
        else avatarHealth.OnGameOver += OnEnd;

        restartButton.onClick.AddListener(OnRestartButton);
        backMenuButton.onClick.AddListener(OnBackMenu);
    }

    private void OnBackMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnd()
    {
        content.SetActive(true);
    }
}
