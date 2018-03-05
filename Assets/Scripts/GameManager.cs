using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject player;
	public GameObject RTS;
	public GameObject playerSpawn;

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
			SwitchToFPS();
		}

		if (Input.GetButtonDown("Escape")) {
			// UnityEditor.EditorApplication.isPlaying = false;
			Application.Quit();
		}

		if (FPSModeOn && EnemyController.enemyCount == 0 && !SpawnController.instance.isSpawning) {
			if (SpawnController.instance.waveCount % 3 == 0) {
				SwitchToRTS();
			} else {
				SpawnController.instance.StartWave();
			}
		}
	}

	void SwitchToFPS() {
		player.transform.position = new Vector3(playerSpawn.transform.position.x, player.transform.position.y, playerSpawn.transform.position.z);
		Cursor.lockState = CursorLockMode.Locked;
		player.SetActive(true);
		RTS.SetActive(false);

		RTSCamera.enabled = false;
		FPSCamera.enabled = true;

		GridOverlay.instance.removeGhost();
		SpawnController.instance.StartWave();
		FPSModeOn = true;
		playerSpawn.SetActive(false);
	}

	void SwitchToRTS() {
		Cursor.lockState = CursorLockMode.None;
		player.SetActive(false);
		RTS.SetActive(true);

		RTSCamera.enabled = true;
		FPSCamera.enabled = false;

		GridOverlay.instance.ActivateGhost();
		FPSModeOn = false;
		playerSpawn.SetActive(true);
	}
}
