using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public EnemyWaveSpawner enemyWaveSpawner;
    public GameObject content;

    public Button restartButton;
    public Button backMenuButton;

    void Start()
    {
        content.SetActive(false);

        enemyWaveSpawner.OnWon += EnemyWaveSpawner_OnWon;

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

    private void EnemyWaveSpawner_OnWon()
    {
        content.SetActive(true);
    }
}
