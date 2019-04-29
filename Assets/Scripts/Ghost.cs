using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ghost : MonoBehaviour {
	public float rotationSpeed = 10;
	private bool isRotating = false;
	protected Quaternion currentRotation;
	private Quaternion previousRotation;
	private float rotationStart = 0;
	protected Transform graphics;
	public Material currentMaterial;
	public GameObject building;
	protected BuildingDetector buildingDetector;
	public bool isWall;

	void Update () {
		if (isRotating) {
			transform.rotation = Quaternion.Lerp(previousRotation, currentRotation, (Time.time - rotationStart) * rotationSpeed);
			if (transform.rotation == currentRotation) isRotating = false;
		}
	}

	public abstract void Build(Vector3 constructionCellStart, Vector3 coord);

	public void Rotate() {
		isRotating = true;
		previousRotation = currentRotation;
		currentRotation *=  Quaternion.AngleAxis(90, Vector3.up);
		rotationStart = Time.time;
	}

	public BuildingDetector GetDetector() {
		return buildingDetector;
	}
	
	public void SetPosition(Vector3 coord) {
		transform.position = coord;
	}

	public bool CheckZAxisFacing() {
		return ((currentRotation.eulerAngles.y / 90) % 2 == 0);
	}

	public void ChangeGhostColor(Material newMaterial) {
		currentMaterial = newMaterial;
		foreach (Renderer renderer in graphics.GetComponentsInChildren<Renderer>()) {
			renderer.material = currentMaterial;
		}
	}

	protected int GetRotationDirection() {
		int direction = currentRotation.eulerAngles.y > 90 ? -1 : 1;
		return CheckZAxisFacing() ? direction : direction * -1;
	}

	public Quaternion GetCurrentRotation() {
		return currentRotation;
	}

	public abstract void PreviewWall(Vector3 startCell, int previousOffsetCell, int currentOffsetCell);

	public abstract void CancelPreview();

}
