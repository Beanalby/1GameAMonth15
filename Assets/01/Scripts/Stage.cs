using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1501 {
    public class Stage: MonoBehaviour {
        public const float yMax = -.5f;
        public const float yMin = -13;

        public string stageDescription;
        public Section[] sections;
        public TextBubble Message;
        public TextBubble textBubble;

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
            player = GameObject.FindObjectOfType<Player>();
            StartCoroutine(StartStage());
        }

        private IEnumerator StartStage() {
            player.CanControl = false;
            CinemaBars.Instance.ShowCinemaBars();
            yield return new WaitForSeconds(1);
            Message.Display(Camera.main.gameObject, stageDescription, gameObject);
        }

        public void BubbleDone() {
            // stageDescription's done, activate the first section
            CinemaBars.Instance.HideCinemaBars();
            sections[0].Activate();
        }

        public void SectionDone(Section section) {
            int index = Array.IndexOf(sections, section);
            if (index == sections.Length - 1) {
                Debug.Log("All Done!");
            } else {
                index++;
                Debug.Log("Activating next section " + index);
                sections[index].Activate();
            }
        }

        public TextBubble GetBubble() {
            return textBubble;
        }
    }
}