using HellLumber;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarGrab : MonoBehaviour {

    public float pullTime = 1;
    public float stunTime = 0.5f;

    public bool holdToTrigger;
    public float minHoldTimeToTrigger;
    public float maxHoldTime;

    public bool changeSize;
    public float startSize = 0;
    public float endSize = 1;

    public AvatarMove avatarMove;
    public bool slowWhenHolding;
    public AnimationCurve speedMultiplierEvoHold;

    public Renderer feedBack;
    public AnimationCurve feedbackAlphaEvoHoldTime;
    
    private List<Enemy> enemiesInRange;
    private List<Enemy> enemiesGrabbed;
    private int numberEnemiesGrabbed;

    private float holdTime;

    private Vector3 defaultSize;

    private void Start()
    {
        enemiesInRange = new List<Enemy>();
        enemiesGrabbed = new List<Enemy>();
        feedBack.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (holdToTrigger)
            {
                holdTime = 0;
                defaultSize = transform.localScale;
                feedBack.gameObject.SetActive(true);
            }
            else Pull(); 
        }

        if (holdToTrigger)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                holdTime += Time.deltaTime;

                float progress = Mathf.Clamp01(holdTime / maxHoldTime);

                Color newColor = feedBack.material.color;
                newColor.a = feedbackAlphaEvoHoldTime.Evaluate(progress);
                feedBack.material.color = newColor;

                if (changeSize)
                {
                    Vector3 size = defaultSize;
                    size.z = Mathf.Lerp(startSize, endSize, progress);
                    transform.localScale = size;
                }

                if (slowWhenHolding)
                {
                    avatarMove.SetMultiplier(speedMultiplierEvoHold.Evaluate(progress));
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                avatarMove.SetMultiplier(1);
                feedBack.gameObject.SetActive(false);
                if (holdTime >= minHoldTimeToTrigger)
                {
                    holdTime = 0;
                    Pull();
                }
            }
        }
    }

    private void Pull()
    {
        enemiesGrabbed.Clear();

        int count = enemiesInRange.Count;
        for (int i = 0; i < count; i++)
        {
            if (enemiesInRange[i] == null) continue;

            enemiesGrabbed.Add(enemiesInRange[i]);
        }
        numberEnemiesGrabbed = enemiesGrabbed.Count;

        if(numberEnemiesGrabbed > 0) StartCoroutine(PullCoroutine());
    }

    private IEnumerator PullCoroutine()
    {
        Vector3 playerPos = transform.position;
        playerPos.y = 0;

        float[] grabbedSpeed = new float[numberEnemiesGrabbed];

        for (int i = 0; i < numberEnemiesGrabbed; i++) {
            enemiesGrabbed[i].enabled = false;

            float distance = Vector3.Distance(enemiesGrabbed[i].transform.position, playerPos);
            grabbedSpeed[i] = distance / pullTime;
        }

        float time = 0;
        while(time < 1)
        {
            time += Time.deltaTime / pullTime;

            for (int i = 0; i < numberEnemiesGrabbed; i++)
            {
                Vector3 pos = enemiesGrabbed[i].transform.position;
                pos.y = 0;
                Vector3 pullDirection = playerPos - pos;
                enemiesGrabbed[i].characterController.Move(pullDirection.normalized * grabbedSpeed[i] * Time.deltaTime);
            }
            yield return null;
        }

        yield return new WaitForSeconds(stunTime);

        for (int i = 0; i < numberEnemiesGrabbed; i++) enemiesGrabbed[i].enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemiesInRange.Add(enemy);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }
}
