using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBuildingController : MonoBehaviour {

	void OnDestroy() {
		BuildingController.instance.RemoveBuilding(gameObject);
	}
}
