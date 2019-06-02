using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostController : MonoBehaviour {
	static public GhostController instance;
	public LayerMask mapLayer;
	public Material mat;
	public GameObject[] ghosts;
	public int index = 0;
	private Ghost ghost;
	private Camera cam;
	private Vector3 hitPoint = Vector3.zero;
	private Vector3 coord = Vector3.zero;
	private Color gridColor = Color.green;
	private int rotation = 0;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
	
	void Start () {
		cam = this.GetComponent<Camera>();
		mat = new Material(Shader.Find("Hidden/Internal-Colored"));
		mat.hideFlags = HideFlags.HideAndDontSave;
		mat.SetColor("_Color", new Color(1, 1, 1, 0.1f));
		mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		mat.SetInt("_ZWrite", 0);
		ghost = Instantiate(ghosts[0], ghosts[0].transform.position, ghosts[0].transform.rotation).GetComponent<Ghost>();
	}
	
	
	void Update () {
		bool collisionChanged = ghost.CheckCollisionChange();
		ChangeColor(collisionChanged);
		
		// Get mouse coordinates on map
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, mapLayer)) {
			Transform objectHit = hit.transform;
			hitPoint = hit.point;
			coord = new Vector3(Mathf.Floor(hitPoint.x), 0, Mathf.Floor(hitPoint.z));
			ghost.SetPosition(coord);
		}

		if (Input.GetButtonDown("Rotate") && ghost.CheckRotationable()) ghost.Rotate();

		if (Input.GetButtonDown("Fire1") && !ghost.isColliding) ghost.Build(coord);

		if (Input.GetButtonDown("Fire2")) ghost.Cancel();
		
		ghost.GetDetector().UpdateColliderCorners();

		if (Input.GetButtonDown("Fire3")) Switch(coord);


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

	private void ChangeColor(bool collisionChanged) {
		if (collisionChanged) gridColor = gridColor == Color.red ? Color.green : Color.red;
	}

	private void Switch(Vector3 coord) {
		index = (index >= ghosts.Length - 1) ? 0 : index + 1;
		Quaternion currentRotation = ghost.GetCurrentRotation();
		Destroy(ghost.gameObject);
		ghost = Instantiate(ghosts[index], coord, currentRotation).GetComponent<Ghost>();
	}

	private void DrawQuads(float x, float z) {
		mat.SetColor("_Color", new Color(1, 1, 1, 0.2f));
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
		mat.SetColor("_Color", new Color(1, 1, 1, .6f));
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