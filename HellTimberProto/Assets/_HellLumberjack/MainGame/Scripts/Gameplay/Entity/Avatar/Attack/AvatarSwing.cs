using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HellLumber
{
    public class AvatarSwing : MonoBehaviour
    {
        public WeaponHitbox normalAxe;
        public WeaponHitbox holdAxe;

        public GameObject normalRestVisual;
        public GameObject normalSwingVisual;

        public GameObject holdRestVisual;
        public GameObject holdSwingVisual;

        public float swingTime = 0.5f;

        public float timeToStartHold = 0.5f;
        public float timeToMaxHold = 2f;
        public float baseHoldDamageBoost = 1;
        public float holdStrengthDamageBoost = 1;

        public int damage = 10;

        public Transform damageBoostFeedback;
        public AnimationCurve feedbackSizeEvoDamageBoost;

        public bool overrideSwingInput;
        public string overridingSwingInput;

        private float timeBeforeAxeUsable;
        private float holdingTime;

        private bool isHolding;
        private float holdingStrength;

        private int damageBoost;

        public UnityEvent OnSwing;
        public UnityEvent OnCircularSwing;
        public UnityEvent OnCircularSwingFatalHits;

        public UnityEvent OnStartHold;
        public UnityEvent OnStopHold;

        /// <summary>
        /// Treu when we switched to hold, false otherwise
        /// </summary>
        public event Action<bool> OnSwitchHold;

        public float HoldingStrength => holdingStrength;

        public string SwingInput => overrideSwingInput? overridingSwingInput : "Fire1";

        private void Start()
        {
            isHolding = false;
            normalRestVisual.SetActive(true);
            holdRestVisual.SetActive(false);
            UpdateDamageBoostFeedback();
        }

        void Update()
        {
            if(timeBeforeAxeUsable > 0)
            {
                timeBeforeAxeUsable -= Time.deltaTime;
            }
            else
            {
                if (Controller.GetButtonDown(SwingInput, ButtonInputType.Swing))
                {
                    SwingWeapon(normalAxe, damage, normalSwingVisual, normalRestVisual);
                    return;
                }
            }

            if (Controller.GetButton(SwingInput, ButtonInputType.Swing))
            {
                holdingTime += Time.deltaTime;

                if (isHolding)
                {
                    if (holdingTime > timeToMaxHold) holdingStrength = 1;
                    else holdingStrength = Mathf.Clamp01(holdingTime - timeToStartHold / (timeToMaxHold - timeToStartHold));
                }
                else if (holdingTime > timeToStartHold)
                {
                    isHolding = true;
                    OnStartHold?.Invoke();
                    OnSwitchHold?.Invoke(true);

                    normalRestVisual.SetActive(false);
                    holdRestVisual.SetActive(true);
                }
            }
            if (Controller.GetButtonUp(SwingInput, ButtonInputType.Swing))
            {
                if(isHolding)
                {
                    //Swing
                    float damageBoost = 1 + baseHoldDamageBoost + holdingStrength * holdStrengthDamageBoost;
                    int holdDamage = Mathf.CeilToInt(damage * damageBoost);
                    SwingWeapon(holdAxe, holdDamage, holdSwingVisual, holdRestVisual);

                    OnStopHold?.Invoke();

                    //Reset
                    holdingStrength = 0;
                    isHolding = false;
                    OnSwitchHold?.Invoke(false);
                }

                holdingTime = 0;
            }
        }

        private void SwingWeapon(WeaponHitbox weapon, int damage, GameObject swingVisual, GameObject restVisual)
        {
            AttackResult attackResult = weapon.Attack(damage + damageBoost, transform);
            ResetDamageBoost();

            if (weapon == holdAxe)
            {
                OnCircularSwing?.Invoke();
                if(attackResult.targetsFatalHit > 0) OnCircularSwingFatalHits?.Invoke();
            }
            else OnSwing?.Invoke();

            StartCoroutine(SwingAnim(swingVisual, restVisual));
            timeBeforeAxeUsable = swingTime;
        }

        private IEnumerator SwingAnim(GameObject swingVisual, GameObject restVisual)
        {
            swingVisual.SetActive(true);
            restVisual.SetActive(false);

            yield return new WaitForSeconds(0.2f);

            swingVisual.SetActive(false);

            normalRestVisual.SetActive(true);
        }

        public void AddDamageBoost(int boostAdded)
        {
            damageBoost += boostAdded;
            UpdateDamageBoostFeedback();
        }
        public void ResetDamageBoost()
        {
            damageBoost = 0;
            UpdateDamageBoostFeedback();
        }
        private void UpdateDamageBoostFeedback()
        {
            damageBoostFeedback.localScale = Vector3.one * feedbackSizeEvoDamageBoost.Evaluate(damageBoost);
        }
    }
}

