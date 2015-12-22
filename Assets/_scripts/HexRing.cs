using UnityEngine;
using System.Collections;

public class HexRing : MonoBehaviour {

	public float m_Length;
	public float innerProportion = 0.9f;
	public float m_rotation = 30.0f;
	public Vector3 m_pos;
	public float flickerSpeed = 2.5f;

	private Renderer tileRenderer;
	private MeshBuilder hexMesh = new MeshBuilder();

	void Awake(){
		tileRenderer = GetComponent<MeshRenderer> ();
	}

	void Update(){
		Color newColour = tileRenderer.material.color;
		newColour.a = 0.0f + Mathf.PingPong (Time.time * flickerSpeed, 1.0f);
		tileRenderer.material.color = newColour;
	}

	void Start(){
		m_Length = PlayerManager.Instance.hexSize;
		float i_Length = innerProportion * m_Length;

		//Set up the vertices and triangles:
		hexMesh.Vertices.Add (new Vector3 (0.0f, m_Length, 0.0f));
		hexMesh.UVs.Add (new Vector2 (0.0f, m_Length));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (0.0f, i_Length, 0.0f));
		hexMesh.UVs.Add (new Vector2 (0.0f, i_Length));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (Mathf.Sqrt(3)*m_Length/2, m_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*m_Length, m_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (Mathf.Sqrt(3)*i_Length/2, i_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*i_Length, i_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (Mathf.Sqrt(3)*m_Length/2, -m_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*m_Length, -m_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (Mathf.Sqrt(3)*i_Length/2, -i_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*i_Length, -i_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (0.0f, -m_Length, 0.0f));
		hexMesh.UVs.Add (new Vector2 (0.0f, -m_Length));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (0.0f, -i_Length, 0.0f));
		hexMesh.UVs.Add (new Vector2 (0.0f, -i_Length));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (-Mathf.Sqrt(3)*m_Length/2, -m_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (-Mathf.Sqrt(3)*m_Length, -m_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (-Mathf.Sqrt(3)*i_Length/2, -i_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (-Mathf.Sqrt(3)*i_Length, -i_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (-Mathf.Sqrt(3)*m_Length/2, m_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*m_Length, m_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (-Mathf.Sqrt(3)*i_Length/2, i_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*i_Length, i_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		for (int i = 0; i < 12; i=i+2) {
			hexMesh.AddTriangle (i, (int)Mathf.Repeat(i+2,12), (int)Mathf.Repeat(i+1,12));
			hexMesh.AddTriangle ((int)Mathf.Repeat(i+1,12), (int)Mathf.Repeat(i+2,12), (int)Mathf.Repeat(i+3,12));
		}

		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter> ();

		if (filter != null) {
			filter.sharedMesh = hexMesh.CreateMesh ();
		}
			
		this.transform.rotation = Quaternion.Euler (0.0f, 0.0f, m_rotation);
	}
}
