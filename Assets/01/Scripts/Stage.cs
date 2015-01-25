using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1501 {
    [RequireComponent(typeof(AudioSource))]
    public class Stage: MonoBehaviour {
        public const float yMax = -.5f;
        public const float yMin = -13;

        public Material skybox;
        public string stageDescription;
        public Section[] sections;
        public TextBubble Message;
        public TextBubble textBubble;
        public Texture2D finishTexture;
        private Texture2D displayedImage = null;

        private Player player;

        private static Stage _instance = null;
        public static Stage Instance {
            get { return _instance; }
        }
        public void Awake() {
            if (_instance != null) {
                Debug.LogError("Can't have more than one stage");
                Destroy(gameObject);
            }
            _instance = this;
        }

        public void Start() {
            if (skybox) {
                RenderSettings.skybox = skybox;
            }
            player = GameObject.FindObjectOfType<Player>();
            StartCoroutine(StartStage());
        }

        public void OnGUI() {
            if (displayedImage != null) {
                GUI.DrawTexture(
                    new Rect(
                        Screen.width / 2 - displayedImage.width / 2,
                        50,
                        displayedImage.width,
                        displayedImage.height),
                    displayedImage);

            }
        }

        private IEnumerator StartStage() {
            player.CanControl = false;
            CinemaBars.Instance.ShowCinemaBars();
            yield return new WaitForSeconds(1);
            Message.Display(Camera.main.GetComponent<Speaker>(), stageDescription, gameObject);
        }

        public void BubbleDone() {
            // stageDescription's done, activate the first section
            CinemaBars.Instance.HideCinemaBars();
            sections[0].Activate();
            audio.Play(); // start the bg music
        }

        public void SectionDone(Section section) {
            int index = Array.IndexOf(sections, section);
            if (index == sections.Length - 1) {
            } else {
                index++;
                sections[index].Activate();
            }
        }

        public void StageDone(StageEnder ender) {
            StartCoroutine(_stageDone(ender));
        }

        private IEnumerator _stageDone(StageEnder ender) {
            // stop the player and all active enemies
            player.CanControl = false;
            foreach(Mover m in GameObject.FindObjectsOfType<Mover>()) {
                m.CanControl = false;
            }
            displayedImage = finishTexture;
            yield return new WaitForSeconds(3);
            Application.LoadLevel(ender.nextLevel);
        }

        public TextBubble GetBubble() {
            return textBubble;
        }
    }
}