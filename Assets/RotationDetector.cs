using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDetector : MonoBehaviour {
	public GhostClosingWall closingWall;
	private bool detected = false;
	void Update() {
		bool wasDetected = detected;
		detected = true;
		foreach (Transform child in transform) {
			if (!child.GetComponent<RotationDetectorTrigger>().triggered) detected = false;
		}
	}

	public bool CheckRotation() => detected;

	public void RemoveObjects() {
    foreach (Transform child in transform) {
      child.GetComponent<RotationDetectorTrigger>().RemoveObject();
    }
  }

	public void Reset() {
		detected = false;
	}
}
