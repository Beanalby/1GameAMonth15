using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1501 {
    public class GameUI: MonoBehaviour {
        public Texture2D heartEmpty, heartFull;

        private Attackable player;

        public void Start() {
            player = GameObject.FindObjectOfType<Player>().GetComponent<Attackable>();
        }

        public void OnGUI() {
            DrawHealth();
        }

        private void DrawHealth() {
            float pad=10, heartSz = heartEmpty.width / 4;
            for (int i = 0; i < player.MaxHealth; i++) {
                Rect heartRect = new Rect(pad + i * (pad+heartSz), pad, heartSz, heartSz);

                if (player.CurrentHealth > i) {
                    GUI.DrawTexture(heartRect, heartFull);
                } else {
                    GUI.DrawTexture(heartRect, heartEmpty);
                }
            }
        }
    }
}