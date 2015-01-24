﻿using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Player: MonoBehaviour {

        public AttackEffect attacker;

        private float moveSpeed = 10;
        private CharacterController2D cc;

        // Use this for initialization
        void Start() {
            cc = GetComponent<CharacterController2D>();
        }

        public void Update() {
            HandleAttack();
        }

        // Update is called once per frame
        void FixedUpdate() {
            HandleMovement();
        }

        void HandleAttack() {
            if (Input.GetButtonDown("Jump")) {
                //s top moving when attacking
                cc.velocity = Vector3.zero;
                attacker.gameObject.SetActive(true);
            }
        }

        void HandleMovement() {
            Vector3 delta = new Vector3(
                moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal"),
                moveSpeed * Time.deltaTime * Input.GetAxis("Vertical"),
                0);
            
            // flip ourselves if our direction changed
            if ((delta.x > 0 && transform.localScale.x < 0f)
                    || (delta.x < 0 && transform.localScale.x > 0f)) {
                transform.localScale = new Vector3(
                    -transform.localScale.x,
                    transform.localScale.y,
                    transform.localScale.z);
            }
            //cap delta based on our movement restrictions
            if (delta.y > 0) {
                delta.y = Mathf.Min(Stage.yMax - transform.position.y, delta.y);
            } else {
                delta.y = Mathf.Max(Stage.yMin - transform.position.y, delta.y);
            }
            cc.move(delta);
        }
    }
}