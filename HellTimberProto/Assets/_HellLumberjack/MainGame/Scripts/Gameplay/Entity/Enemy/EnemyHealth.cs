using UnityEngine;

namespace HellLumber
{
    public class EnemyHealth : EntityHealth
    {
        public Enemy enemy;

        protected override void NormalHurtBehaviour(int damage)
        {
            enemy.NormalHit(HealthEmpty);
        }
        protected override void DirectionalHurtBehaviour(int damage, Transform from, Vector3 origin)
        {
            Vector3 knockBackDirection = transform.position - origin;
            knockBackDirection.y = 0;
            knockBackDirection.Normalize();

            enemy.KnockBackHit(knockBackDirection, HealthEmpty, damage);
        }
    }
}
