using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HellLumber
{
    public class AvatarSlowedWhenHolding : MonoBehaviour
    {
        public AvatarSwing avatarSwing;
        public AvatarMove avatarMove;

        public AnimationCurve speedMultiplierOverHoldPercent;

        private Action doAction;

        private void Start()
        {
            avatarSwing.OnSwitchHold += AvatarSwing_OnSwitchHold;
            SetMode(false);
        }

        private void OnDestroy()
        {
            SetMode(false);
            avatarSwing.OnSwitchHold -= AvatarSwing_OnSwitchHold;
        }

        private void Update()
        {
            doAction();
        }

        private void AvatarSwing_OnSwitchHold(bool hold)
        {
            SetMode(hold);
        }

        private void SetMode(bool hold)
        {
            if (hold) doAction = DoActionHold;
            else
            {
                doAction = DoActionVoid;
                avatarMove.SetMultiplier(1);
            }
        }

        public void DoActionVoid() { }
        public void DoActionHold() 
        {
            float holdMultiplier = speedMultiplierOverHoldPercent.Evaluate(avatarSwing.HoldingStrength);
            avatarMove.SetMultiplier(holdMultiplier);
        }
    }

}
