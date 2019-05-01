using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPartGhost : MonoBehaviour {
	public GameObject wallPartPrefab;

    public void Build(GameObject parent) {
		GameObject instance = Instantiate(wallPartPrefab, transform.position, transform.localRotation);
		instance.transform.parent = parent.transform;
	}
}
