using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HellLumber
{
    public class AvatarTarget : MonoBehaviour
    {
        public EntityHealth entityHealth;
        public int priorityOverDistance = 0;

        public bool Destroyed => destroyed;

        private bool destroyed = false;

        private void Start()
        {
            destroyed = false;
            entityHealth.OnHurt += EntityHealth_OnHurt;
        }

        private void OnDestroy()
        {
            entityHealth.OnHurt -= EntityHealth_OnHurt;
        }

        private void EntityHealth_OnHurt()
        {
            if(entityHealth.HealthEmpty)
            {
               destroyed = true;
            }
        }
    }
}

