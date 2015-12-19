using UnityEngine;
using System.Collections;

abstract public class HexTile : MonoBehaviour {

	public float m_Length;
	public float m_rotation = 30.0f;
	public Vector3 m_pos;


	protected MeshBuilder hexMesh = new MeshBuilder();

	protected void Start(){
		m_Length = PlayerManager.Instance.hexSize;

		//Set up the vertices and triangles:
		hexMesh.Vertices.Add (new Vector3 (0.0f, m_Length, 0.0f));
		hexMesh.UVs.Add (new Vector2 (0.0f, m_Length));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (Mathf.Sqrt(3)*m_Length/2, m_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*m_Length, m_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (Mathf.Sqrt(3)*m_Length/2, -m_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*m_Length, -m_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (0.0f, -m_Length, 0.0f));
		hexMesh.UVs.Add (new Vector2 (0.0f, -m_Length));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (-Mathf.Sqrt(3)*m_Length/2, -m_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (-Mathf.Sqrt(3)*m_Length, -m_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.Vertices.Add (new Vector3 (-Mathf.Sqrt(3)*m_Length/2, m_Length/2, 0.0f));
		hexMesh.UVs.Add (new Vector2 (Mathf.Sqrt(3)*m_Length, m_Length/2));
		hexMesh.Normals.Add (Vector3.forward);

		hexMesh.AddTriangle (0, 1, 5);
		hexMesh.AddTriangle (1, 2, 5);
		hexMesh.AddTriangle (2, 4, 5);
		hexMesh.AddTriangle (2, 3, 4);

		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter> ();

		if (filter != null) {
			filter.sharedMesh = hexMesh.CreateMesh ();
		}
			
		this.transform.rotation = Quaternion.Euler (0.0f, 0.0f, m_rotation);
	}
}
