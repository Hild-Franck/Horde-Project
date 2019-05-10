﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : Ghost {
	void Start () {
		isWall = false;
		currentRotation = transform.rotation;
		graphics = transform.GetChild(0);
		buildingDetector = transform.GetComponentInChildren<BuildingDetector>();
	}

	public override void Build(Vector3 coord) {
		if (!isColliding) Instantiate(building, coord, transform.rotation);
	}

	public override void Cancel() {}
}
