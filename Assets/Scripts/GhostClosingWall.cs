using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostClosingWall : MonoBehaviour {
	public GameObject wallEndPrefab;
	public GameObject wallEndAltPrefab;
	private Vector3 initialLocalPosition;
	private GameObject wallEnd;
	private GameObject wallEndAlt;
	private SnapDetector snapDetector;

	public static GhostClosingWall Create(GameObject wallBeginningPrefab) {
		GameObject instance = Instantiate(wallBeginningPrefab, Vector3.zero, Quaternion.identity);
		return instance.GetComponent<GhostClosingWall>();
	}
    void Start() {
			initialLocalPosition = transform.localPosition;
			Transform graphic = transform.GetChild(0);
			wallEnd = graphic.GetChild(0).gameObject;
			wallEndAlt = graphic.GetChild(1).gameObject;
			snapDetector = GetComponentInChildren<SnapDetector>();
		}

    public void ResetPosition() => transform.localPosition = initialLocalPosition;

    public void Build(GameObject parent) {
			GameObject prefabToUse = wallEndPrefab;
			if (snapDetector.CheckSnapping()){
      	snapDetector.RemoveObjects();
				prefabToUse = wallEndAltPrefab;
			}
			GameObject instance = Instantiate(prefabToUse, Vector3.zero, Quaternion.identity);
			instance.transform.parent = parent.transform;
			instance.transform.localPosition = transform.localPosition;
			instance.transform.localRotation = transform.localRotation;
	}

	public void ToggleGraphic() {
		wallEnd.SetActive(!wallEnd.activeSelf);
		wallEndAlt.SetActive(!wallEndAlt.activeSelf);
	}

	private void ResetGraphic() {
		Debug.Log("Reset graphic");
		wallEnd.SetActive(true);
		wallEndAlt.SetActive(false);
	}
}
