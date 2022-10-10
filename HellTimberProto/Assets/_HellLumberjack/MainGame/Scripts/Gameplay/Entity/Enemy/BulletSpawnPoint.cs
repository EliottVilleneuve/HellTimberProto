using UnityEngine;

namespace HellLumber
{
    public class BulletSpawnPoint : MonoBehaviour
    {
        public Bullet bulletPrefab;
        public Vector3 offset;

        public Vector3 SpawnPos => transform.position + transform.TransformDirection(offset);

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(SpawnPos, transform.forward);
        }

        public void Shoot()
        {
            Instantiate(bulletPrefab, SpawnPos, transform.rotation);
        }
    }
}
