using UnityEngine;
using System.Collections;


namespace onegam_1501 {
    public class Stage: MonoBehaviour {
        public const float yMax = -.5f;
        public const float yMin = -13;

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
    }
}