using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HellLumber {

    public class Bomb : EntityHealth {

        public BombExplosion bombExplosionPrefab;
        public GameObject visual;
        public GameObject ignitFeedback;

        public float timeBeforeExplode;
        public float destroyDelayAfterExplode;

        public UnityEvent OnExplode;

        private BombExplosion bombExplosion;

        protected override void Start()
        {
            base.Start();
            ignitFeedback.SetActive(false);
            visual.SetActive(true);
        }

        public void OnPlace()
        {
            bombExplosion = Instantiate(bombExplosionPrefab, transform.position, transform.rotation);
        }

        protected override void AnyHurtBehaviour(int damage)
        {
            base.AnyHurtBehaviour(damage);

            StartCoroutine(IgnitCoroutine());
        }

        private IEnumerator IgnitCoroutine()
        {
            ignitFeedback.SetActive(true);

            yield return new WaitForSeconds(timeBeforeExplode);

            ignitFeedback.SetActive(false);
            visual.SetActive(false);

            bombExplosion.Explode();
            OnExplode?.Invoke();
            Destroy(gameObject, destroyDelayAfterExplode);
            Destroy(bombExplosion.gameObject, destroyDelayAfterExplode);
        }
    }
}