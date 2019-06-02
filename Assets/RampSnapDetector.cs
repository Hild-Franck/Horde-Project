using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampSnapDetector : MonoBehaviour {
	public GameObject objectToReplace;
  private bool detected = false;

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
  }

  public bool CheckSnapping() => detected;

  public void ReplaceObjects() {
    foreach (Transform child in transform) {
      Spike spike = child.GetComponent<SnapDetectorTrigger>().GetObjectToRemove();
			GameObject newObject = Instantiate(objectToReplace);
			newObject.transform.parent = spike.transform.parent;
			newObject.transform.position = spike.transform.position;
      child.GetComponent<SnapDetectorTrigger>().RemoveObject();
    }
  }

  public bool CheckDetected() => detected;
}
