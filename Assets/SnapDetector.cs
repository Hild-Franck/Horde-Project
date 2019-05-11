using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapDetector : MonoBehaviour {
  private GhostClosingWall closingWall;
  private bool detected = false;

  void Start() {
		closingWall = transform.parent.GetComponent<GhostClosingWall>();
	}

	void Update() {
		bool wasDetected = detected;
		detected = true;
		foreach (Transform child in transform) {
			if (!child.GetComponent<SnapDetectorTrigger>().triggered) detected = false;
		}
		if (detected) {
			foreach (Transform child in transform) {
				child.GetComponent<SnapDetectorTrigger>().HideObject();
			}
		}
		if (wasDetected != detected) closingWall.ToggleGraphic();
	}

	public bool CheckSnapping() => detected;

  public void RemoveObjects() {
    foreach (Transform child in transform) {
      child.GetComponent<SnapDetectorTrigger>().RemoveObject();
    }
  }
}
