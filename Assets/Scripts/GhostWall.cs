using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWall : Ghost {
	public GameObject wallPrefab;
	public GameObject ghostWallCenterPrefab;
	public GameObject preview;
	public RotationDetector rotationDetectorTop;
	public RotationDetector rotationDetectorBottom;
	private GhostClosingWall wallBeginning;
	private GhostClosingWall wallEnding;
	private List<GameObject> constructionStack = new List<GameObject>();
	private Vector3 startingCell = Vector3.zero;
	private bool isConstructing = false;
	private int currentOffset = 0;
	private int lastOffset = 0;
	private int rotation = 0;
	private bool rotated = false;
	private Vector3 coord = Vector3.zero;

	void Start () {
		buildingDetector = transform.GetComponentInChildren<BuildingDetector>();
		wallBeginning = preview.transform.GetChild(0).GetComponent<GhostClosingWall>();
		wallEnding = preview.transform.GetChild(1).GetComponent<GhostClosingWall>();
		currentRotation = transform.rotation;
		graphics = transform;
		isWall = true;
	}

	protected override void Update() {
		base.Update();
		if (isConstructing) PreviewWall();
    if ((rotationDetectorTop.CheckRotation() || rotationDetectorBottom.CheckRotation()) && !rotated) {
			rotated = true;
      rotation = -90;
      preview.transform.eulerAngles = new Vector3(0, rotation, 0);
		} else if (!(rotationDetectorTop.CheckRotation() || rotationDetectorBottom.CheckRotation()) && rotated) {
			rotated = false;
			rotation = 0;
      preview.transform.eulerAngles = new Vector3(0, rotation, 0);
		}
	}

	public override void SetPosition(Vector3 _coord) {
		coord = _coord;
		if (!isConstructing) transform.position = _coord;
	}

	private void PreviewWall() {
		lastOffset = currentOffset;
    float xOffset = coord.x - startingCell.x;
    float zOffset = coord.z - startingCell.z;
    if (Mathf.Abs(xOffset) >= Mathf.Abs(zOffset)) {
      rotation = 0;
      currentOffset = (int)xOffset;
    } else {
      rotation = -90;
      currentOffset = (int)zOffset;
    }
		if (rotationDetectorTop.CheckRotation() || rotationDetectorBottom.CheckRotation()) rotation = -90;
		preview.transform.eulerAngles = new Vector3(0, rotation, 0);
    float orientation = GetRotationDirection();
		// Number of cells between clicked cell and current hover cell
		int currentAbsOffset = Mathf.Abs(currentOffset);
		buildingDetector.UpdateCollider(currentOffset, currentAbsOffset, GetRotationDirection());
		if (currentAbsOffset > 0 && currentOffset > 0) {
			wallEnding.transform.localPosition = new Vector3((currentOffset) * orientation, 0, 0);
		} else if (currentAbsOffset > 0 && currentOffset < 0) {
			wallBeginning.transform.localPosition = new Vector3((currentOffset) * orientation, 0, 0);
		}
		if (currentAbsOffset > 1 && currentOffset > 0) {
			PreviewWallCenter(currentOffset, lastOffset, orientation);
		} else if(currentAbsOffset > 1 && currentOffset < 0) {
			PreviewWallCenter(currentOffset, lastOffset, orientation, -1);
		} else {
			ResetGraphics();
		}
	}

	public override void Cancel() {
		ResetGraphics();
		buildingDetector.ResetCollider();
		isConstructing = false;
		rotation = 0;
    preview.transform.eulerAngles = new Vector3(0, rotation, 0);
  }

	public override void Build(Vector3 coord) {
		if (!isConstructing) {
			startingCell = coord;
			isConstructing = true;
		} else {
			Construct();
		}
	}

	private void Construct() {
		GameObject wallInstance = Instantiate(wallPrefab, transform.position, preview.transform.rotation);
		BoxCollider col = wallInstance.GetComponent<BoxCollider>();
		col.size = buildingDetector.GetCollider().size;
		col.center = buildingDetector.GetCollider().center;
		wallBeginning.Build(wallInstance, rotationDetectorBottom);
		wallEnding.Build(wallInstance, rotationDetectorTop);
		foreach (var construction in constructionStack) {
			construction.GetComponent<WallPartGhost>().Build(wallInstance);
		}
		ResetGraphics();
		buildingDetector.ResetCollider();
		isConstructing = false;
		rotation = 0;
    preview.transform.eulerAngles = new Vector3(0, rotation, 0);
  }

	private void PreviewWallCenter(int offset, int prevOffset, float orientation, int mod = 0) {
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
				for (int i = 0+mod; i < absOffsetDiff+mod; i++) {
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
		GameObject wallCenterPart = Instantiate(ghostWallCenterPrefab, Vector3.zero, Quaternion.identity);
		foreach (Renderer renderer in wallCenterPart.transform.GetComponentsInChildren<Renderer>()) {
			renderer.material = currentMaterial;
		}
		wallCenterPart.transform.parent = preview.transform;
		wallCenterPart.transform.localPosition = new Vector3(xPosition, 0f, -1.5f);
		wallCenterPart.transform.localRotation = Quaternion.identity;
		constructionStack.Add(wallCenterPart);
	}

	private void ResetGraphics() {
		foreach (var construction in constructionStack) {
				Destroy(construction);
		}
		constructionStack.Clear();
		wallEnding.ResetPosition();
		wallBeginning.ResetPosition();
	}
}
