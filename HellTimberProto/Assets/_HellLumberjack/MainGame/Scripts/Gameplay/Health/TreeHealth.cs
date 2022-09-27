using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HellLumber
{
    public class TreeHealth : EntityHealth
    {
        public float fallTime = 1;

        public CapsuleCollider capsuleCollider;
        public Transform pivotablePart;

        public float pivotOffset;

        public float topY = 1;
        public float bottomY = 0;

        public int numPiece = 4;
        public float pieceOffset;
        public GameObject woodPiecePrefab;

        public int fallDamage = 70;
        public float damageBoxWidth = 2;

        public UnityEvent OnFall;
        public UnityEvent OnHitGround;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 bottomTree = PosFromY(bottomY);
            Vector3 topTree = PosFromY(topY);

            Gizmos.DrawRay(bottomTree, Vector3.right * 5);
            Gizmos.DrawRay(topTree, Vector3.right * 5);

            Gizmos.color = Color.yellow;
            PiecesMethod((Vector3 pos) => { Gizmos.DrawRay(pos, -Vector3.right * 5); });

            Gizmos.color = Color.red;
            Vector3 damageBoxSize = Vector3.one * damageBoxWidth;
            damageBoxSize.y = Mathf.Abs(topY - bottomY);
            Gizmos.DrawWireCube((bottomTree + topTree) * 0.5f, damageBoxSize);
        }

        private void PiecesMethod(Action<Vector3> method)
        {
            Vector3 bottomPiece = PosFromY(bottomY + pieceOffset);
            Vector3 topPiece = PosFromY(topY - pieceOffset);

            for (int i = 0; i < numPiece; i++)
            {
                float coeff = i * 1f / (numPiece - 1);
                Vector3 pos = Vector3.Lerp(bottomPiece, topPiece, coeff);
                method(pos);
            }
        }

        private Vector3 PosFromY(float y)
        {
            return transform.TransformPoint(Vector3.up * y);
        }

        protected override void DirectionalHurtBehaviour(int damage, Transform from, Vector3 origin)
        {
            if (!HealthEmpty) return;

            StartCoroutine(FallTree(origin));
        }

        private IEnumerator FallTree(Vector3 from)
        {
            OnFall?.Invoke();

            float coeff = 0;

            Vector3 direction = transform.position - from;
            direction.y = 0;
            direction.Normalize();

            Vector3 offset = direction * pivotOffset;
            Vector3 localOffset = transform.InverseTransformVector(offset);

            capsuleCollider.center -= localOffset;

            transform.position += offset;
            pivotablePart.position -= offset;

            Vector3 pivotAxis = Quaternion.AngleAxis(90, Vector3.up) * direction;

            Quaternion startRot = transform.rotation;
            Quaternion endRot = Quaternion.AngleAxis(90, pivotAxis) * startRot;

            while(coeff < 1)
            {
                coeff += Time.deltaTime / fallTime;

                transform.rotation = Quaternion.Slerp(startRot, endRot, coeff);

                yield return null;
            }

            transform.rotation = endRot;


            PiecesMethod((pos) =>
            {
                Vector3 spawnPos = pos;
                spawnPos.y = 0;
                Instantiate(woodPiecePrefab, spawnPos, Quaternion.identity);
            });

            //Damage cast
            Vector3 center = (PosFromY(bottomY) + PosFromY(topY)) * 0.5f;
            Vector3 halfExtents = new Vector3(2, Mathf.Abs(topY - bottomY) * 0.5f, 1);

            RaycastHit[] hits = Physics.BoxCastAll(center, halfExtents, -Vector3.up, transform.rotation, 10);

            for (int i = 0; i < hits.Length; i++)
            {
                //Debug.DrawRay(hits[i].transform.position, Vector3.up, Color.green);
                if (!hits[i].transform.TryGetComponent(out EnemyHealth enemyHealth)) continue;

                enemyHealth.DirectionalHurt(fallDamage, transform, center);
            }

            OnHitGround?.Invoke();
            Destroy(gameObject);
        }
    }
}

