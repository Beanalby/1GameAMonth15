using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1501 {
    public class Section: MonoBehaviour {

        private List<Enemy> enemies;

        public void Start() {
            enemies = new List<Enemy>(transform.GetComponentsInChildren<Enemy>());
            foreach (Enemy e in enemies) {
                e.CanControl = false;
            }
        }

        public void Activate() {
            foreach(Enemy e in enemies) {
                e.CanControl = true;
            }
            Camera.main.GetComponent<CamFollow>().SetMaxX(transform.position.x);
        }

        public void AttackableDied(Attackable obj) {
            enemies.Remove(obj.GetComponent<Enemy>());
            Debug.Log("+++ Now there are " + enemies.Count + " enemies");
            if (enemies.Count == 0) {
                Debug.Log("Section done!");
                SendMessageUpwards("SectionDone", this);
            }
        }
    }
}