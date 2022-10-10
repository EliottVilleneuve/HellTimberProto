using HellLumber;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ParryType { CircularSwing, Space }
public class AvatarParry : MonoBehaviour {

    public AvatarSwing avatarSwing;
    public AvatarAim avatarAim;
    public ParryType parryType;
    public UnityEvent OnParryBullet;

    private List<Bullet> bulletsInRange;

    private void Start()
    {
        avatarSwing.OnCircularSwing.AddListener(AvatarSwing_OnCircularSwing);
        bulletsInRange = new List<Bullet>();
    }

    private void AvatarSwing_OnCircularSwing()
    {
        if(parryType == ParryType.CircularSwing) Parry();
    }

    private void Update () {
        if (ParryInput())
        {
            Parry();
        }
    }

    private void Parry()
    {
        bool parriedOne = false;

        int count = bulletsInRange.Count;
        for (int i = 0; i < count; i++)
        {
            if (bulletsInRange[i] == null) continue;
            if(bulletsInRange[i].Parry(transform, avatarAim.Direction)) parriedOne = true;
        }
        bulletsInRange.Clear();

        if (parriedOne) OnParryBullet?.Invoke();
    }

    private bool ParryInput()
    {
        if (parryType == ParryType.Space) return Input.GetKeyDown(KeyCode.Space);
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(HurtByBullet.BULLET_TAG)) return;

        if (other.TryGetComponent(out Bullet bullet))
        {
            bulletsInRange.Add(bullet);
        } 
        else U.L("No bullet found!");
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(HurtByBullet.BULLET_TAG)) return;

        if (other.TryGetComponent(out Bullet bullet))
        {
            bulletsInRange.Remove(bullet);
        }
        else U.L("No bullet found!");
    }
}
