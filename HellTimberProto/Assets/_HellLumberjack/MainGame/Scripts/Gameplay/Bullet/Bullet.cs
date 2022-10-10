using System;
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

        public float parriedSpeed;
        public int parriedDamage;
        public Renderer visual;
        public Material pariableMaterial;
        public float pariableOdd;
        public bool aimedParry;

        private Transform t;
        private Vector3 bulletMovement;
        private Vector3 currentMovement;
        
        private float remainingSafeTime;
        private Vector3 endScale;

        private bool pariable;
        private bool parried;

        public bool Enemy => !parried;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * speed);
        }

        private void Start()
        {
            parried = false;

            pariable = pariableOdd > UnityEngine.Random.value;
            if (pariable && pariableOdd < 1) visual.material = pariableMaterial;

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

            entityHealth.DirectionalHurt(parried? parriedDamage : damage, t);
            Destroy(gameObject);
        }

        public bool Parry(Transform parryOrigin, Vector3 aimDirection)
        {
            if (!pariable) return false;

            Vector3 parryDirection = -t.forward;
            if (aimedParry) parryDirection = aimDirection;

            /*Vector3 awayPlayer = t.position - parryOrigin.position;
            if (Vector3.Dot(parryDirection, awayPlayer) < 0)
            {
                parryDirection = awayPlayer;
            }*/

            parried = true;
            bulletMovement = parryDirection * parriedSpeed;

            return true;
        }
    }

}
