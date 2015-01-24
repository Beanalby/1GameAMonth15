﻿using UnityEngine;
using System.Collections;

namespace onegam_1501 {
    public class Attackable: MonoBehaviour {

        public void GotHit(float damage) {
            Debug.Log("+++ " + name + " got hit!");
            SendMessageUpwards("AttackableDied", this, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}