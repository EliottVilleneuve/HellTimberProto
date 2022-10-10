using HellLumber;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AvatarParry : MonoBehaviour {

    private List<Bullet> bulletsInRange;

    public UnityEvent OnParryBullet;

    private void Start () {
        bulletsInRange = new List<Bullet>();
    }

    private void Update () {
        if (ParryInput())
        {
            int count = bulletsInRange.Count;
            for (int i = 0; i < count; i++)
            {
                if(bulletsInRange[i] == null) continue;
                bulletsInRange[i].Parry(transform);
            }
            bulletsInRange.Clear();

            if (count > 0) OnParryBullet?.Invoke();
        }
    }

    private bool ParryInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(HurtByBullet.BULLET_TAG)) return;

        U.L("bullet enter !");
        if (other.TryGetComponent(out Bullet bullet))
        {
            bulletsInRange.Add(bullet);
        } 
        else U.L("No bullet found!");
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(HurtByBullet.BULLET_TAG)) return;

        U.L("bullet exit !");
        if (other.TryGetComponent(out Bullet bullet))
        {
            bulletsInRange.Remove(bullet);
        }
        else U.L("No bullet found!");
    }
}
