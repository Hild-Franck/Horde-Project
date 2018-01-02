using UnityEngine;
using System.Collections;
 
public class GridOverlay : MonoBehaviour {

    public static GridOverlay instance = null;
 
    public GameObject map;
    public Camera camera;
    public GameObject buildingGhost;
    public GameObject wallGhost;

    private GameObject ghost;
 
    public bool showMain = true;
    public bool showSub = false;
    
    public int gridSizeX;
    private int gridSizeY = 0;
    public int gridSizeZ;

    public float smallStep = 1;
    public float largeStep = 2;

    public float startX;
    private float startY = 0.01f;
    public float startZ;

    private Vector3 hitPoint;
    private bool cursorOnMap = false;

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
        ghost = buildingGhost;
        wallGhost.SetActive(false);

		gridSizeX = (int) map.transform.localScale.x;
		gridSizeZ = (int) map.transform.localScale.y;

        startX = map.transform.position.x - gridSizeX/2;
        startZ = map.transform.position.z - gridSizeZ/2;
	}

    void Update() {
        if (Input.GetButtonDown("Switch")) {
            if (ghost == buildingGhost) {
                ghost = wallGhost;
                buildingGhost.SetActive(false);
                wallGhost.SetActive(true);
            } else {
                ghost = buildingGhost;
                wallGhost.SetActive(false);
                buildingGhost.SetActive(true);
            }
        }

		RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
            cursorOnMap = true;
			hitPoint = hit.point;
        } else {
            cursorOnMap = false;
        }
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
        // GL.Begin(GL.QUADS);
        // GL.Color(mainColor);
        var x = Mathf.Floor(hitPoint.x);
        // x = x - (x%largeStep)-smallStep;
        var y = Mathf.Floor(hitPoint.z);
        // y = y - (y%largeStep)-smallStep;
        // GL.Vertex3(x, 0.01f, y);
        // GL.Vertex3(x+1f, 0.01f, y);
        // GL.Vertex3(x+1f, 0.01f, y+1f);
        // GL.Vertex3(x, 0.01f, y+1f);
        // GL.End();
        ghost.transform.position = new Vector3(x+0.5f, 0.5f, y+0.5f);
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