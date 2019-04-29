using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour {
	public float fadeSpeed = .1f;
	private bool isFadingOut = false;
	private bool isFadingIn = false;
	private CanvasGroup group;
	// Use this for initialization
	void Start () {
		group = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isFadingOut) {
			group.alpha -= fadeSpeed;
			transform.Translate(fadeSpeed * 20, 0f, 0f);
		}
		if (isFadingIn) {
			group.alpha += fadeSpeed;
		}
		if (isFadingOut && group.alpha <= 0) {
			Destroy(gameObject);
		}
		if (isFadingIn && group.alpha >= 0) {
			group.alpha = 1;
			isFadingIn = false;
		}
	}

	public void EnterScreen() {
		isFadingIn = true;
	}

	public void LeaveScreen() {
		isFadingOut = true;
	}
}
