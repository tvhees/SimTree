using UnityEngine;
using System.Collections;

abstract public class HexTile : ProcBase {

	public float m_rotation = 30.0f;

	protected MeshBuilder hexMesh = new MeshBuilder();

	protected void Start(){
		MeshBuilder meshBuilder = new MeshBuilder();

		BuildHex (meshBuilder, Vector3.zero, PlayerManager.Instance.hexSize);

		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter> ();

		ApplyMesh(meshBuilder, filter);
			
		this.transform.rotation = Quaternion.Euler (0.0f, 0.0f, m_rotation);
	}
}
