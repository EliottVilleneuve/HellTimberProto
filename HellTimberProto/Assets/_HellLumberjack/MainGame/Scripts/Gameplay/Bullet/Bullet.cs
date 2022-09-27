using UnityEngine;

namespace HellLumber
{
    public class Bullet : MonoBehaviour
    {
        public int speed = 10;
        public int lifeTime = 30;

        public int damage = 10;

        public float safeStartDuration = 0.5f;

        public LayerMask solidObjectLayer;

        private Transform t;
        private Vector3 bulletMovement;
        private Vector3 currentMovement;
        
        private float remainingSafeTime;
        private Vector3 endScale;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * speed);
        }

        private void Start()
        {
            t = transform;

            remainingSafeTime = safeStartDuration;
            endScale = t.localScale;
            bulletMovement = speed * t.forward;//We save it here because the forward never change

            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            if (remainingSafeTime > 0)
            {
                remainingSafeTime -= Time.deltaTime;
                float coeff = 1f - remainingSafeTime / safeStartDuration;

                t.localScale = Vector3.Lerp(Vector3.zero, endScale, coeff);

                if (remainingSafeTime <= 0)
                {
                    t.localScale = endScale;
                }
            }

            currentMovement = Time.deltaTime * bulletMovement;

            //if (U.RD(transform.position, movement, out RaycastHit hit, Color.yellow, movement.magnitude, solidObjectLayer))
            if (Physics.Raycast(t.position, currentMovement, currentMovement.magnitude, solidObjectLayer))
            {
                Destroy(gameObject);
                return;
            }

            t.position += currentMovement;
        }

        public void Damage(EntityHealth entityHealth)
        {
            if (remainingSafeTime > 0) return;

            entityHealth.DirectionalHurt(damage, t);
            Destroy(gameObject);
        }
    }

}
