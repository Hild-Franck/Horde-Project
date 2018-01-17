using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 4f;

	public float dashTime = 0.2f;
	public float dashSpeed = 5f;
	public bool isAttacking = false;

	private PlayerMotor motor;
	public Animator swordAnimation;
	public Camera FPSCamera;

	public GameObject fireBall;
	public GameObject sword;

	private GameObject weapon;
	private bool isDashing = false;
	private bool isGuarding = false;
	private float nextDash;
	private float smoothVelocity = 0.0f;

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
		if (isAttacking && (!swordAnimation.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !swordAnimation.IsInTransition(0))) {
			isAttacking = false;
		}

		if (Input.GetButtonDown("Fire1") && weapon == sword) {
			swordAnimation.SetTrigger("Attacking");
			isAttacking = true;
		}

		if (Input.GetButtonDown("Fire2") && !isGuarding) {
			swordAnimation.SetTrigger("Guarding");
			isGuarding = true;
		} else if (Input.GetButtonUp("Fire2") && isGuarding){
			Debug.Log("Hello !");	
			swordAnimation.SetBool("Guarding", false);
			isGuarding = false;
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
