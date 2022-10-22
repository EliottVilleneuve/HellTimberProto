
using System.Collections;
using UnityEngine;
using TMPro;
using System;
using HellLumber;

public class AvatarCloseBullet : MonoBehaviour {
    private const string BULLET_TAG = "Bullet";

    public Renderer renderer;
    public float flickerDuration = 0.1f;

    public TextMeshProUGUI scoreText;
    public int scoreGainedPerContact = 100;

    public AvatarSwing avatarSwing;
    public AvatarHealth avatarHealth;
    public int damageBoostPerContact = 10;

    private int score = 0;

    private void Start () {
        renderer.enabled = false;
        avatarHealth.OnHurt += AvatarHealth_OnHurt;    
        SetScore(0);
    }

    private void AvatarHealth_OnHurt()
    {
        avatarSwing.ResetDamageBoost();
    }

    private void SetScore(int value)
    {
        score = value;
        scoreText.text = "" + score;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(BULLET_TAG)) return;

        StartCoroutine(CloseBullet());
    }

    private IEnumerator CloseBullet()
    {
        renderer.enabled = true;
        SetScore(score + scoreGainedPerContact);
        avatarSwing.AddDamageBoost(damageBoostPerContact);
        U.L("close !");
        yield return new WaitForSeconds(flickerDuration);
        renderer.enabled = false;
    }
}
