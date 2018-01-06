using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float health = 3f;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health == 0) {
			if (gameObject.tag == "Enemy") {
				EnemyController.playerAttackCount--;
			}
			Destroy(gameObject);
		}
	}

	public float TakeDamage(float damage) {
		health -= damage;
		return health;
	}
}
