using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {

	public float upTime;
	public float coolDown;

	private bool isActive = false;
	private float startY;
	private float lastTrigger = 0f;

	// Use this for initialization
	void Start () {
		startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Enemy" && !isActive && CheckCooldown()) {
			TriggerTrap();
		}
	}

	void OnTriggerStay(Collider col) {
		if (col.tag == "Enemy" && !isActive && CheckCooldown()) {
			TriggerTrap();
		}
	}

	void TriggerTrap () {
		isActive = true;
		lastTrigger = Time.time;
		transform.position = new Vector3(transform.position.x, 0.555f, transform.position.z);

		StartCoroutine(UntriggerTrap());
	}

	bool CheckCooldown() {
		return (lastTrigger + coolDown < Time.time);
	}

	IEnumerator UntriggerTrap() {
		yield return new WaitForSeconds(upTime);

		transform.position = new Vector3(transform.position.x, startY, transform.position.z);
		isActive = false;

	}
}
