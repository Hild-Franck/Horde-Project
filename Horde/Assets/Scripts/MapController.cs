using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
	public float cellSize = 32;
	public int xCell = 10;
	public int yCell = 10;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3((float) xCell, (float) yCell, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
