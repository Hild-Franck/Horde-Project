﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 4f;

	public float dashTime = 0.2f;
	public float dashSpeed = 5f;

	private PlayerMotor motor;
	private EntityController entityController;
	public Animator swordAnimation;
	public Camera FPSCamera;

	public GameObject fireBall;
	public GameObject sword;

	private GameObject weapon;
	private bool isDashing = false;
	private bool isGuarding = false;
	private float nextDash;
	private float smoothVelocity = 0.0f;
	private float speedModifier = 1f;

	void Start() {
		motor = GetComponent<PlayerMotor>();
		entityController = GetComponent<EntityController>();
		weapon = sword;
	}

	void Update () {
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed * speedModifier;

		if (isDashing) {
			FPSCamera.fieldOfView = Mathf.SmoothDamp(FPSCamera.fieldOfView, 60, ref smoothVelocity, dashTime);
			_velocity = Dash(_velocity);
		}

		if (!isDashing && FPSCamera.fieldOfView != 55) {
			FPSCamera.fieldOfView = Mathf.SmoothDamp(FPSCamera.fieldOfView, 55, ref smoothVelocity, 0.1f);
		}

		motor.Move(_velocity);

		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
		
		motor.Rotate(_rotation);

		float _xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 _cameraRotation = new Vector3(_xRot,  0f, 0f) * lookSensitivity;
		
		motor.RotateCamera(-_cameraRotation);

		if (Input.GetButtonDown("Fire1") && weapon == sword) {
			entityController.Attack();
		}

		if (Input.GetButtonDown("Fire2") && !entityController.isGuarding) {
			entityController.Guard();
			speedModifier = 0.6f;
		} else if (Input.GetButtonUp("Fire2") && entityController.isGuarding){
			entityController.Unguard();
			speedModifier = 1f;
		}

		if (Input.GetButtonDown("Switch")) {
			if (weapon == fireBall) {
				weapon = sword;
				fireBall.SetActive(false);
				sword.SetActive(true);
			} else {
				weapon = fireBall;
				fireBall.SetActive(true);
				sword.SetActive(false);
			}
		}

		if (isDashing && Time.time > nextDash) {
			isDashing = false;
		}

		if(Input.GetButtonDown("Jump") && _zMov == -1.0f && !isDashing) {
			isDashing = true;

			nextDash = Time.time + dashTime;
		}
	}

	Vector3 Dash(Vector3 _velocity) {
		return _velocity + transform.forward * -dashSpeed;
	}
}
