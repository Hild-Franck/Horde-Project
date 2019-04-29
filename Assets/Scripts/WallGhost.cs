using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGhost : Ghost {

	public GameObject wallEndGhostPrefab;
	public GameObject wallEndPrefab;
	public GameObject wallPartGhostPrefab;
	public GameObject wallPartPrefab;
	public GameObject wall;
	private Transform wallEnds;
	private List<GameObject> constructionStack = new List<GameObject>();
	private bool isEndingWall = false;
	private int offsetModifier = 0;
	private GameObject finalWallEnd;
	private WallEndDetector detector;
	private BuildingEndDetector buildingEndDetector;

	// Use this for initialization
	void Start () {
		isWall = true;
		currentRotation = transform.rotation;
		graphics = transform.GetChild(0);
		wallEnds = graphics.GetChild(0);
		detector = transform.GetComponentInChildren<WallEndDetector>();
		buildingDetector = transform.GetComponentInChildren<BuildingDetector>();
		buildingEndDetector = transform.GetComponentInChildren<BuildingEndDetector>();
		building = null;
	}

	public override void Build(Vector3 startCell, Vector3 finishCell) {
		GameObject wallInstance = Instantiate(wall, transform.position, transform.rotation);

		BoxCollider col = wallInstance.GetComponent<BoxCollider>();
		col.size = buildingDetector.GetCollider().size;
		col.center = buildingDetector.GetCollider().center;
		GameObject prefabToUse = isEndingWall && !detector.GetIsColliding() ? wallEndPrefab : wallPartPrefab;
		float orientation = CheckZAxisFacing()
			? GetRotationDirection() * Mathf.Sign(finishCell.x - startCell.x)
			: GetRotationDirection() * Mathf.Sign(finishCell.z - startCell.z);
		GameObject wallPartEnd = Instantiate(prefabToUse, Vector3.zero, Quaternion.identity);
		wallPartEnd.transform.parent = wallInstance.transform;
		wallPartEnd.transform.localRotation = Quaternion.identity;
		wallPartEnd.transform.localPosition = new Vector3(.5f, 0,0);
		prefabToUse = isEndingWall ? wallEndPrefab : wallPartPrefab;
		if (isEndingWall) {
			GameObject wallPartEndEnd = Instantiate(prefabToUse, Vector3.zero, Quaternion.identity);
			wallPartEndEnd.transform.parent = wallInstance.transform;
			wallPartEndEnd.transform.localRotation = Quaternion.identity;
			wallPartEndEnd.transform.localPosition = new Vector3(.5f + (constructionStack.Count + 1) * orientation, 0,0);
		}
		for (int i = 1; i <= constructionStack.Count; i++) {
			GameObject wallPart = Instantiate(wallPartPrefab, Vector3.zero, Quaternion.identity);
			wallPart.transform.parent = wallInstance.transform;
			wallPart.transform.localRotation = Quaternion.identity;
			wallPart.transform.localPosition = new Vector3(.5f + i * orientation, 0,0);
		}

		CancelPreview();
	}

	public override void CancelPreview() {
		ResetGraphics();
		if (isEndingWall) {
			Destroy(wallEnds.GetChild(1).gameObject);
			Destroy(wallEnds.GetChild(0).gameObject);
			InstantiateWallEnd(wallPartGhostPrefab, 0);
			isEndingWall = false;
		}
		offsetModifier = 0;
		buildingDetector.ResetCollider();
	}

	public override void PreviewWall(Vector3 startCell, int previousOffsetCell, int currentOffsetCell) {
		bool shouldInstantiate = true;
		bool shouldDestroy = true;
		float orientation = GetRotationDirection() * Mathf.Sign(currentOffsetCell);
		int currentOffset = Mathf.Abs(currentOffsetCell);
		int previousOffset = Mathf.Abs(previousOffsetCell);
		GameObject prefabToUse = !detector.GetIsColliding() ? wallEndPrefab : wallPartPrefab;
		buildingDetector.UpdateCollider(currentOffsetCell, currentOffset, GetRotationDirection());

		if (!isEndingWall && currentOffset > 2) {
			isEndingWall = true;
			shouldInstantiate = false;
			Destroy(wallEnds.GetChild(0).gameObject);
			InstantiateWallEnd(prefabToUse, 0);
			finalWallEnd = InstantiateWallEnd(wallEndGhostPrefab, currentOffsetCell);
			offsetModifier = -1;
		} else if (isEndingWall && currentOffset <= 2) {
			isEndingWall = false;
			shouldDestroy = false;
			Destroy(wallEnds.GetChild(1).gameObject);
			Destroy(wallEnds.GetChild(0).gameObject);
			InstantiateWallEnd(wallPartGhostPrefab, 0);
			offsetModifier = 0;
		}

		if (isEndingWall) {
			finalWallEnd.transform.localPosition = new Vector3(currentOffset * orientation, 0, 0);
			buildingEndDetector.SnapToCursor(finalWallEnd.transform.position);
		}

		if (currentOffsetCell * previousOffsetCell < 0 || previousOffsetCell == 0) {
			ResetGraphics();
			Transform t = transform;
			for (int i = 1; i <= currentOffset + offsetModifier; i++) {
				InstantiateWall(orientation * i);
			}
			if (isEndingWall && shouldInstantiate) InstantiateWall(orientation * currentOffset + offsetModifier);
		} else {
			int absDiff = currentOffset - previousOffset;
			if (absDiff > 0) {
				if (isEndingWall && shouldInstantiate) InstantiateWall(orientation * previousOffset);
				for (int i = 1; i <= absDiff + offsetModifier; i++) {
					InstantiateWall(orientation * (i + previousOffset));
				}
			} else if (absDiff < 0) {
				absDiff = Mathf.Abs(absDiff);
				int diffModifier = shouldDestroy ? 0 : -1;
				foreach (var construction in constructionStack.Skip(currentOffset + offsetModifier)) {
					Destroy(construction);
				}
				constructionStack.RemoveRange(previousOffset - absDiff + offsetModifier, absDiff + diffModifier);
			}
		}
	}

	private void ResetGraphics() {
		foreach (var construction in constructionStack.Skip(0)) {
				Destroy(construction);
			}
		constructionStack.Clear();
	}

	private GameObject InstantiateWallPart(GameObject wallPrefab, Transform parent, float xPosition) {
		GameObject newWallPart = Instantiate(wallPrefab, Vector3.zero, Quaternion.identity);
		foreach (Renderer renderer in newWallPart.transform.GetComponentsInChildren<Renderer>()) {
			renderer.material = currentMaterial;
		}
		newWallPart.transform.parent = parent;
		newWallPart.transform.localPosition = new Vector3(xPosition, 0, 0);
		newWallPart.transform.localRotation = Quaternion.identity;
		return newWallPart;
	}

	private void InstantiateWall(float xPosition) {
		GameObject newWallPart = InstantiateWallPart(wallPartGhostPrefab, graphics, xPosition);
		constructionStack.Add(newWallPart);
	}

	private GameObject InstantiateWallEnd(GameObject wallPrefab, int position) {
		return InstantiateWallPart(wallPrefab, wallEnds, position);
	}
}
