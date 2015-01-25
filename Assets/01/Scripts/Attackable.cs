using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    public class Attackable: MonoBehaviour {
        private float maxHealth = 50;
        private float currentHealth;

        public void Start() {
            currentHealth = maxHealth;
        }
        public void GotHit(float damage) {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            if (currentHealth == 0) {
                SendMessageUpwards("AttackableDied", this, SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject);
            }
        }
    }
}