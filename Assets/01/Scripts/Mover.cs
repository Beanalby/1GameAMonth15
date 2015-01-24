﻿using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Mover: MonoBehaviour {

        private float groundDampening = 20f;

        private float maxSpeed = 10;
        private CharacterController2D cc;

        void Start() {
            cc = GetComponent<CharacterController2D>();
        }

        public void Move(float x, float y) {
            Vector3 newV = cc.velocity;
            newV.x = Mathf.Lerp(newV.x, x * maxSpeed, Time.fixedDeltaTime * groundDampening);
            newV.y = Mathf.Lerp(newV.y, y * maxSpeed, Time.fixedDeltaTime * groundDampening);
            cc.velocity = newV;

            // apply the new velocity to our position
            Vector3 delta = cc.velocity * Time.deltaTime;
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