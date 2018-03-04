using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackController : MonoBehaviour {

	public float upSpeed;
	public float destroyTime;
	public float spawnTime;
	public int maxCount;
	public float distanceBetweenSpawns;
	public GameObject specialAttack;

	private bool spawned = false;
	private Vector3 startingPoint;
	private Vector3 finishingPoint;
	private float startTime;
	private float distance;
	private int spawnNumber;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		startingPoint = transform.position;
		finishingPoint = new Vector3(transform.position.x, 0.15f, transform.position.z);
		distance = Vector3.Distance(startingPoint, finishingPoint);
	}
	
	// Update is called once per frame
	void Update () {
		Up();

		if (Time.time > startTime + destroyTime) {
			Destroy(gameObject);
		}

		if (Time.time > spawnTime + startTime && spawnNumber < maxCount && !spawned) {
			Spawn();
		}
	}

	void Up() {
		float distanceDone = (Time.time - startTime) / upSpeed;
		float completion = distanceDone / distance;
		transform.position = Vector3.Lerp(startingPoint, finishingPoint, completion);
	}

	public void SetNumber(int _spawnNumber) {
		spawnNumber = _spawnNumber;
	}

	void Spawn() {
		Vector3 position = new Vector3(transform.position.x, -0.5f, transform.position.z);
		GameObject attack = Instantiate (specialAttack, position + (transform.forward * distanceBetweenSpawns), transform.rotation);
		SpecialAttackController attackController = attack.GetComponent<SpecialAttackController>();
		attackController.SetNumber(spawnNumber + 1);
		spawned = true;
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Enemy") {
			(col.gameObject.GetComponent<EntityController>()).TakeDamage(2);
		}
	}
}
