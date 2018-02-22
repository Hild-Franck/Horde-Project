using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DebugDisplayer : MonoBehaviour {

	public Text text;
	public EntityController eC;
	public PlayerController pC;
	public BuildingController bC;

	private bool hit = false;
	private bool parished = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		string newText = "Health: " + eC.health;
		newText += "\nGold: " + bC.gold + "G";
		newText += "\nCurrent building: " + GridOverlay.instance.currentBuilding;
		newText += "\nCurrent people: " + bC.currentPeople;
		newText += "\nCombo: " + eC.GetComboCount();
		text.text = newText;
	}

	string toStr(bool boolean) {
		return boolean ? "true" : "false";
	} 
}
