using HellLumber;
using System;
using System.Collections;
using UnityEngine;

public enum DashType { CircularSwing, MiddleClick, Space}
public class AvatarDash : MonoBehaviour {

    public CharacterController characterController;
    public AvatarMove avatarMove;
    public AvatarAim avatarAim;
    public AvatarHealth avatarHealth;

    public AvatarSwing avatarSwing;

    public DashType dashType;

    public bool dashImmune = false;

    public float dashDuration;
    public float dashDistance;
    public float stunTime = 0.5f;

    private float remainingStunTime;

    private void Start () {
        avatarSwing.OnCircularSwing.AddListener(AvatarSwing_OnCircularSwing);
    }

    private void AvatarSwing_OnCircularSwing()
    {
        if (dashType == DashType.CircularSwing)
        {
            Dash();
        }
    }

    private void Update () {

        if(remainingStunTime > 0)
        {
            remainingStunTime -= Time.deltaTime;
            return;
        }

        if (DashInput())
        {
            Dash();
        }
    }

    private void Dash()
    {
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        remainingStunTime = stunTime;
        avatarMove.enabled = false;
        if(dashImmune) avatarHealth.SetImmunity(true);

        float dashSpeed = dashDistance / dashDuration;
        Vector3 direction = avatarAim.Direction;

        float time = 0;
        while(time < 1)
        {
            time += Time.deltaTime / dashDuration;
            characterController.Move(direction * dashSpeed * Time.deltaTime);
            
            yield return null;
        }

        avatarMove.enabled = true;
        avatarHealth.SetImmunity(false);
    }

    private bool DashInput()
    {
        if(dashType == DashType.MiddleClick) return Input.GetButtonDown("Dash");
        if(dashType == DashType.Space) return Input.GetKeyDown(KeyCode.Space);
        return false;
    }
}