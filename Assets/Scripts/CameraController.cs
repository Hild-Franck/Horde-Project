using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float speed = .5f;
	public float rotationSpeed = .5f;
	public float zoomSpeed = 1.0f;

	private Transform camera;

	// Use this for initialization
	void Start () {
		camera = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Rotate();
		Zoom();
	}

	private void Move() {
		float xInput = Input.GetAxisRaw("Horizontal");
		float yInput = Input.GetAxisRaw("Vertical");

		Vector3 xVector = transform.right * xInput;
		Vector3 yVector = transform.forward * yInput;

		Vector3 velocity = (xVector + yVector).normalized * speed;

		transform.position = transform.position + velocity;
	}

	private void Rotate() {
		float rotationInput = Input.GetAxisRaw("Rotate Camera");
		Vector3 rotationVelocity = new Vector3(0, rotationInput * rotationSpeed, 0);

		transform.Rotate(rotationVelocity);
	}

	private void Zoom() {
		float zoomImput = Input.GetAxisRaw("Mouse ScrollWheel");
		
		Vector3 zoomVector = camera.forward * zoomImput;

		Vector3 zoomVelocity = zoomVector.normalized * zoomSpeed;

		camera.localPosition = camera.localPosition + zoomVelocity;
	}
}
