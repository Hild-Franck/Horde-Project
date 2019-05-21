using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPart : MonoBehaviour {
	public float health;
	public int upgradeCost;
	public int upgradeLevel;

	public void StartAnimation(float waitingAnimation) {
		StartCoroutine(WaitAndAnimate(waitingAnimation));
	}

	public void TakeDamage(float damage) {
		health -= damage;
	}

	public void DestroyBuilding() {

	}

	public void Upgrade() {
		
	}

	private IEnumerator WaitAndAnimate(float waitingAnimation) {
		yield return new WaitForSeconds(waitingAnimation);
		GetComponent<Animation>().Play();
	}
}
