using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BuildingController.instance.currentPeople += 3;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDestroy() {
		BuildingController.instance.currentPeople--;
		
	}
}
