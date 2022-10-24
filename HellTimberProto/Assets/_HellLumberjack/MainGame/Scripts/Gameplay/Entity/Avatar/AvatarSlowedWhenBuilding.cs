///-----------------------------------------------------------------
///   Author : #DEVELOPER_NAME#                    
///   Date   : #DATE#
///-----------------------------------------------------------------

using UnityEngine;

namespace HellLumber {

    public class AvatarSlowedWhenBuilding : MonoBehaviour {

        public AvatarMove avatarMove;
        public AvatarBuildWithHealth avatarBuildWithHealth;

        public float speedMultiplierWhenBuilding = 0.3f;

        private void Start () {
            avatarBuildWithHealth.OnSwitchBuildMode += AvatarBuildWithHealth_OnSwitchBuildMode;
        }

        private void AvatarBuildWithHealth_OnSwitchBuildMode(bool buildMode)
        {
            avatarMove.SetMultiplier(buildMode ? speedMultiplierWhenBuilding : 1);
        }
    }
}