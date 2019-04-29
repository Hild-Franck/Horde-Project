using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipController : MonoBehaviour {
	public Tooltip tooltip;
	public Tooltip secondTooltip;
	public Tooltip switchTooltip;
	public Tooltip rotationTooltip;
	static public TooltipController instance = null;
	private bool triggered = false;
	private bool secondTriggered = false;
	private bool cancelTriggered = false;
	private bool switchTriggered = false;
	private bool rotationTriggered = false;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			if (!triggered) {
				tooltip.LeaveScreen();
				triggered = true;
			}
			if (secondTriggered && !cancelTriggered) {
				secondTooltip.LeaveScreen();
				if (!rotationTriggered) {
					rotationTooltip.EnterScreen();
				} else if (!switchTriggered) {
					switchTooltip.EnterScreen();
				}
			}
			if (GhostController.instance.GetGhost().isWall && !secondTriggered) {
				secondTooltip.EnterScreen();
				secondTriggered = true;
			}
		}

		if (secondTriggered && Input.GetButtonDown("Fire2")) {
			secondTooltip.LeaveScreen();
			cancelTriggered = true;
			rotationTooltip.EnterScreen();
		}

		if (!switchTriggered && Input.GetButtonDown("Fire3")) {
			switchTooltip.LeaveScreen();
			switchTriggered = true;
		}

		if (!rotationTriggered && Input.GetButtonDown("Rotate")) {
			rotationTooltip.LeaveScreen();
			rotationTriggered = true;
			if (!switchTriggered) {
				switchTooltip.EnterScreen();
			}
		}

		if (secondTriggered && switchTriggered && rotationTriggered) {
			Destroy(gameObject);
		}
	}
}
