using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float health = 3f;
	public Animator swordAnimation;

	public bool isGuarding = false;
	public bool isAttacking = false;

	private Material material;

	void Start() {
		GameObject graphic = transform.Find("Graphic").gameObject;
		GameObject mesh = graphic.transform.Find("Mesh").gameObject;
		material = mesh.GetComponent<Renderer>().material;
	}

	void Update () {
		if (health == 0) {
			if (gameObject.tag == "Enemy") {
				EnemyController.playerAttackCount--;
			}
			Destroy(gameObject);
		}

		if (isAttacking && !swordAnimation.GetBool("Attacking")) {
			isAttacking = false;
		}
	}

	public float TakeDamage(float damage) {
		if (!isGuarding) {
			StartCoroutine(flashWhite());
			health -= damage;
			return health;
		} else {
			Debug.Log("Guarding !");
			return health;
		}
	}

	public bool CheckAttack() {
		return (isAttacking && swordAnimation.GetBool("Attacking"));
	}

	public void Guard() {
		swordAnimation.SetTrigger("Guarding");
		isGuarding = true;
	}

	public void Unguard() {
		swordAnimation.SetBool("Guarding", false);
		isGuarding = false;
	}

	public void Attack() {
		swordAnimation.SetTrigger("Attacking");
		isAttacking = true;
	}

	IEnumerator flashWhite() {
		Color color = material.color;
		material.color = Color.white;
		yield return new WaitForSeconds(0.1f);
		material.color = color;
	}
}
