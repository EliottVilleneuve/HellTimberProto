using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWaveSpawner : MonoBehaviour
{
    public float timeBeforeFirstWave = 5;
    public float timeBetweenWaves = 10;

    public Transform waveHolder;

    private GameObject[] waves;

    private GameObject currentWave;
    private int currentWaveIndex;

    private int numberWaves;

    private float timeBeforeNextWave;
    private int currentRoundedTime;

    private Action doAction;

    /// <summary>
    /// Send true if we switched to wave mode, false if we switched to wait mode
    /// </summary>
    public event Action<bool> OnSwitchWaveMode;
    public event Action OnWon;
    public bool IsWaveMode => doAction == DoActionWave;
    public float CurrentTimeBeforeNextWave => timeBeforeNextWave;

    public UnityEvent OnDecrementCounter;

    void Start()
    {
        int childCount = waveHolder.childCount;
        waves = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            waves[i] = waveHolder.GetChild(i).gameObject;
            waves[i].SetActive(false);
        }

        numberWaves = childCount;

        //Start
        timeBeforeNextWave = timeBeforeFirstWave;
        currentWaveIndex = 0;
        SetModeWait();
    }

    private void SetModeWait()
    {
        OnSwitchWaveMode?.Invoke(false);
        doAction = DoActionWait;
    }

    private void SetModeWave()
    {
        OnSwitchWaveMode?.Invoke(true);
        doAction = DoActionWave;
    }

    private void DoActionWait()
    {
        timeBeforeNextWave -= Time.deltaTime;

        int roundedTime = Mathf.FloorToInt(timeBeforeNextWave);
        if(currentRoundedTime != roundedTime)
        {
            currentRoundedTime = roundedTime;
            OnDecrementCounter?.Invoke();
        }

        if (Input.GetButtonDown("Skip")) timeBeforeNextWave = 0;

        if (timeBeforeNextWave > 0) return;

        currentWave?.SetActive(false);
        currentWave = waves[currentWaveIndex];
        currentWave.SetActive(true);

        SetModeWave();
    }

    private void DoActionWave()
    {
        if (currentWave == null) return;
        if (currentWave.transform.childCount > 0) return;

        timeBeforeNextWave = timeBetweenWaves;
        currentWaveIndex++;

        if (currentWaveIndex >= numberWaves)
        {
            OnWon?.Invoke();
            Destroy(this);
            return;
        }

        SetModeWait();
    }

    void Update()
    {
        doAction();
    }
}
