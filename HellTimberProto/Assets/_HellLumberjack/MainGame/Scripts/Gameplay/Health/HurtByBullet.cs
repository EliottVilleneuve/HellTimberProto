using UnityEngine;

namespace HellLumber
{
    public class HurtByBullet : MonoBehaviour
    {
        public const string BULLET_TAG = "Bullet";

        public EntityHealth entityHealth;
        public bool hurtByEnemyBullet = true;
        public bool useTriggerEnter = true;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(BULLET_TAG)) return;

            if (other.TryGetComponent(out Bullet bullet))
            {
                if (bullet.Enemy == hurtByEnemyBullet) bullet.Damage(entityHealth);
            }
            else
            {
                Debug.LogWarning("No script found on this bullet !");
                Destroy(other.gameObject);
                entityHealth.DirectionalHurt(10, other.transform);
            }
        }
    }
}

