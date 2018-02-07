using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

	public static BuildingController instance = null;

	public int maxBuildings = 5;
	public int maxWalls = 25;
	public int currentBuildings = 0;
	public int currentWalls = 0;
	public EntityController playerController;

	private List<Transform> buildings = new List<Transform>();

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	public void AddBuilding(GameObject building, GameObject buildingOrigin) {
		currentWalls++;
		if (buildingOrigin.name == "Building") {
			playerController.health += 5;
			buildings.Add(building.transform);
			currentBuildings++;
		}
	}

	public void RemoveBuilding(GameObject building) {
		currentBuildings--;
		currentWalls--;
		playerController.startHealth -= 5;
		playerController.health = Mathf.Clamp(playerController.health, 0, playerController.startHealth);
		buildings.Remove(building.transform);
	}

	public bool Check(GameObject building) {
		if (building.name == "Building")
			return (currentWalls < maxWalls && currentBuildings < maxBuildings);
		return (currentWalls < maxWalls);
	}

	public List<Transform> GetBuildings() {
		return buildings;
	}
}
