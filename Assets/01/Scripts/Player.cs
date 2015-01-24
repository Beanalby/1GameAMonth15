using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Player: MonoBehaviour {

        private float moveSpeed = 10;

        private CharacterController2D cc;

        // Use this for initialization
        void Start() {
            cc = GetComponent<CharacterController2D>();
        }

        // Update is called once per frame
        void FixedUpdate() {
            HandleMovement();
        }

        void HandleMovement() {
            Vector3 delta = new Vector3(
                moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal"),
                moveSpeed * Time.deltaTime * Input.GetAxis("Vertical"),
                0);
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