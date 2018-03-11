using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float startHealth = 3f;
	public float health;
	public float attackCooldown = 0f;
	public float comboTime;
	public float damage = 1;
	public Animator swordAnimation;

	public bool isGuarding = false;
	public bool isAttacking = false;
	public bool isHit = false;

	[Header("Unity Stuff")]
	public Image healthBar;

	private Material material;
	private bool canFlash = false;
	private int comboCount = 0;
	private float nextAttack = 0f;
	private float comboEnd;

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
		if (health <= 0) {
			if (gameObject.tag == "Enemy") {
				EnemyController.playerAttackCount--;
			}
			Destroy(gameObject);
		}

		if (isGuarding && !swordAnimation.GetBool("Guarding")) {
			isGuarding = false;
		}

		if (Time.time > comboEnd) {
			ResetCombo();
		}

	}

	public float TakeDamage(float damage) {
		if (!isGuarding) {
			comboCount = 0;
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
		return swordAnimation.GetBool("Attacking");
	}

	public void Guard() {
		swordAnimation.SetTrigger("Guarding");
		isGuarding = true;
	}

	public void Attack() {
		if (!swordAnimation.GetBool("Attacking")) {
			if (Time.time > nextAttack) {
				swordAnimation.SetTrigger("Attacking");
				isAttacking = true;
				nextAttack = attackCooldown + Time.time;
			}
		} else if (!swordAnimation.GetBool("Combo1")) {
			swordAnimation.SetTrigger("Combo1");
			nextAttack = attackCooldown + Time.time;
		} else if (!swordAnimation.GetBool("Combo2")) {
			swordAnimation.SetTrigger("Combo2");
			nextAttack = attackCooldown + Time.time;
		}
	}

	public float GetComboEnd() {
		return comboEnd;
	}

	public int GetComboCount() {
		return comboCount;
	}

	public void IncreaseComboCount() {
		comboEnd = Time.time + comboTime;
		comboCount++;
	}

	public void ResetCombo() {
		comboCount = 0;
	}

	public void Reset() {
		health = startHealth;
	}

	IEnumerator flashWhite() {
		isHit = true;
		Color color = material.color;
		material.color = Color.white;
		canFlash = false;
		yield return new WaitForSeconds(0.05f);
		canFlash = true;
		material.color = color;
		isHit = false;
	}
}
