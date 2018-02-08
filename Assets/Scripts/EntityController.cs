using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float startHealth = 3f;
	public float health;
	public Animator swordAnimation;

	public bool isGuarding = false;
	public bool isAttacking = false;
	public bool isHit = false;

	[Header("Unity Stuff")]
	public Image healthBar;

	private Material material;
	private bool canFlash = false;

	void Start() {
		GameObject graphic = transform.Find("Graphic").gameObject;
		Transform meshComponent = graphic.transform.Find("Mesh");
		if (meshComponent != null) {
			canFlash = true;
			material = meshComponent.gameObject.GetComponent<Renderer>().material;
		}

		health = startHealth;
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
			if (canFlash) StartCoroutine(flashWhite());
			health -= damage;
			healthBar.fillAmount = health / startHealth;
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
		isHit = true;
		Color color = material.color;
		material.color = Color.white;
		yield return new WaitForSeconds(0.1f);
		material.color = color;
		isHit = false;
	}
}
