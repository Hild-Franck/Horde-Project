using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectileController : MonoBehaviour {
	public float speed = 10f;

	private Rigidbody rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		rigidbody.velocity = transform.forward * speed;
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Enemy") {
			Destroy (col.gameObject);
		}
		Destroy (gameObject);
	}
}
