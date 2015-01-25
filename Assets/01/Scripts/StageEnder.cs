using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    public class StageEnder: MonoBehaviour {
        public Attackable watchedObj;
        public string nextLevel;

        public void Start() {
            watchedObj.deathListeners += new Attackable.AttackableDied(OnDeath);
        }

        public void OnDeath(Attackable obj) {
            Stage.Instance.StageDone(this);
        }
    }
}