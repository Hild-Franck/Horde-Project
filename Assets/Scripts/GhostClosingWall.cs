﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostClosingWall : MonoBehaviour {
	public GameObject wallEndPrefab;
	private Vector3 initialLocalPosition;

	public static GhostClosingWall Create(GameObject wallBeginningPrefab) {
		GameObject instance = Instantiate(wallBeginningPrefab, Vector3.zero, Quaternion.identity);
		return instance.GetComponent<GhostClosingWall>();
	}
    void Start() => initialLocalPosition = transform.localPosition;

    public void ResetPosition() => transform.localPosition = initialLocalPosition;

    public void Build(GameObject parent) {
		GameObject instance = Instantiate(wallEndPrefab, Vector3.zero, Quaternion.identity);
		instance.transform.parent = parent.transform;
		instance.transform.localPosition = transform.localPosition;
		instance.transform.localRotation = transform.localRotation;
	}
}
