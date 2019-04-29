using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEndDetector : MonoBehaviour {

	private bool isColliding = false;

	public bool GetIsColliding() {
		return isColliding;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Building") isColliding = true;
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Building") isColliding = true;
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Building") isColliding = false;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
