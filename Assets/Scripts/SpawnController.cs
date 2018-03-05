using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

	public static SpawnController instance = null;

	public int hazardCount = 5;
	public GameObject destructerEnemy;
	public GameObject killerEnemy;
	public GameObject buildingToDefend;
	public GameObject player;
	public float spawnWait = 5f;

	public int waveCount = 1;
	public bool isSpawning = false;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	public void StartWave() {
		StartCoroutine (SpawnWave ());
	}

	IEnumerator SpawnWave () {
		Debug.Log("Spawn wave");
		isSpawning = true;
		for (int i = 0; i < (hazardCount * waveCount); i++) {
			GameObject enemyInstance;
			if (i % 2 == 0) {
				enemyInstance = Instantiate (killerEnemy, transform.position, Quaternion.identity);
			} else {
				enemyInstance = Instantiate (destructerEnemy, transform.position, Quaternion.identity);
			}
			enemyInstance.GetComponent<EnemyController>().buildingToAttack = buildingToDefend;
			enemyInstance.GetComponent<EnemyController>().player = player;
			yield return new WaitForSeconds (spawnWait);
		}
		isSpawning = false;
		waveCount++;
	}
}
