using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBarController : MonoBehaviour {

	public Image healthBar;
	public EntityController player;

	// Update is called once per frame
	void Update () {
		healthBar.fillAmount = player.health / player.startHealth;
	}
}
