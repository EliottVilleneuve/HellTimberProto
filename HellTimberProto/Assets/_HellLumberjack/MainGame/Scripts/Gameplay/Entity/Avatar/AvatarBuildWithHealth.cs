using System;
using UnityEngine;
using UnityEngine.Events;

namespace HellLumber {

    public class AvatarBuildWithHealth : MonoBehaviour {

        public Transform spawnPoint;
        public AvatarHealth avatarHealth;

        public float maxWoodQuantity = 100;
        public float woodQuantityGainedPerLog = 10;
        public float woodQuantityLostPerDamage = 10;
        public float woodQuantityLostPerBuild = 10;

        public BuildInfo[] buildInfos;

        private float currentWoodQuantity = 50;
        private Buildable currentBuildable;

        public UnityEvent<float> OnChangeQuantityPercent;
        public UnityEvent OnBuild;
        public UnityEvent OnPickupWood;

        private void Start () 
        {
            avatarHealth.OnHurt += AvatarHealth_OnHurt;
            avatarHealth.autoGameOver = false;

            UpdateWoodCount();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.CompareTag("WoodPiece")) return;
            if (currentWoodQuantity >= maxWoodQuantity) return;

            Destroy(other.gameObject);
            currentWoodQuantity += woodQuantityGainedPerLog;

            if (currentWoodQuantity >= maxWoodQuantity) currentWoodQuantity = maxWoodQuantity;
            UpdateWoodCount();

            OnPickupWood?.Invoke();
        }

        private void Update()
        {
            if (currentWoodQuantity < woodQuantityLostPerBuild) return;

            for (int i = 0; i < buildInfos.Length; i++)
            {
                if (Input.GetButtonDown(buildInfos[i].inputAxisName) && !buildInfos[i].blocked)
                {
                    if (currentBuildable != null) return;
                    
                    currentBuildable = Instantiate(buildInfos[i].buildable, spawnPoint.position, spawnPoint.rotation);
                    currentBuildable.transform.SetParent(spawnPoint);
                    currentBuildable.Setup(true);
                }
                else if (Input.GetButtonUp(buildInfos[i].inputAxisName))
                {
                    if (currentBuildable == null) return;

                    currentBuildable.Place(spawnPoint);
                    currentBuildable = null;

                    currentWoodQuantity -= woodQuantityLostPerBuild;
                    UpdateWoodCount();

                    OnBuild?.Invoke();
                }
            }
        }

        private void UpdateWoodCount()
        {
            OnChangeQuantityPercent?.Invoke(currentWoodQuantity / maxWoodQuantity);
        }

        private void AvatarHealth_OnHurt()
        {
            if (currentWoodQuantity == 0)
            {
                avatarHealth.GameOver();
                return;
            }

            currentWoodQuantity -= woodQuantityLostPerDamage;
            if (currentWoodQuantity < 0) currentWoodQuantity = 0;
            UpdateWoodCount();
        }
    }

    [Serializable]
    public struct BuildInfo
    {
        public string inputAxisName;
        public Buildable buildable;
        public bool blocked;
    }
}