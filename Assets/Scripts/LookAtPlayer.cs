﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

	private GameObject player = null;

	void Update () {
		if (player == null) {
			player = GameObject.Find("Camera");
		} else {
		transform.LookAt(player.transform);
		Quaternion rot = transform.rotation;
		rot.eulerAngles = new Vector3(0, rot.eulerAngles.y, rot.eulerAngles.z);
		transform.rotation = rot;
		}
	}
}
