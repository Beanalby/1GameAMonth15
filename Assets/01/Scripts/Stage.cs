using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1501 {
    public class Stage: MonoBehaviour {
        public const float yMax = -.5f;
        public const float yMin = -13;

        public Section[] sections;


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
            StartCoroutine(StartStage());
        }

        private IEnumerator StartStage() {
            yield return new WaitForSeconds(1);
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
    }
}