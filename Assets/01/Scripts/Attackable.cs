using System;
using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(AudioSource))]
    public class Attackable: MonoBehaviour {

        public delegate void AttackableDied(Attackable obj);
        public event AttackableDied deathListeners;

        public float CurrentHealth {
            get { return currentHealth; }
        }
        public float MaxHealth {
            get { return maxHealth; }
        }
        private float maxHealth = 3;
        private float currentHealth;

        public void Start() {
            currentHealth = maxHealth;
        }
        public void GotHit(float damage) {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            if (audio) {
                audio.Play();
            }
           if (currentHealth == 0) {
                GetComponent<BoxCollider2D>().enabled = false;
                AttackEffect ae = GetComponentInChildren<AttackEffect>();
                if (ae) {
                    ae.AttackableDied();
                }
                SendMessageUpwards("AttackableDied", this, SendMessageOptions.DontRequireReceiver);
                if (deathListeners != null) {
                    deathListeners(this);
                }
            }
        }
    }
}