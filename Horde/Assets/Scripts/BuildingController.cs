using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

	public static BuildingController instance = null;

	public int maxBuildings = 5;
	public int maxWalls = 25;
	private int currentBuildings = 0;
	private int currentWalls = 0;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	public void AddBuilding(GameObject building) {
		currentWalls++;
		if (building.name == "Building")
			currentBuildings++;
	}

	public bool Check(GameObject building) {
		if (building.name == "Building")
			return (currentWalls < maxWalls && currentBuildings < maxBuildings);
		return (currentWalls < maxWalls);
	}
}
