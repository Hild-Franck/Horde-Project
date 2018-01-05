using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 cameraRotation = Vector3.zero;

	private Rigidbody rigidbody;
	private bool isColliding = false;
	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
	}
	
	public void Move(Vector3 _velocity){
		velocity = _velocity;
	}

	public void Rotate(Vector3 _rotation){
		rotation = _rotation;
	}

	void FixedUpdate() {
		if (!isColliding) {
			PerformMovement();
		}
		PerformRotation();
	}

	void PerformMovement(){
		if(velocity != Vector3.zero) {
			rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
		}
	}

	void PerformRotation() {
		rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotation));
		if (cam!= null) {
			cam.transform.Rotate(cameraRotation);
		}
	}

	public void RotateCamera(Vector3 _cameraRotation){
		cameraRotation = _cameraRotation;
	}

	public void OnCollisionEnter (Collision col) {
		// isColliding = true;
	}

}
