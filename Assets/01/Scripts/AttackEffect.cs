using UnityEngine;
using System.Collections;

public class AttackEffect : MonoBehaviour {

    public float damage= 10;

    private float maxYDist = 1f;

    private float started, duration=.25f;
    public bool hitEnemy;
    private int targetLayer;

    public void Start() {
        gameObject.SetActive(false);
    }

    public void OnEnable() {
        started = Time.time;
        if (hitEnemy) {
            targetLayer = LayerMask.NameToLayer("enemy");
        } else {
            targetLayer = LayerMask.NameToLayer("player");
        }
    }
    
    public void Update() {
        if ((Time.time - started) > duration) {
            gameObject.SetActive(false);
        }
    }
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == targetLayer) {
            // even if the trigger hit, make sure our y coord isn't too
            // far away, or we'll consider them far apart in our fake
            // 2d engine.
            float dist = Mathf.Abs(transform.parent.position.y - other.transform.position.y);
            if (dist < maxYDist) {
                other.SendMessage("GotHit", damage);
            }
        }
    }
}
