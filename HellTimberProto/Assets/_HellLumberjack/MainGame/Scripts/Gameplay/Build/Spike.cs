using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HellLumber
{
    public class Spike : MonoBehaviour
    {
        public float timeBetweenHurt = 0.5f;
        public int hurtDamage = 10;

        private List<EnemyHealth> enemiesInRange;
        private List<float> timesBeforeHurt;

        private void Start()
        {
            enemiesInRange = new List<EnemyHealth>();
            timesBeforeHurt = new List<float>();
        }

        private void Update()
        {
            for (int i = timesBeforeHurt.Count - 1; i >= 0; i--)
            {
                if(enemiesInRange[i] == null)
                {
                    enemiesInRange.RemoveAt(i);
                    timesBeforeHurt.RemoveAt(i);
                    continue;
                }

                timesBeforeHurt[i] -= Time.deltaTime;

                if (timesBeforeHurt[i] <= 0)
                {
                    timesBeforeHurt[i] += timeBetweenHurt;
                    enemiesInRange[i].NormalHurt(hurtDamage);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out EnemyHealth enemyHealth)) return;

            enemiesInRange.Add(enemyHealth);
            timesBeforeHurt.Add(0);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.transform.TryGetComponent(out EnemyHealth enemyHealth)) return;

            int index = enemiesInRange.IndexOf(enemyHealth);
            enemiesInRange.RemoveAt(index);
            timesBeforeHurt.RemoveAt(index);
        }
    }

}

