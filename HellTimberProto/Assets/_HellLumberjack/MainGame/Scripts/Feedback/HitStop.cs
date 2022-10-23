using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float timeScale = 0.2f;
    public float duration = 0.5f;

    private Coroutine currentCoroutine;

    public void TriggerHitStop()
    {
        currentCoroutine = StartCoroutine(HitStopCoroutine());
    }

    private IEnumerator HitStopCoroutine()
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        currentCoroutine = null;
    }

    private void OnDestroy()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        Time.timeScale = 1;
    }
}
