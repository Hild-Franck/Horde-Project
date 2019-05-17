using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetector : MonoBehaviour {
	private bool isColliding = false;
	private Vector3[] vertices = new Vector3[4];
	private BoxCollider collider;
	private Vector3 startingPoint;
	private Vector3 finishPoint;
	private Transform collidingBuilding;

	void Start () {
		collider = transform.GetComponent<BoxCollider>();
	}
	
	void Update () {
	}

	public bool GetIsColliding() {
		return isColliding;
	}

	public BoxCollider GetCollider() {
		return collider;
	}

	public Vector3 GetFinishPoint() {
		return finishPoint;
	}

	public Vector3 GetStartingPoint() {
		return startingPoint;
	}

	public void UpdateColliderCorners() {
		Matrix4x4 matrix = transform.localToWorldMatrix;
		Quaternion storedRotation = transform.rotation;

		transform.rotation = Quaternion.identity;
		Vector3 extents = collider.bounds.extents;
		Vector3 center = collider.center;
		vertices[0] = matrix.MultiplyPoint3x4(new Vector3(extents.x + center.x, -extents.y + center.y, extents.z + center.z));
		vertices[1] = matrix.MultiplyPoint3x4(new Vector3(-extents.x + center.x, -extents.y + center.y, extents.z + center.z));
		vertices[2] = matrix.MultiplyPoint3x4(new Vector3(extents.x + center.x, -extents.y + center.y, -extents.z + center.z));
		vertices[3] = matrix.MultiplyPoint3x4(new Vector3(-extents.x + center.x, -extents.y + center.y, -extents.z + center.z));

		transform.rotation = storedRotation;

		startingPoint = vertices[0];
		finishPoint = vertices[0];
		foreach (Vector3 vertice in vertices) {
			if (Mathf.Round(vertice.x) <= startingPoint.x && Mathf.Round(vertice.z) >= startingPoint.z) {
				startingPoint = vertice;
			}
			if (Mathf.Round(vertice.x) >= startingPoint.x && Mathf.Round(vertice.z) <= startingPoint.z) {
				finishPoint = vertice;
			}
		}
	}

	public void ResetCollider() {
		collider.center = new Vector3(0, collider.center.y, 1);
		collider.size = new Vector3(3.9f, collider.size.y, collider.size.z);
    transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
	}

	public void UpdateCollider(int currentOffsetCell, int currentOffset, float rotation) {
    transform.rotation = new Quaternion(0, rotation, 0, transform.rotation.w);
		float mod = Mathf.Sign(currentOffsetCell) == -1 ? 1f : 0;
		collider.center = new Vector3((currentOffsetCell / 2f - .5f + mod + (rotation == 0 ? 0 : 1f)), collider.center.y, rotation == 0 ? 1f : 0);
		collider.size = new Vector3(Mathf.Clamp(currentOffset + 2.9f, 3.9f, Mathf.Infinity), collider.size.y, collider.size.z);

	}

	void FixedUpdate() {
		isColliding = false;
	}


	void OnTriggerEnter(Collider other) {
		if (other.tag == "Building") isColliding = true;
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Building") isColliding = true;
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Building") isColliding = false;
	}
}
