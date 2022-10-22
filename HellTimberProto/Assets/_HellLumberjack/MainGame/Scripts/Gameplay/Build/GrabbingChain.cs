///-----------------------------------------------------------------
///   Author : #DEVELOPER_NAME#                    
///   Date   : #DATE#
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HellLumber {

    public class GrabbingChain : MonoBehaviour {

        public EntityHealth entityHealth;

        public GameObject zone;
        public Collider zoneTrigger;

        public GameObject activeFeedback;

        public AnimationCurve grabSpeedEvoDistance = default;
        public float grabbingTime = 1;
        public float coolDown = 2;
        public float stunTimeFromGrab = 2;
        
        private List<Enemy> grabbedEnemies = new List<Enemy>();

        private bool isGrabbing = false;
        private float remainingGrabbingTime;
        private float remainingCooldown;

        private void Start()
        {
            isGrabbing = false;
            remainingGrabbingTime = 0;

            entityHealth.OnDirectionalDamage += EntityHealth_OnDirectionalDamage;

            StartGrab();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Enemy enemy)) return;

            grabbedEnemies.Add(enemy);
            enemy.SetStunTime(stunTimeFromGrab);
        }

        private void Update()
        {
            if (!isGrabbing)
            {
                if(remainingCooldown > 0) remainingCooldown -= Time.deltaTime;
                else if (remainingCooldown < 0)
                {
                    remainingCooldown = 0;
                    activeFeedback.SetActive(true);
                }
                return;
            }

            Vector3 dir;
            for (int i = grabbedEnemies.Count - 1; i >= 0; i--)
            {
                if (grabbedEnemies[i] == null)
                {
                    grabbedEnemies.RemoveAt(i);
                    continue;
                }

                dir = transform.position - grabbedEnemies[i].transform.position;
                dir = dir.normalized * grabSpeedEvoDistance.Evaluate(dir.magnitude);
                grabbedEnemies[i].characterController.Move(dir * Time.deltaTime);
            }

            remainingGrabbingTime -= Time.deltaTime;
            if(remainingGrabbingTime < 0)
            {
                isGrabbing = false;
                grabbedEnemies.Clear();
                ActivateGrabElement(false);

                remainingCooldown = coolDown;
            }
        }

        private void EntityHealth_OnDirectionalDamage(Transform from, Vector3 origin)
        {
            if (isGrabbing) return;
            if (!from.TryGetComponent(out AvatarSwing avatarSwing)) return;

            if (remainingCooldown <= 0) StartGrab();
        }

        private void StartGrab()
        {
            isGrabbing = true;
            remainingGrabbingTime = grabbingTime;
            ActivateGrabElement(true);

            activeFeedback.SetActive(false);
        }

        private void ActivateGrabElement(bool activation)
        {
            zone.SetActive(activation);
            zoneTrigger.enabled = activation;
        }
    }
}