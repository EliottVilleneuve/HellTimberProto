using System;
using UnityEngine;
using UnityEngine.Events;


namespace HellLumber
{
    public class AvatarHealth : EntityHealth
    {
        public UnityEvent<float> OnChangeLifePercent;

        protected override void Start()
        {
            base.Start();
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            OnChangeLifePercent?.Invoke(currentHealth * 1f / maxHealth);
        }

        protected override void AnyHurtBehaviour(int damage)
        {
            if(HealthEmpty)
            {
                Destroy(gameObject);
            }
            UpdateHealth();
        }
    }

}
