using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

	public EntityController entityController;

	private Vector3 startingPosition;
	private Quaternion startingRotation;

	void Start() {
		startingPosition = transform.localPosition;
		startingRotation = transform.localRotation;
	}

	public void Reset() {
		transform.localPosition = startingPosition;
		transform.localRotation = startingRotation;
	}

	void OnTriggerEnter(Collider col) {
		string tag = col.gameObject.tag;
		if ((tag == "Enemy" || tag == "Building" || tag == "Player") && entityController.CheckAttack()) {
			entityController.IncreaseComboCount();
			(col.gameObject.GetComponent<EntityController>()).TakeDamage(entityController.damage);
		}
	}
}
