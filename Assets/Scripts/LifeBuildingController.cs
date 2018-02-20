using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBuildingController : MonoBehaviour {

	void Start() {
		BuildingController.instance.currentPeople -= 2;

	}

	void OnDestroy() {
		BuildingController.instance.RemoveBuilding(gameObject);
	}
}
