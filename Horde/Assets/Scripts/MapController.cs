using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
	public float cellSize = 32;
	public float xCell = 10f;
	public float yCell = 10f;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(xCell, yCell, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
