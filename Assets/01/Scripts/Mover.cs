using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Mover: MonoBehaviour {

        public Sprite stationary, walk1, walk2, attack;

        [HideInInspector]
        public bool CanControl = true;

        public float maxSpeed = 10;

        private float groundDampening = 20f;
        private float hitStart = -1, hitDuration = .5f;
        private Color hitColor = Color.red, normalColor = Color.white;
        private float deathStart = -1, deathTarget = 90, deathDuration=.5f;

        private CharacterController2D cc;
        private SpriteRenderer sprite;
        private float stopStart=-1, stopDuration;
        private Vector3 forceTarget = Vector3.zero;
        private Section forceSection = null;
        private float animSpeed = .15f;
        private float animLastFlip = -1f;
        private bool isFlipped = false;

        public void Start() {
            cc = GetComponent<CharacterController2D>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            isFlipped = transform.localRotation.y != 0;
        }
        public void Update() {
            Animate();
        }

        public void Animate() {
            if (deathStart != -1) {
                sprite.sprite = stationary;
            } else {
                // choose the right sprite based on our state
                if (stopStart != -1) {
                    sprite.sprite = attack;
                } else {
                    if (cc.velocity == Vector3.zero) {
                        sprite.sprite = stationary;
                    } else {
                        if (Time.time - animLastFlip > animSpeed) {
                            if (sprite.sprite == walk1) {
                                sprite.sprite = walk2;
                            } else {
                                sprite.sprite = walk1;
                            }
                            animLastFlip = Time.time;
                        }
                    }
                }
            }
            // set sprite color if we've been hit recently
            if (hitStart == -1) {
                sprite.color = normalColor;
            } else {
                float percent = (Time.time - hitStart) / hitDuration;
                if (percent >= 1) {
                    sprite.color = normalColor;
                    hitStart = -1;
                } else {
                    sprite.color = Color.Lerp(hitColor, normalColor, percent);
                }
            }
        }

        public void Move(float x, float y) {
            // if we're dying, just apply the rotation
            if (deathStart != -1) {
                float percent = (Time.time - deathStart) / deathDuration;
                if (percent >= 1) {
                    Destroy(gameObject);
                    return;
                } else {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0,
                        Mathf.Lerp(0, deathTarget, percent)));
                }
                return;
            }

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
            if (isFlipped) {
                // reverse our input to match the character's reversal
                x = -x;
            }
            newV.x = Mathf.Lerp(newV.x, x * maxSpeed, Time.fixedDeltaTime * groundDampening);
            newV.y = Mathf.Lerp(newV.y, y * maxSpeed, Time.fixedDeltaTime * groundDampening);
            cc.velocity = newV;

            // apply the new velocity to our position
            Vector3 delta = cc.velocity * Time.deltaTime;
            //cap delta based on our movement restrictions
            if (delta.y > 0) {
                delta.y = Mathf.Min(Stage.yMax - transform.position.y, delta.y);
            } else {
                delta.y = Mathf.Max(Stage.yMin - transform.position.y, delta.y);
            }
            cc.move(delta);

            // flip ourselves if our direction changed
            if (delta.x < 0) {
                Quaternion newRotation;
                if (isFlipped) {
                    newRotation = Quaternion.identity;
                } else {
                    newRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                }
                transform.localRotation = newRotation;
                isFlipped = !isFlipped;
            }
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

        public void GotHit(float damage) {
            hitStart = Time.time;
        }
        public void AttackableDied() {
            CanControl = false;
            deathStart = Time.time;
            if (isFlipped) {
                deathTarget = -deathTarget;
            }
        }
   }
}