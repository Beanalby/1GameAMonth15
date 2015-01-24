using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Mover: MonoBehaviour {

        [HideInInspector]
        public bool CanControl = true;

        public float maxSpeed = 10;

        private float groundDampening = 20f;

        private CharacterController2D cc;
        private float stopStart=-1, stopDuration;
        private Vector3 forceTarget = Vector3.zero;
        private Section forceSection = null;

        void Start() {
            cc = GetComponent<CharacterController2D>();
        }

        public void Move(float x, float y) {
            // if we can't control at all, don't allow movement
            // (unless we're forcing movement)
            if (!CanControl && forceTarget == Vector3.zero) {
                return;
            }
            // if we're stopped, don't allow movement
            if (stopStart != -1) {
                if (Time.time - stopStart < stopDuration) {
                    return;
                } else {
                    stopStart = -1;
                }
            }

            if (forceTarget != Vector3.zero) {
                // if we're close enough to the force target,
                // then stop.  Otherwise ignore provided x & y
                // and force moving towards it
                Vector3 dir = forceTarget - transform.position;
                if (dir.magnitude < .1f) {
                    forceTarget = Vector3.zero;
                    cc.velocity = Vector3.zero;
                    if (forceSection) {
                        forceSection.ForceMoveFinished();
                    }
                    return;
                } else {
                    dir.Normalize();
                    x = dir.x;
                    y = dir.y;
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

        /// <summary>
        /// Forces the target to move to a certain location and
        /// disables control
        /// </summary>
        /// <param name="newTarget"></param>
        public void ForceMove(Section section) {
            forceSection = section;
            forceTarget = forceSection.ForceTarget.position;
            CanControl = false;
        }
    }
}