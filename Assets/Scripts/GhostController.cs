using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostController : MonoBehaviour {
	static public GhostController instance;
	public LayerMask mapLayer;
	public Material mat;
	public Material greenConstruct;
	public Material redConstruct;
	public GameObject[] ghosts;
	public int index = 0;
	private Ghost ghost;
	private Camera camera;
	private Vector3 hitPoint = Vector3.zero;
	private Vector3 coord = Vector3.zero;
	private Color gridColor = Color.green;
	private bool isConstructing = false;
	private bool canConstruct = true;
	private Vector3 constructionCellStart;
	private int lastConstructionOffset = 0;
	private int constructionOffset = 0;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
	void Start () {
		camera = this.GetComponent<Camera>();
		mat = new Material(Shader.Find("Hidden/Internal-Colored"));
		mat.hideFlags = HideFlags.HideAndDontSave;
		mat.SetColor("_Color", new Color(1, 1, 1, 0.1f));
		mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		mat.SetInt("_ZWrite", 0);
		ghost = Instantiate(ghosts[0], ghosts[0].transform.position, ghosts[0].transform.rotation).GetComponent<Ghost>();
	}
	
	// Update is called once per frame
	void Update () {
		bool isColliding = ghost.GetDetector().GetIsColliding();
		ChangeColor(isColliding);
		RaycastHit hit;

		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, mapLayer)) {
			Transform objectHit = hit.transform;
			hitPoint = hit.point;
			coord = new Vector3(Mathf.Floor(hitPoint.x), 0, Mathf.Floor(hitPoint.z));
			if (!isConstructing) ghost.SetPosition(coord);
		}

		if (Input.GetButtonDown("Rotate") && !isConstructing) {
			ghost.Rotate();
		}

		if (isConstructing && ghost.isWall) constructionOffset = Construct();

		if (Input.GetButtonDown("Fire1") ) {
			if (!isConstructing) {
				if (ghost.GetDetector().GetIsColliding()) {
					Debug.Log("Unable to construct");
				} else {
					if (ghost.isWall) {
						isConstructing = true;
						constructionCellStart = coord;
					} else if (canConstruct) {
						ghost.Build(constructionCellStart, coord);
					}
				}
			} else if (canConstruct) {
				ghost.Build(constructionCellStart, coord);
				isConstructing = false;
				lastConstructionOffset = 0;
			}
		}

		if (Input.GetButtonDown("Fire2") ) {
			ghost.CancelPreview();
			isConstructing = false;
			lastConstructionOffset = 0;
		}
		
		ghost.GetDetector().UpdateColliderCorners();

		if (Input.GetButtonDown("Fire3")) {
			Switch(coord);
		}


	}

	void OnPostRender() {
		Vector3 startingPoint = ghost.GetDetector().GetStartingPoint();
		Vector3 finishPoint = ghost.GetDetector().GetFinishPoint();

		for (float i = startingPoint.x; i < finishPoint.x; i++) {
			for (float j = startingPoint.z; j > finishPoint.z; j--) {
				DrawQuads(Mathf.Floor(i), Mathf.Floor(j) + 1);
			}
			DrawVertLines(Mathf.Floor(i), startingPoint, finishPoint);
		}
		for (float i = startingPoint.x; i < finishPoint.x + 1; i++) {
			DrawVertLines(Mathf.Floor(i), startingPoint, finishPoint);
		}
		for (float i = startingPoint.z + 1; i > finishPoint.z; i--) {
			DrawHorLines(Mathf.Floor(i), startingPoint, finishPoint);
		}
	}

	public Ghost GetGhost() {
		return ghost;
	}

	private void ChangeColor(bool isColliding) {
		if (isColliding && canConstruct) {
			gridColor = Color.red;
			ghost.ChangeGhostColor(redConstruct);
			canConstruct = false;
		} else if (!isColliding && !canConstruct) {
			gridColor = Color.green;
			ghost.ChangeGhostColor(greenConstruct);
			canConstruct = true;
		}
	}

	private void Switch(Vector3 coord) {
		index = (index >= ghosts.Length - 1) ? 0 : index + 1;
		Quaternion currentRotation = ghost.GetCurrentRotation();
		Destroy(ghost.gameObject);
		ghost = Instantiate(ghosts[index], coord, currentRotation).GetComponent<Ghost>();
	}

	private int Construct() {
		int currentConstructionOffset = ghost.CheckZAxisFacing()
			? (int)(coord.x - constructionCellStart.x)
			: (int)(coord.z - constructionCellStart.z);
		ghost.PreviewWall(constructionCellStart, lastConstructionOffset, currentConstructionOffset);
		lastConstructionOffset = currentConstructionOffset;
		return currentConstructionOffset;
	}

	private void DrawQuads(float x, float z) {
		mat.SetColor("_Color", new Color(1, 1, 1, 0.1f));
		mat.SetPass(0);
		GL.Begin(GL.QUADS);
		GL.Color(gridColor);
		GL.Vertex3(x, .01f, z);
		GL.Vertex3(x+1, .01f, z);
		GL.Vertex3(x+1, .01f, z-1);
		GL.Vertex3(x, .01f, z-1);
		GL.End();
	}

	private void DrawVertLines(float x, Vector3 start, Vector3 finish) {
		mat.SetColor("_Color", new Color(1, 1, 1, .4f));
		mat.SetPass(0);
		GL.Begin(GL.LINES);
		GL.Color(gridColor);
		GL.Vertex3(x, .01f, start.z);
		GL.Vertex3(x, .01f, finish.z);
		GL.End();
	}

	private void DrawHorLines(float z, Vector3 start, Vector3 finish) {
		mat.SetColor("_Color", new Color(1, 1, 1, .4f));
		mat.SetPass(0);
		GL.Begin(GL.LINES);
		GL.Color(gridColor);
		GL.Vertex3(start.x, .01f, z);
		GL.Vertex3(finish.x, .01f, z);
		GL.End();
	}
}