using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

	public EntityController entityController;

	void OnTriggerEnter(Collider col) {
		Debug.Log(col.gameObject);
		if (col.gameObject.tag == "Enemy" && entityController.CheckAttack()) {
			(col.gameObject.GetComponent<EntityController>()).TakeDamage(1);
		}
	}
}
