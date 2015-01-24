using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Mover: MonoBehaviour {

        public float maxSpeed = 10;

        private float groundDampening = 20f;

        private CharacterController2D cc;
        private float stopStart=-1, stopDuration;

        void Start() {
            cc = GetComponent<CharacterController2D>();
        }

        public void Move(float x, float y) {
            // if we're stopped, don't allow movement
            if (stopStart != -1) {
                if (Time.time - stopStart < stopDuration) {
                    return;
                } else {
                    stopStart = -1;
                }
            }

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

        /// <summary>
        /// Called when something wants to kill our velocity
        /// </summary>
        public void Stop(float duration) {
            cc.velocity = Vector3.zero;
            stopStart = Time.time;
            stopDuration = duration;
        }
    }
}