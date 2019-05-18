using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPartGhost : MonoBehaviour {
	public GameObject wallPartPrefab;

    public GameObject Build(GameObject parent) {
			GameObject instance = Instantiate(wallPartPrefab, Vector3.zero, Quaternion.identity);
			instance.transform.parent = parent.transform;
			instance.transform.localPosition = transform.localPosition;
			instance.transform.localRotation = transform.localRotation;
			return instance;
	}
}
