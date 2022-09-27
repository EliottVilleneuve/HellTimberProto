using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public Button startButton;

    public GameObject[] objectsToActivate;
    public MonoBehaviour[] componentsToActivate;

    // Start is called before the first frame update
    void Start()
    {
        SetActivation(false);
        startButton.onClick.AddListener(OnStartButton);
    }

    private void OnStartButton()
    {
        gameObject.SetActive(false);
        SetActivation(true);
    }

    private void SetActivation(bool active)
    {
        for (int i = 0; i < objectsToActivate.Length; i++)  objectsToActivate[i].SetActive(active);
        for (int i = 0; i < componentsToActivate.Length; i++)  componentsToActivate[i].enabled = active;
    }
}
