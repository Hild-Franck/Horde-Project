using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

	public static BuildingController instance = null;

	public int maxBuildings = 2;
	private int currentBuildings = 0;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	public void AddBuilding() {
		currentBuildings++;
	}

	public bool Check() {
		return (maxBuildings != currentBuildings);
	}
}
