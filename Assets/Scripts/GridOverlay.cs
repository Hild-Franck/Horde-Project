using UnityEngine;
using System.Collections;
 
public class GridOverlay : MonoBehaviour {

    public static GridOverlay instance = null;
 
    public GameObject map;
    public Camera camera;
    public GameObject[] buildingGhosts;
    public GameObject buildingGhost;
    public GameObject wallGhost;
    public string currentBuilding;

    public LayerMask mapLayer;

    private GameObject ghost;
 
    public bool showMain = true;
    public bool showSub = false;
    
    public int gridSizeX;
    private int gridSizeY = 0;
    public int gridSizeZ;

    public float smallStep = 1;
    public float largeStep = 2;
    public float gridSnap = 0.5f;

    public float startX;
    public float startZ;
    private float startY = 0.01f;

    private Vector3 hitPoint;
    private bool cursorOnMap = false;
    private int index = 0;

    private Material lineMaterial;

    public  Color mainColor = new Color(0f, 1f, 0f, 1f);
    public Color subColor = new Color(0f, 0.5f, 0f, 1f);

    public void removeGhost () {
        ghost.SetActive(false);
        ghost = null;
    }

    void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}
    
    void Start () {
        ghost = buildingGhosts[0];
        ghost.SetActive(true);
        currentBuilding = ghost.name;

		gridSizeX = (int) map.transform.localScale.x;
		gridSizeZ = (int) map.transform.localScale.y;

        startX = map.transform.position.x - gridSizeX/2;
        startZ = map.transform.position.z - gridSizeZ/2;
	}

    void Update() {
        if (Input.GetButtonDown("Switch")) {
            Switch();
        }

		RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, mapLayer)) {
            Transform objectHit = hit.transform;
            cursorOnMap = true;
			hitPoint = hit.point;
        } else {
            cursorOnMap = false;
        }
	}

    void Switch() {
        ghost.SetActive(false);
        if (++index >= buildingGhosts.Length) index = 0;
        ghost = buildingGhosts[index];
        ghost.SetActive(true);
        currentBuilding = ghost.name;
    }
 
     void CreateLineMaterial() {
        if (!lineMaterial) {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void DrawLines(float steps) {
        //Layers
        for (float j = 0; j <= gridSizeY; j += steps) {
            //X axis lines
            for (float i = 0; i <= gridSizeZ; i += steps) {
                GL.Vertex3(startX, startY + j , startZ + i);
                GL.Vertex3(startX + gridSizeX, startY + j , startZ + i);
            }

            //Z axis lines
            for (float i = 0; i <= gridSizeX; i += steps) {
                GL.Vertex3(startX + i, startY + j , startZ);
                GL.Vertex3(startX + i, startY + j , startZ + gridSizeZ);
            }
        }

            //Y axis lines
        for (float i = 0; i <= gridSizeZ; i += steps) {
            for (float k = 0; k <= gridSizeX; k += steps) {
                GL.Vertex3(startX + k, startY , startZ + i);
                GL.Vertex3(startX + k, startY + gridSizeY , startZ + i);
            }
        }
    }

    void DrawQuad() {
        var x = hitPoint.x - (hitPoint.x % (gridSnap*2));
        var y = hitPoint.z - (hitPoint.z % (gridSnap*2));
        ghost.transform.position = new Vector3(x, gridSnap, y);
    }
 
    void OnPostRender() {
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        if (showSub) {
            GL.Color(subColor);
            DrawLines(smallStep);
        }
 
        if (showMain) {
            GL.Color(mainColor);
            DrawLines(largeStep);
        }
 
        GL.End();

        if (cursorOnMap) {
            DrawQuad();
        }
    }
}