using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBuilding : MonoBehaviour {

	public GameObject building;
	private Vector3 colPosition;
	private bool haveCollided = false;
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && checkPosition()) {
			Instantiate(building, transform.position, Quaternion.identity);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Building") {
			haveCollided = true;
			colPosition = col.gameObject.transform.position;
		}
	}

	bool checkPosition() {
		Debug.Log(colPosition.x);
		Debug.Log(colPosition.z);
		return (!haveCollided || !(colPosition.x == transform.position.x && colPosition.z == transform.position.z));
	}
}
