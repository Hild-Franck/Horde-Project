using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ComboBarController : MonoBehaviour {

	public Image comboBar;
	public EntityController player;

	void Update () {
		float comboEnd = player.GetComboEnd();
		if (Time.time <= comboEnd) {
			comboBar.fillAmount = (comboEnd - Time.time) / player.comboTime;
		} else {
			comboBar.fillAmount = 0f;
		}
	}
}
