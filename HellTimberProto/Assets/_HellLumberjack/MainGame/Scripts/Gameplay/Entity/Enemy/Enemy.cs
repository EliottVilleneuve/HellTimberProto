using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HellLumber
{
    public class Enemy : MonoBehaviour
    {
        [Header("Move")]
        public CharacterController characterController;

        public Transform avatar;

        public float moveSpeed = 5;

        [Header("Shoot")]
        public Transform bulletSpawnPointsParent;

        public float timeBetween = 0.5f;

        public float startUpTime = 2f;

        [Header("Hurt")]
        public EnemyHealth enemyHealth; 

        public float knockBackDistance = 2;
        public float stunDuration = 0.5f;
        public float finalLaunchSpeed = 10;
        public float launchedLifeTime = 10;

        public float baseHitStun = 0.01f;
        public float hitStunByDamage = 0.01f;

        private BulletSpawnPoint[] bulletSpawnPoints;

        private float timeBeforeShoot;
        
        private float stunTime;
        private Vector3 knockBack;

        private float hitStunTime;

        private bool knockedOut;
        private ControllerColliderHit lastHit;

        private Action doAction;

        public UnityEvent OnShoot;

        public UnityEvent OnKOLaunch;
        public UnityEvent OnKODestroy;

        private void Start()
        {
            timeBeforeShoot = startUpTime + UnityEngine.Random.value * timeBetween;
            stunTime = 0;
            knockedOut = false;

            doAction = DoActionAttack;

            int numPoint = bulletSpawnPointsParent.childCount;
            bulletSpawnPoints = new BulletSpawnPoint[numPoint];
            for (int i = 0; i < numPoint; i++)
            {
                bulletSpawnPoints[i] = bulletSpawnPointsParent.GetChild(i).GetComponent<BulletSpawnPoint>();
            }
        }

        private void Update()
        {
            doAction();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (doAction != DoActionLaunched) return;

            //Prevent collision detection from same object
            if (lastHit != null) if (lastHit.gameObject == hit.gameObject) return;

            if (hit.transform.TryGetComponent(out EnemyHealth otherEnemyHealth))
            {
                //hit another enemy !
                otherEnemyHealth.DirectionalHurt(10, transform);
                if (!knockedOut) enemyHealth.DirectionalHurt(10, otherEnemyHealth.transform);
            }

            if (knockedOut)
            {
                EnemyDestroy();
            }
        }

        private void EnemyDestroy()
        {
            OnKODestroy?.Invoke();
            Destroy(gameObject);
        }

        private void DoActionLaunched()
        {
            characterController.Move(knockBack * Time.deltaTime);

            if (!knockedOut)
            {
                stunTime -= Time.deltaTime;

                if (stunTime < 0)
                {
                    lastHit = null;
                    doAction = DoActionAttack;
                }
            }
        }

        private void DoActionAttack()
        {
            if(stunTime > 0)
            {
                stunTime -= Time.deltaTime;
                return;
            }

            if(avatar == null) return;

            Vector3 move = avatar.position - transform.position;
            move.y = 0;
            move = move.normalized * moveSpeed * Time.deltaTime;

            characterController.Move(move);

            timeBeforeShoot -= Time.deltaTime;

            if (timeBeforeShoot > 0) return;
            timeBeforeShoot = timeBetween;

            for (int i = 0; i < bulletSpawnPoints.Length; i++)
            {
                bulletSpawnPoints[i].Shoot();
            }

            OnShoot?.Invoke();
        }

        public void NormalHit(bool KO)
        {
            if (KO) EnemyDestroy();
            else SetStunTime(stunDuration);
        }

        public void KnockBackHit(Vector3 knockBackDirection, bool KO, int damage)
        {
            if(KO)
            {
                knockBack = knockBackDirection * finalLaunchSpeed;
                knockedOut = true;

                Destroy(gameObject, launchedLifeTime);
                OnKOLaunch?.Invoke();
            }
            else
            {
                knockBack = knockBackDirection * knockBackDistance / stunDuration;
                SetStunTime(stunDuration);
            }

            hitStunTime = baseHitStun + hitStunByDamage * damage;
            doAction = DoActionHitStun;
            //doAction = DoActionLaunched;
        }

        public void SetStunTime(float duration)
        {
            stunTime = duration;
        }

        private void DoActionHitStun()
        {
            hitStunTime -= Time.deltaTime;

            if (hitStunTime < 0)
            {
                doAction = DoActionLaunched;
            }
        }
    }
}
