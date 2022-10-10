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

        private bool immune;

        public bool HealthEmpty => healthEmpty;

        public event Action OnHurt;

        public UnityEvent OnDamage;
        public UnityEvent OnAutoDestroy;

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            healthEmpty = false;
            destroyed = false;
            immune = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damage"></param>
        /// <returns>Return true if succesfully damaged</returns>
        private bool Hurt(int damage)
        {
            if (destroyed || immune)
            {
                return false;
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
            return true;
        }

        public void NormalHurt(int damage)
        {
            if (Hurt(damage)) return;

            NormalHurtBehaviour(damage);
        }

        public void DirectionalHurt(int damage, Transform from) => DirectionalHurt(damage, from, from.position);

        public void DirectionalHurt(int damage, Transform from, Vector3 origin)
        {
            if (!Hurt(damage)) return;

            DirectionalHurtBehaviour(damage, from, origin);
        }
        protected virtual void AnyHurtBehaviour(int damage) { }
        protected virtual void NormalHurtBehaviour(int damage) { }
        protected virtual void DirectionalHurtBehaviour(int damage, Transform from, Vector3 origin) { }

        public void SetImmunity(bool immunity)
        {
            immune = immunity;
        }
    }

}
