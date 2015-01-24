using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(Mover))]
    public class Enemy: MonoBehaviour {
        public AttackEffect attackEffect;

        private float attackDelay = 1f;
        private float minDist = 2f;
        private float attackDist = 4f;

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
                if (dir.magnitude > minDist) {
                    dir.Normalize();
                    mover.Move(dir.x, dir.y);
                }
            }
        }

        IEnumerator LoopAttack() {
            yield return new WaitForSeconds(attackDelay + Random.Range(-attackDelay*.5f, attackDelay * .5f));
            while (true) {
                if(player) {
                    // attack if we're kinda close to the player
                    float dist = (player.transform.position - transform.position).magnitude;
                    if (dist < attackDist) {
                        attackEffect.gameObject.SetActive(true);
                        mover.Stop(.2f);
                    }
                }
                yield return new WaitForSeconds(attackDelay + Random.Range(-attackDelay*.5f, attackDelay * .5f));
            }
        }
    }
}