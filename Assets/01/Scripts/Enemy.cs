using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(Mover))]
    public class Enemy: MonoBehaviour {
        public AttackEffect attackEffect;

        private float attackDelay = 1f;

        private Mover mover;

        public void Start() {
            mover = GetComponent<Mover>();
            StartCoroutine(LoopAttack());
        }

        public void FixedUpdate() {
            mover.Move(-.5f, 0);
        }

        IEnumerator LoopAttack() {
            while (true) {
                attackEffect.gameObject.SetActive(true);
                yield return new WaitForSeconds(attackDelay);
            }
        }
    }
}