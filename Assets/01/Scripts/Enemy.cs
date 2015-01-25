using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(Mover))]
    public class Enemy: MonoBehaviour {
        public AttackEffect attackEffect;

        public bool CanControl {
            get { return mover.CanControl; }
            set { mover.CanControl = value; }
        }
        private float attackDelay = 1f;
        private float maxMoveDist = 20f;
        private float minMoveDist = 3f;
        private float attackDist = 9f;

        private Mover mover;
        private Player player;

        public void Awake() {
            mover = GetComponent<Mover>();
        }
        public void Start() {
            StartCoroutine(LoopAttack());
            mover.CanControl = false;
            player = GameObject.FindObjectOfType<Player>();
        }

        public void FixedUpdate() {
            // if we're not close & not too far, slowly move towards player
            if (player) {
                Vector3 dir = player.transform.position - transform.position;
                float dist = dir.magnitude;
                if (dist > minMoveDist && dist < maxMoveDist) {
                    dir.Normalize();
                    mover.Move(dir.x, dir.y);
                } else {
                    mover.Move(0, 0);
                }
            }
        }

        IEnumerator LoopAttack() {
            yield return new WaitForSeconds(attackDelay + Random.Range(-attackDelay*.5f, attackDelay * .5f));
            while (true) {
                if(player && CanControl) {
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