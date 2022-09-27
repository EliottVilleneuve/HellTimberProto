using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCountDown : MonoBehaviour
{
    public TextMeshProUGUI timerTMP;
    public GameObject waitModeContainer;

    public EnemyWaveSpawner enemyWaveSpawner;

    void Start()
    {
        enemyWaveSpawner.OnSwitchWaveMode += EnemyWaveSpawner_OnSwitchWaveMode;
        SetActivation(enemyWaveSpawner.IsWaveMode);
    }

    private void EnemyWaveSpawner_OnSwitchWaveMode(bool waveMode)
    {
        SetActivation(waveMode);
    }

    private void SetActivation(bool waveMode)
    {
        waitModeContainer.SetActive(!waveMode);
    }

    void Update()
    {
        timerTMP.text = "" + Mathf.CeilToInt(enemyWaveSpawner.CurrentTimeBeforeNextWave);
    }
}
