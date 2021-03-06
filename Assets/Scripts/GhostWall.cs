﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWall : Ghost {
	public GameObject wallPrefab;
	public GameObject ghostWallBeginningPrefab;
	private GhostClosingWall wallBeginning;
	private GhostClosingWall wallEnding;
	private List<GameObject> constructionStack = new List<GameObject>();

	private bool isWallEnding = false;

	void Start () {
		buildingDetector = transform.GetComponentInChildren<BuildingDetector>();
		wallBeginning = transform.GetChild(0).GetComponent<GhostClosingWall>();
		wallEnding = transform.GetChild(1).GetComponent<GhostClosingWall>();
		currentRotation = transform.rotation;
		graphics = transform;
		isWall = true;
	}

	public override void PreviewWall(Vector3 startCell, int previousOffsetCell, int currentOffsetCell) {
		float orientation = GetRotationDirection();
		int currentOffset = Mathf.Abs(currentOffsetCell);
		buildingDetector.UpdateCollider(currentOffsetCell, currentOffset, GetRotationDirection());
		if (currentOffset > 0) {
			wallEnding.gameObject.SetActive(true);
			wallEnding.transform.localPosition = new Vector3(currentOffsetCell * orientation, 0, 0);
		} else {
			wallEnding.gameObject.SetActive(false);
		}

		if (currentOffset > 1) {
			PreviewWallCenter(currentOffsetCell, previousOffsetCell, orientation);
		} else {
			ResetGraphics();
		}

		if (currentOffset > 2 && !isWallEnding) {
			wallBeginning.SwitchGraphic();
			wallEnding.SwitchGraphic();
			isWallEnding = true;
		} else if (currentOffset < 3 && isWallEnding) {
			wallBeginning.SwitchGraphic();
			wallEnding.SwitchGraphic();
			isWallEnding = false;
		}
	}

	public override void CancelPreview() {
		wallEnding.gameObject.SetActive(false);
		isWallEnding = false;
		if (wallBeginning.isEndingWall) wallBeginning.SwitchGraphic();
		if (wallEnding.isEndingWall) wallEnding.SwitchGraphic();
		ResetGraphics();
		buildingDetector.ResetCollider();
	}

	public override void Build(Vector3 constructionCellStart, Vector3 coord) {
		GameObject wallInstance = Instantiate(wallPrefab, transform.position, transform.rotation);
		BoxCollider col = wallInstance.GetComponent<BoxCollider>();
		col.size = buildingDetector.GetCollider().size;
		col.center = buildingDetector.GetCollider().center;
		wallBeginning.Build(wallInstance);
		wallEnding.Build(wallInstance);
		foreach (var construction in constructionStack) {
			construction.GetComponent<GhostClosingWall>().Build(wallInstance);
		}
		ResetGraphics();
		buildingDetector.ResetCollider();
		wallEnding.gameObject.SetActive(false);
	}

	private void PreviewWallCenter(int offset, int prevOffset, float orientation) {
		int absOffset = Mathf.Abs(offset);
		int absPrevOffset = Mathf.Abs(prevOffset);
		float relOrientation = orientation * Mathf.Sign(offset);
		if (offset * prevOffset < 0 || prevOffset == 0) {
			ResetGraphics();
			for (int i = 1; i < absOffset; i++) {
				InstantiateWallCenterPart(relOrientation * i);
			}
		} else {
			int absOffsetDiff = absOffset - absPrevOffset;
			if (absOffsetDiff > 0) {
				for (int i = 0; i < absOffsetDiff; i++) {
					InstantiateWallCenterPart(relOrientation * (i + absPrevOffset));
				}
			} else if (absOffsetDiff < 0) {
				absOffsetDiff = Mathf.Abs(absOffsetDiff);
				foreach (var construction in constructionStack.Skip(absOffset - 1)) {
					Destroy(construction);
				}
				constructionStack.RemoveRange(absPrevOffset - absOffsetDiff - 1, absOffsetDiff);
			}
		}
	}

	private void InstantiateWallCenterPart(float xPosition) {
		GameObject wallCenterPart = Instantiate(ghostWallBeginningPrefab, Vector3.zero, Quaternion.identity);
		foreach (Renderer renderer in wallCenterPart.transform.GetComponentsInChildren<Renderer>()) {
			renderer.material = currentMaterial;
		}
		wallCenterPart.transform.parent = transform;
		wallCenterPart.transform.localPosition = new Vector3(xPosition, 0f, 0f);
		wallCenterPart.transform.localRotation = Quaternion.identity;
		constructionStack.Add(wallCenterPart);
	}

	private void ResetGraphics() {
		foreach (var construction in constructionStack) {
				Destroy(construction);
		}
		constructionStack.Clear();
	}
}
