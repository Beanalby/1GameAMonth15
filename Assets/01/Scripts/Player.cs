﻿using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Attackable))]
    public class Player: MonoBehaviour {

        public AttackEffect attacker;
        private Mover mover;

        public void Start() {
            mover = GetComponent<Mover>();
        }

        public void Update() {
            HandleAttack();
        }

        // Update is called once per frame
        void FixedUpdate() {
            mover.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        void HandleAttack() {
            if (Input.GetButtonDown("Jump")) {
                //s top moving when attacking
                if (!attacker.gameObject.activeSelf) {
                    attacker.gameObject.SetActive(true);
                    mover.Stop(.1f);
                }
            }
        }
    }
}