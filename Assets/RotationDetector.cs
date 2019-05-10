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
		if (detected) {
      foreach (Transform child in transform) {
        child.GetComponent<RotationDetectorTrigger>().HideObject();
      }	
		}
		if (wasDetected != detected) closingWall.ToggleGraphic();
	}

	public bool CheckRotation() => detected;

	public void RemoveObjects() {
    foreach (Transform child in transform)
    {
      child.GetComponent<RotationDetectorTrigger>().RemoveObject();
    }
  }
}
