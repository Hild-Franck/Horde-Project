using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float health = 3f;
	public Animator swordAnimation;

	public bool isGuarding = false;

	void Update () {
		if (health == 0) {
			if (gameObject.tag == "Enemy") {
				EnemyController.playerAttackCount--;
			}
			Destroy(gameObject);
		}
	}

	public float TakeDamage(float damage) {
		if (!isGuarding) {
			health -= damage;
			return health;
		} else {
			Debug.Log("Guarding !");
			return health;
		}
	}

	public void Guard() {
		swordAnimation.SetTrigger("Guarding");
		isGuarding = true;
	}

	public void Unguard() {
		swordAnimation.SetBool("Guarding", false);
		isGuarding = false;
	}
}
