using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(CharacterController2D))]
    public class Player: MonoBehaviour {

        private float moveSpeed = 5;

        private CharacterController2D cc;

        // Use this for initialization
        void Start() {
            cc = GetComponent<CharacterController2D>();
        }

        // Update is called once per frame
        void Update() {
            cc.move(new Vector3(
                moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal"),
                moveSpeed * Time.deltaTime * Input.GetAxis("Vertical"),
                0));
        }
    }
}