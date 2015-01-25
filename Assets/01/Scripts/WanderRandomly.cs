using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(Mover))]
    public class WanderRandomly: MonoBehaviour {

        private Vector3 currentDir;
        private float changeCooldown= 1;
        private Mover mover;
        private Player player;

        public void Start() {
            mover = GetComponent<Mover>();
            player = GameObject.FindObjectOfType<Player>();
            StartCoroutine(ControlDirection());
        }
        public void FixedUpdate() {
            mover.Move(currentDir.x, currentDir.y);
        }

        private IEnumerator ControlDirection() {
            while (true) {
                ChangeDirection();
                yield return new WaitForSeconds(changeCooldown
                    + Random.Range(-changeCooldown / 2, changeCooldown / 2));
            }
        }

        private void ChangeDirection() {
            // give it a 1/2 chance of walking towards the player
            if (Random.Range(0, 2) >= 1) {
                currentDir = player.transform.position - transform.position;
            } else {
                currentDir = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    0);
            }
            currentDir.Normalize();
        }
    }
}