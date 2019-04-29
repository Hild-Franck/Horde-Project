using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEndDetector : MonoBehaviour {

	private bool isColliding = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Building") isColliding = true;
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Building") isColliding = true;
	}

	public void SnapToCursor(Vector3 cursorPosition) {
		transform.position = cursorPosition;
	}
}
