using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRamp : Ghost {
  public GameObject building;
  private RampSnapDetector snapDetector;

  void Start() {
    currentRotation = transform.rotation;
    graphics = transform.GetChild(0);
    buildingDetector = transform.GetComponentInChildren<BuildingDetector>();
    snapDetector = GetComponentInChildren<RampSnapDetector>();
  }

  public override void Build(Vector3 coord) {
    if (!isColliding) {
      snapDetector.RemoveObjects();
			Instantiate(building, coord, transform.rotation);
    }
  }

  public override void Cancel() { }
}
