using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HellLumber
{
    public class EnemyPlayerDetect : MonoBehaviour
    {
        public Enemy enemy;
        public float timeBetweenUpdate = 3;
        public float checkRadius = 10;

        public LayerMask playerLayer;

        private float timeBeforeUpdate;

        private bool avatarFound;
        private AvatarTarget avatar;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(avatar == null)
            {
                avatar = null;
                avatarFound = false;
            }

            if(!avatarFound)
            {
                UpdateAvatarReference();
                return;
            }

            timeBeforeUpdate -= Time.deltaTime;
            if (timeBeforeUpdate > 0) return;

            timeBeforeUpdate = timeBetweenUpdate;
            UpdateAvatarReference();
        }

        private void UpdateAvatarReference()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, checkRadius, Vector3.up, 1, playerLayer);

            List<AvatarTarget> targetsFound = new List<AvatarTarget>();

            for (int i = 0; i < hits.Length; i++)
            {
                if (!hits[i].transform.CompareTag("Avatar")) continue;
                if (!hits[i].transform.TryGetComponent(out AvatarTarget avatarTarget)) continue;

                targetsFound.Add(avatarTarget);
            }

            if (targetsFound.Count <= 0) return;

            AvatarTarget bestTarget = null;
            float lowestDistance = 100;

            for (int i = 0; i < targetsFound.Count; i++)
            {
                float currentDistance = Vector3.Distance(transform.position, targetsFound[i].transform.position) - targetsFound[i].priorityOverDistance;

                if (currentDistance < lowestDistance)
                {
                    lowestDistance = currentDistance;
                    bestTarget = targetsFound[i];
                }
            }

            //Once we found the best target
            avatarFound = true;
            avatar = bestTarget;

            enemy.avatar = avatar.transform;
        }
    }
}

