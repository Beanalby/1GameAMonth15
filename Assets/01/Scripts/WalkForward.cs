using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(Mover))]
    public class WalkForward: MonoBehaviour {

        public Material skybox;
        public TextBubble message;
        public string messageText;
        private Mover mover;

        public void Start() {
            if(skybox) {
                RenderSettings.skybox = skybox;
            }
            mover = GetComponent<Mover>();
            if (message) {
                message.Display(Camera.main.gameObject, messageText, null);
            }
        }

        public void FixedUpdate() {
            mover.Move(1, 0);
        }
    }
}