using UnityEngine;
using System.Collections;

public class CinemaBars : MonoBehaviour {

    public static CinemaBars Instance {
        get { return _instance; }
    }
    private static CinemaBars _instance = null;
    public void Awake() {
        if (_instance != null) {
            Debug.LogError("Can only have one CinemaBars");
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private float hidePos = 13, showPos = 11;
    private float moveStart = -1, moveDuration = .25f;

    public Transform topBar, bottomBar;
    private Vector3 topStart, topFinish, bottomStart, bottomFinish;

    public void Update() {
        UpdatePosition();
    }

    private void UpdatePosition() {
        if (moveStart == -1) {
            return;
        }
        float percent = (Time.time - moveStart) / moveDuration;
        if (percent >= 1) {
            topBar.localPosition = topFinish;
            bottomBar.localPosition = bottomFinish;
            moveStart = -1;
        } else {
            topBar.localPosition = Vector3.Lerp(topStart, topFinish, percent);
            bottomBar.localPosition = Vector3.Lerp(bottomStart, bottomFinish, percent);
        }
    }

    public void ShowCinemaBars() {
        moveStart = Time.time;
        topStart = new Vector3(topBar.localPosition.x, hidePos, topBar.localPosition.z);
        topFinish = new Vector3(topBar.localPosition.x, showPos, topBar.localPosition.z);
        bottomStart = new Vector3(bottomBar.localPosition.x, -hidePos, bottomBar.localPosition.z);
        bottomFinish = new Vector3(bottomBar.localPosition.x, -showPos, bottomBar.localPosition.z);
    }

    public void HideCinemaBars() {
        moveStart = Time.time;
        topStart = new Vector3(topBar.localPosition.x, showPos, topBar.localPosition.z);
        topFinish = new Vector3(topBar.localPosition.x, hidePos, topBar.localPosition.z);
        bottomStart = new Vector3(bottomBar.localPosition.x, -showPos, bottomBar.localPosition.z);
        bottomFinish = new Vector3(bottomBar.localPosition.x, -hidePos, bottomBar.localPosition.z);
    }
}
