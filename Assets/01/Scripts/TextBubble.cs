﻿using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    [RequireComponent(typeof(AudioSource))]
    public class TextBubble: MonoBehaviour {

        public string testText;
        public SpriteRenderer textLine;
        public bool CanDismiss = true;

        private float maxCamDistance = 9f;
        private Speaker speaker;
        private GameObject caller;
        private string text = "";
        private TextMesh tm;
        private float displayStart= -1;
        private float displaySpeed = 10;

        public void Awake() {
            tm = GetComponentInChildren<TextMesh>();
        }

        public void Display(Speaker newSpeaker, string newText, GameObject newCaller=null) {
            displayStart = Time.time;
            text = newText.Replace("\\n", "\n");
            EnableBubble();
            caller = newCaller;
            speaker = newSpeaker;
            GetComponent<AudioSource>().clip = speaker.speakSound;
            GetComponent<AudioSource>().loop = true;
            GetComponent<AudioSource>().Play();
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
                pos.x = speaker.transform.position.x;
                float camX = Camera.main.transform.position.x;
                pos.x = Mathf.Min(camX + maxCamDistance, pos.x);
                transform.position = pos;

                if (textLine) {
                    textLine.transform.position = new Vector3(
                        speaker.transform.position.x,
                        textLine.transform.position.y,
                        textLine.transform.position.z);
                }
            }
        }

        private void UpdateText() {
            if (GetComponent<Renderer>().enabled && Input.GetButtonDown("Jump")) {
                if (displayStart != -1) {
                    // force displaying it all
                    displayStart = -100;
                } else {
                    if (CanDismiss) {
                        DisableBubble();
                        if (caller) {
                            caller.SendMessage("BubbleDone", this);
                            speaker = null;
                        }
                    }
                }
            }
            if (displayStart != -1) {
                int currentNum = Mathf.Min((int)((Time.time - displayStart) * displaySpeed), text.Length);
                tm.text = text.Substring(0, currentNum);
                if (currentNum >= text.Length) {
                    displayStart = -1;
                    GetComponent<AudioSource>().loop = false;
                }
            }
        }

        private void EnableBubble() {
            tm.GetComponent<Renderer>().enabled = true;
            GetComponent<Renderer>().enabled = true;
            if (textLine) {
                textLine.enabled = true;
            }
        }

        private void DisableBubble() {
            tm.GetComponent<Renderer>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            if (textLine) {
                textLine.enabled = false;
            }
       }
    }
}