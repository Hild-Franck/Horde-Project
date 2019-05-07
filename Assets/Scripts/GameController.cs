using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	static public GameController instance = null;
	
	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
}