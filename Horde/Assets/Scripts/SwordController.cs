using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

	public EntityController entityController;

	void OnTriggerEnter(Collider col) {
			Debug.Log(entityController.isAttacking);
		if (col.gameObject.tag == "Enemy" && entityController.isAttacking) {
			(col.gameObject.GetComponent<EntityController>()).TakeDamage(1);
		}
	}
}
