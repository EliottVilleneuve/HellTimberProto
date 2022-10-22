using UnityEngine;

namespace HellLumber {

    public class PushableBarricade : EntityHealth {

        public float pushSpeed;
        public Rigidbody rigidbody;

        public LayerMask crushLayer;

        public bool alignToDirection;

        private bool isPushed;
        private Vector3 pushDirection;

        protected override void Start()
        {
            base.Start();
            isPushed = false;
        }

        protected override void DirectionalHurtBehaviour(int damage, Transform from, Vector3 origin)
        {
            base.DirectionalHurtBehaviour(damage, from, origin);

            if (isPushed) return;

            if (!from.TryGetComponent(out AvatarSwing avatarSwing)) return;

            isPushed = true;

            Vector3 dir = transform.position - origin;
            dir.y = 0;

            if (alignToDirection) pushDirection = dir.normalized;
            else pushDirection = (Vector3.Dot(dir, transform.forward) > 0)? transform.forward : -transform.forward;

            transform.rotation = Quaternion.LookRotation(pushDirection);

            Destroy(gameObject,10);
        }

        private void FixedUpdate()
        {
            if (!isPushed) return;

            Vector3 movement = pushDirection * pushSpeed * Time.fixedDeltaTime;
            rigidbody.MovePosition(rigidbody.position + movement);
            PushingRay(movement, 0);
            PushingRay(movement, 1.5f);
            PushingRay(movement, -1.5f);
        }

        private void PushingRay(Vector3 movement, float offset)
        {
            if (Physics.Raycast(transform.position + Vector3.up + transform.right * offset, pushDirection, out RaycastHit hit, 1))
            {
                if (hit.transform.TryGetComponent(out CharacterController enemy))
                {
                    enemy.Move(movement);
                    enemy.GetComponent<Enemy>().timeBetween = 1000;

                    if (Physics.Raycast(enemy.transform.position + Vector3.up, pushDirection, 2, crushLayer))
                    {
                        enemy.GetComponent<EnemyHealth>().NormalHurt(100000);

                    }
                }
                else isPushed = false;
            }
        }
    }
}