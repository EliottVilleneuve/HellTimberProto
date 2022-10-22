using System;
using UnityEngine;
using UnityEngine.Events;


namespace HellLumber
{
    public class AvatarHealth : EntityHealth
    {
        public UnityEvent<float> OnChangeLifePercent;

        [HideInInspector] public bool autoGameOver = true;

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
                GameOver();
            }
            UpdateHealth();
        }

        public void GameOver()
        {
            Destroy(gameObject);
        }
    }

}
