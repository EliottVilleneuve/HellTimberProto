using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HellLumber
{
    public class WeaponHitbox : MonoBehaviour
    {
        public Transform aimForward;
        public AnimationCurve zRangeEvoForward;

        public new Collider collider;
        public Collider avatarCollider;

        private List<EntityHealth> targetsInRange;

        private bool adjustCollider;
        private BoxCollider boxCollider;

        private void Start()
        {
            targetsInRange = new List<EntityHealth>();
            Physics.IgnoreCollision(avatarCollider, collider, true);

            adjustCollider = collider.transform.TryGetComponent(out BoxCollider boxCollider);

            if (adjustCollider) this.boxCollider = boxCollider;
        }

        private void Update()
        {
            if(adjustCollider)
            {
                float dot = Vector3.Dot(Vector3.forward, aimForward.forward);
                float zRange = zRangeEvoForward.Evaluate(dot);

                boxCollider.size = V.SetZ(boxCollider.size, zRange);
                boxCollider.center = V.SetZ(boxCollider.center, zRange * 0.5f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent(out EntityHealth hurtable)) return;

            if (!targetsInRange.Contains(hurtable)) targetsInRange.Add(hurtable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.transform.TryGetComponent(out EntityHealth hurtable)) return;

            if (targetsInRange.Contains(hurtable)) targetsInRange.Remove(hurtable);
        }

        public AttackResult Attack(int damage, Transform origin)
        {
            AttackResult attackResult = new AttackResult(targetsInRange.Count);
            
            if (targetsInRange.Count > 0)
            {
                for (int i = targetsInRange.Count - 1; i >= 0; i--)
                {
                    if (targetsInRange[i] == null)
                    {
                        targetsInRange.RemoveAt(i);
                        continue;
                    }
                    targetsInRange[i].DirectionalHurt(damage, origin);
                    if (targetsInRange[i].HealthEmpty) attackResult.targetsFatalHit++;
                    //Debug.Log("cut " + colliding[i].transform.name);
                }
            }

            return attackResult;
        }
    }

    public struct AttackResult
    {
        public int targetsHit;
        public int targetsFatalHit;

        public AttackResult(int targetsHit)
        {
            this.targetsHit = targetsHit;
            targetsFatalHit = 0;
        }
    }

}
