using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(Mover))]
    public class Enemy: MonoBehaviour {
        public AttackEffect attackEffect;

        private float attackDelay = 1f;
        private float minDistSqared = 4f;
        private Mover mover;
        private Player player;

        public void Start() {
            mover = GetComponent<Mover>();
            StartCoroutine(LoopAttack());
            player = GameObject.FindObjectOfType<Player>();
        }

        public void FixedUpdate() {
            // if we're not close to the player, slowly move towards them
            if (player) {
                Vector3 dir = player.transform.position - transform.position;
                if (dir.sqrMagnitude > minDistSqared) {
                    dir.Normalize();
                    mover.Move(dir.x, dir.y);
                }
            }
        }

        IEnumerator LoopAttack() {
            while (true) {
                yield return new WaitForSeconds(attackDelay);
                attackEffect.gameObject.SetActive(true);
                mover.Stop(.2f);
            }
        }
    }
}