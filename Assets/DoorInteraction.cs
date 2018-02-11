using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour {

	public float interactionRadius = 1f;

	private bool isClosed = true;
	private GameObject player;
	private Vector3 interactionCenter;
	private Vector3 openPosition;
	private Vector3 closePosition;

	// Use this for initialization
	void Start () {
		player= GameObject.Find("Player");
		interactionCenter = transform.position;
		openPosition = new Vector3(transform.position.x, 2.14f, transform.position.z);
		closePosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		float playerDistance = Vector3.Distance(player.transform.position, interactionCenter);

		if (playerDistance <= interactionRadius && Input.GetButtonDown("Interact")) {
			Interact();
		}

	}

	void Interact() {
		if (isClosed) {
			Debug.Log("Opening Door");
			transform.position = openPosition;
			isClosed = false;
		} else {
			Debug.Log("Closing Door");
			transform.position = closePosition;
			isClosed = true;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(interactionCenter, interactionRadius);
	}
}
