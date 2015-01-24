using UnityEngine;
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
            //if (Input.GetAxis("Horizontal") > 0) {
            //    Debug.Log(Time.time + " (" + Time.deltaTime + "): " + Input.GetAxis("Horizontal"));
            //}
            mover.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        void HandleAttack() {
            if (Input.GetButtonDown("Jump")) {
                //s top moving when attacking
                attacker.gameObject.SetActive(true);
            }
        }


    }
}