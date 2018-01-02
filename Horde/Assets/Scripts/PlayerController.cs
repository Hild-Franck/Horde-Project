using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 4f;

	private PlayerMotor motor;
	public Animator swordAnimation;

	public GameObject fireBall;
	public GameObject sword;

	private GameObject weapon;

	void Start() {
		motor = GetComponent<PlayerMotor>();
		weapon = fireBall;
	}

	void Update () {
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

		motor.Move(_velocity);

		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
		
		motor.Rotate(_rotation);

		float _xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 _cameraRotation = new Vector3(_xRot,  0f, 0f) * lookSensitivity;
		
		motor.RotateCamera(-_cameraRotation);

		if (Input.GetButtonDown("Fire1")) {
			swordAnimation.SetTrigger("Attacking");
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
	}
}
