using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

	public PlayerController playerController;


	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Enemy" && playerController.isAttacking) {
			(col.gameObject.GetComponent<EntityController>()).health--;
		}
	}
}
