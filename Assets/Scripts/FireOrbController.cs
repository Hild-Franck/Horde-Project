using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrbController : MonoBehaviour {

	public GameObject fireBall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Instantiate(fireBall, transform.position, transform.rotation);
		}
	}
}
