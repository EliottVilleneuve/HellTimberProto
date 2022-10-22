///-----------------------------------------------------------------
///   Author : #DEVELOPER_NAME#                    
///   Date   : #DATE#
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace HellLumber {

    public class AvatarDashNearTotem : MonoBehaviour {
        private const string TOTEMZONE_TAG = "TotemZone";

        public AvatarDash avatarDash;
        public AvatarMove avatarMove;
        public AvatarBuildWithHealth avatarBuildWithHealth;

        public float inZoneSpeedMultiplier = 1.5f;
        public int buildToBlock = 0;

        private List<Collider> zonesIn = new List<Collider>();
        private bool dashActive = false;

        private void Start () 
        {
            avatarDash.enabled = false;
            avatarDash.dashType = DashType.MiddleClick;
        }

        private void Update()
        {
            if (!dashActive) return;

            for (int i = zonesIn.Count - 1; i >= 0; i--)
            {
                if (zonesIn[i] == null) zonesIn.RemoveAt(i);
            }

            if (zonesIn.Count <= 0) AllowDash(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(TOTEMZONE_TAG)) return;

            if (zonesIn.Contains(other)) return;

            zonesIn.Add(other);
            AllowDash(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(TOTEMZONE_TAG)) return;

            zonesIn.Remove(other);

            if (zonesIn.Count > 0) return;

            AllowDash(false);
        }
        
        private void AllowDash(bool allow)
        {
            dashActive = allow;
            avatarDash.enabled = allow;
            avatarMove.SetMultiplier(allow? inZoneSpeedMultiplier : 1);
            avatarBuildWithHealth.buildInfos[buildToBlock].blocked = allow;
        }
    }
}