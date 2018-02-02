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
		string newText = "health: " + eC.health + "\nattacking: " + toStr(eC.isAttacking) + "\nguarding: " + toStr(eC.isGuarding) + "\nisHit: " + toStr(eC.isHit);
		newText += "\ndashing: " + toStr(pC.isDashing) + "\nweapon: " + pC.weapon.name;
		newText += "\nhouse: " + bC.currentBuildings + "/" + bC.maxBuildings;
		newText += "\nbuildings: " + bC.currentWalls + "/" + bC.maxWalls;
		newText += "\nattack count: " + EnemyController.playerAttackCount;
		text.text = newText;
	}

	string toStr(bool boolean) {
		return boolean ? "true" : "false";
	} 
}
