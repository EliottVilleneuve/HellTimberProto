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
                if (buildInfos[i].blocked) continue;
                if (Input.GetButtonDown(buildInfos[i].inputAxisName))
                {
                    currentWoodQuantity -= woodQuantityLostPerBuild;
                    UpdateWoodCount();

                    Buildable buildable = Instantiate(buildInfos[i].buildable, spawnPoint);
                    buildable.Setup(true);
                    buildable.Place(spawnPoint);

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