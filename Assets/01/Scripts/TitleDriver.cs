using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace onegam_1501 {
    public class TitleDriver: MonoBehaviour {
        public Material skybox;
        public Texture2D title;
        public string nextLevel;

        public void Start() {
            if (skybox) {
                RenderSettings.skybox = skybox;
            }
        }

        public void Update() {
            if (Input.GetButtonDown("Jump")) {
                SceneManager.LoadScene(nextLevel);
            }
        }
        public void OnGUI() {
            GUI.DrawTexture(new Rect(
                Screen.width / 2 - title.width / 2,
                Screen.height / 2 - title.height / 2,
                title.width,
                title.height),
                title);
        }
    }
}