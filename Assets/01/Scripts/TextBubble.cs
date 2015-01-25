using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    public class TextBubble: MonoBehaviour {

        public string testText;
        public SpriteRenderer textLine;

        private float maxCamDistance = 7f;
        private Transform speaker;
        private GameObject caller;
        private string text = "";
        private TextMesh tm;
        private float displayStart= -1;
        private float displaySpeed = 10;

        public void Start() {
            tm = GetComponentInChildren<TextMesh>();
            tm.renderer.enabled = false;
            renderer.enabled = false;
            textLine.enabled = false;
        }

        public void Display(GameObject newSpeaker, string newText, GameObject newCaller=null) {
            displayStart = Time.time;
            text = newText.Replace("\\n", "\n");
            renderer.enabled = true;
            tm.renderer.enabled = true;
            textLine.enabled = true;
            caller = newCaller;
            speaker = newSpeaker.transform;
            UpdatePosition();
        }

        public void Update() {
            UpdateText();
            UpdatePosition();
        }

        private void UpdatePosition() {
            if (speaker) {
                // if it's too far from the camera (some of it offscreen), fix it
                Vector3 pos = transform.position;
                pos.x = speaker.position.x;
                float camX = Camera.main.transform.position.x;
                pos.x = Mathf.Min(camX + maxCamDistance, pos.x);
                transform.position = pos;

                textLine.transform.position = new Vector3(
                    speaker.position.x,
                    textLine.transform.position.y,
                    textLine.transform.position.z);
            }
        }

        private void UpdateText() {
            if (renderer.enabled && Input.GetButtonDown("Jump")) {
                if (displayStart != -1) {
                    // force displaying it all
                    displayStart = -100;
                } else {
                    tm.renderer.enabled = false;
                    renderer.enabled = false;
                    textLine.enabled = false;
                    if (caller) {
                        caller.SendMessage("BubbleDone", this);
                        speaker = null;
                    }
                }
            }
            if (displayStart != -1) {
                int currentNum = Mathf.Min((int)((Time.time - displayStart) * displaySpeed), text.Length);
                tm.text = text.Substring(0, currentNum);
                if (currentNum >= text.Length) {
                    displayStart = -1;
                }
            }
        }
    }
}