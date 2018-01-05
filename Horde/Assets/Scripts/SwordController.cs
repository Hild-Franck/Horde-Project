using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {


	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Enemy") {
			(col.gameObject.GetComponent<EntityController>()).health--;
		}
	}
}
