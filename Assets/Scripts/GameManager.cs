using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject player;
	public GameObject RTS;

	public Camera RTSCamera;
	public Camera FPSCamera;

	private bool FPSModeOn = false;

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
		if (Input.GetButtonDown("SwitchMode") && !FPSModeOn) {
			Cursor.lockState = CursorLockMode.Locked;
			player.SetActive(true);
			RTS.SetActive(false);

			RTSCamera.enabled = false;
			FPSCamera.enabled = true;

			GridOverlay.instance.removeGhost();
			SpawnController.instance.StartWave();
			FPSModeOn = true;

		}

		if (Input.GetButtonDown("Escape")) {
			UnityEditor.EditorApplication.isPlaying = false;
			Application.Quit();
		}

		Debug.Log(EnemyController.enemyCount);
		if (FPSModeOn && EnemyController.enemyCount == 0) {
			SpawnController.instance.StartWave();
		}
	}
}
