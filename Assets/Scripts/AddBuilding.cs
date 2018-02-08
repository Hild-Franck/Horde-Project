using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBuilding : MonoBehaviour {

	public GameObject building;
	public Transform buildingToDefend;
	public Color green;
	public float constructionDistance;

	private Vector3 currentPosition;
	private Vector3 lastPosition;
	private Color red = new Color(0.25f, 0f, 0f, 0.5f);
	static bool haveCollided = false;

	void Start () {
		currentPosition = transform.position;
	}

	void Update () {
		lastPosition = currentPosition;
		currentPosition = transform.position;
		float distance = Vector3.Distance(transform.position, buildingToDefend.position);
		bool inRange = (distance < constructionDistance);
		if (!haveCollided && BuildingController.instance.Check(building) && gameObject.activeSelf && inRange) {
			ChangeColor(green);
			if (Input.GetMouseButtonDown(0)) {
				GameObject newBuilding = Instantiate(building, transform.position, transform.rotation);
				BuildingController.instance.AddBuilding(newBuilding, building);
			}
		} else {
			ChangeColor(red);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Building") {
			haveCollided = CheckPosition(col.transform);
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.tag == "Building") {
			haveCollided = CheckPosition(col.transform);
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.tag == "Building") {
			haveCollided = false;
		}
	}

	bool CheckPosition(Transform col) {
		return (col.position == transform.position);
	}

	void ChangeColor(Color color) {
		Renderer[] renderers = GetComponentsInChildren<Renderer>();

		foreach (Renderer renderer in renderers) {
			renderer.material.color = color;
		}
	}
}
