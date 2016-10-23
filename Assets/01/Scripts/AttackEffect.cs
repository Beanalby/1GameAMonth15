using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    public class AttackEffect: MonoBehaviour {

        public float damage = 1;

        private float maxYDist = 2f;

        private float started, duration = .25f;
        public bool hitEnemy;
        private int targetLayer;
        BoxCollider2D box;
        SpriteRenderer sprite;

        public void Start() {
            box = GetComponent<BoxCollider2D>();
            sprite = GetComponent<SpriteRenderer>();
            box.enabled = false;
            sprite.enabled = false;
        }

        public void Attack() {
            if (started != -1) {
                return;
            }
            started = Time.time;
            box.enabled = true;
            sprite.enabled = true;
            if (hitEnemy) {
                targetLayer = LayerMask.NameToLayer("enemy");
            } else {
                targetLayer = LayerMask.NameToLayer("player");
            }
            if (GetComponent<AudioSource>() && Time.time > .1f) {
                GetComponent<AudioSource>().Play();
            }
        }

        public void Update() {
            if ((Time.time - started) > duration) {
                box.enabled = false;
                sprite.enabled = false;
                started = -1;
            }
        }
        public void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.layer == targetLayer) {
                // even if the trigger hit, make sure our y coord isn't too
                // far away, or we'll consider them far apart in our fake
                // 2d engine.
                float dist = Mathf.Abs(transform.parent.position.y - other.transform.position.y);
                if (dist < maxYDist) {
                    other.SendMessage("GotHit", damage);
                }
            }
        }
        public void AttackableDied() {
            // don't hit anything while we're dying
            Destroy(gameObject);
        }
        public bool IsAttacking() {
            return started != -1;
        }
    }
}