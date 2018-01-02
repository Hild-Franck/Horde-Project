using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject player;
	public GameObject RTS;

	public Camera RTSCamera;
	public Camera FPSCamera;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	void Start () {
		player.SetActive(false);
		RTSCamera.enabled = true;
		FPSCamera.enabled = false;
	}
	
	void Update () {
		if (Input.GetButtonDown("SwitchMode")) {
			player.SetActive(true);
			RTS.SetActive(false);

			RTSCamera.enabled = false;
			FPSCamera.enabled = true;

			GridOverlay.instance.removeGhost();
		}
	}
}
