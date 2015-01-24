using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    public class Enemy: MonoBehaviour {
        public AttackEffect attackEffect;

        private float attackDelay = 1f;

        public void Start() {
            StartCoroutine(LoopAttack());
        }

        IEnumerator LoopAttack() {
            while (true) {
                attackEffect.gameObject.SetActive(true);
                yield return new WaitForSeconds(attackDelay);
            }
        }
    }
}