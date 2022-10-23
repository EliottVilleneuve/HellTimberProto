///-----------------------------------------------------------------
///   Author : #DEVELOPER_NAME#                    
///   Date   : #DATE#
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace HellLumber {

    public class BombExplosion : MonoBehaviour {

        public MeshRenderer[] visual;

        public int damage;

        private List<EnemyHealth> enemiesInRange;
        private List<Bullet> bulletsInRange;

        private void Start()
        {
            foreach(var v in visual) v.enabled = false;
            enemiesInRange = new List<EnemyHealth>();
            bulletsInRange = new List<Bullet>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                bulletsInRange.Add(bullet);
                return;
            }
            if (!other.TryGetComponent(out EnemyHealth enemy)) return;

            enemiesInRange.Add(enemy);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                bulletsInRange.Remove(bullet);
                return;
            }
            if (!other.TryGetComponent(out EnemyHealth enemy)) return;

            enemiesInRange.Remove(enemy);
        }

        public void Explode()
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].DirectionalHurt(damage, transform);
            }
            for (int j = 0; j < bulletsInRange.Count; j++)
            {
                if (bulletsInRange[j] == null) continue;
                Destroy(bulletsInRange[j].gameObject);
            }
            bulletsInRange.Clear();
            foreach (var v in visual) v.enabled = true;
        }
    }
}