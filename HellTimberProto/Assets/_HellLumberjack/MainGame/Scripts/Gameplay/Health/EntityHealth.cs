using System;
using UnityEngine;
using UnityEngine.Events;

namespace HellLumber
{
    public class EntityHealth : MonoBehaviour
    {
        public int maxHealth = 50;
        public bool autoDestroy = false;

        protected int currentHealth;
        private bool healthEmpty;
        private bool destroyed;

        public bool HealthEmpty => healthEmpty;

        public event Action OnHurt;

        public UnityEvent OnDamage;
        public UnityEvent OnAutoDestroy;

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            healthEmpty = false;
            destroyed = false;
        }

        private void Hurt(int damage)
        {
            if (destroyed)
            {
                return;
            }

            currentHealth -= damage;
            healthEmpty = currentHealth <= 0;

            OnDamage?.Invoke();
            OnHurt?.Invoke();
            AnyHurtBehaviour(damage);

            if (healthEmpty)
            {
                if (autoDestroy)
                {
                    Destroy(gameObject);
                    OnAutoDestroy?.Invoke();
                }
                destroyed = true;
            }
        }

        public void NormalHurt(int damage)
        {
            if (destroyed) return;

            Hurt(damage);

            NormalHurtBehaviour(damage);
        }

        public void DirectionalHurt(int damage, Transform from) => DirectionalHurt(damage, from, from.position);

        public void DirectionalHurt(int damage, Transform from, Vector3 origin)
        {
            if (destroyed) return;

            Hurt(damage);

            DirectionalHurtBehaviour(damage, from, origin);
        }
        protected virtual void AnyHurtBehaviour(int damage) { }
        protected virtual void NormalHurtBehaviour(int damage) { }
        protected virtual void DirectionalHurtBehaviour(int damage, Transform from, Vector3 origin) { }
    }

}
