using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBuilding : MonoBehaviour {

	public GameObject building;

	private Vector3 colPosition;
	private bool haveCollided = false;

	void Update () {
		if (Input.GetMouseButtonDown(0) && checkPosition() && BuildingController.instance.Check(building) && gameObject.activeSelf) {
			Debug.Log("Here");
			Instantiate(building, transform.position, transform.rotation);
			BuildingController.instance.AddBuilding(building);
		}

		if (Input.GetButtonDown("Rotate")) {
			transform.Rotate(0f, 90f, 0f);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Building") {
			haveCollided = true;
			colPosition = col.gameObject.transform.position;
		}
	}

	bool checkPosition() {
		return (!haveCollided || !(colPosition.x == transform.position.x && colPosition.z == transform.position.z));
	}
}
