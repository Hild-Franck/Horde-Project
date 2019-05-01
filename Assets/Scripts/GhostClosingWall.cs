using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostClosingWall : MonoBehaviour {
	public GameObject ghostWallPartPrefab;
	public GameObject ghostWallEndPrefab;
	public GameObject wallPartPrefab;
	public GameObject wallEndPrefab;
	public bool isEndingWall = false;
	private GameObject currentGhostWallPrefab;
	private GameObject graphics;
	private int graphicsChildNumber;
	private Vector3 initialLocalPosition;

	public static GhostClosingWall Create(GameObject wallBeginningPrefab) {
		GameObject instance = Instantiate(wallBeginningPrefab, Vector3.zero, Quaternion.identity);
		return instance.GetComponent<GhostClosingWall>();
	}
	void Start () {
		graphics = transform.GetChild(0).gameObject;
		graphicsChildNumber = graphics.transform.childCount;
		initialLocalPosition = transform.localPosition;
	}
	
	public void ResetPosition() {
		transform.localPosition = initialLocalPosition;
	}

	public void SwitchGraphic() {
		isEndingWall = !isEndingWall;
		for (int i = 0; i < graphicsChildNumber; i++) {
			GameObject child = graphics.transform.GetChild(i).gameObject;
			child.SetActive(!child.activeSelf);
		}
	}

	public void Build(GameObject parent) {
		GameObject prefabToUse = isEndingWall ? wallEndPrefab : wallPartPrefab;
		GameObject instance = Instantiate(prefabToUse, transform.position, transform.rotation);
		instance.transform.parent = parent.transform;
	}
}
