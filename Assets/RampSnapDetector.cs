using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampSnapDetector : MonoBehaviour
{
  private bool detected = false;

  void Update() {
    bool wasDetected = detected;
    detected = true;
    foreach (Transform child in transform)
    {
      if (!child.GetComponent<SnapDetectorTrigger>().triggered) detected = false;
    }
    if (detected) {
      foreach (Transform child in transform) {
        child.GetComponent<SnapDetectorTrigger>().HideObject();
      }
    }
  }

  public bool CheckSnapping() => detected;

  public void RemoveObjects() {
    foreach (Transform child in transform) {
      child.GetComponent<SnapDetectorTrigger>().RemoveObject();
    }
  }
}
