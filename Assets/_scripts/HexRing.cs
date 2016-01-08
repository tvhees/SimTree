using UnityEngine;
using System.Collections;

public class HexRing : ProcBase {

	public float m_Length;
	public float innerProportion = 0.9f;
	public float m_rotation = 30.0f;
	public float flickerSpeed = 2.5f;

	private Renderer tileRenderer;

	void Awake(){
		tileRenderer = GetComponent<MeshRenderer> ();
	}

	void Update(){
		Color newColour = tileRenderer.material.color;
		newColour.a = 0.0f + Mathf.PingPong (Time.time * flickerSpeed, 1.0f);
		tileRenderer.material.color = newColour;
	}

	void Start(){
		MeshBuilder meshBuilder = new MeshBuilder ();

		BuildHexRing (meshBuilder, Vector3.zero, PlayerManager.Instance.hexSize, innerProportion);

		//Create the mesh:
		MeshFilter filter = GetComponent<MeshFilter> ();
		ApplyMesh (meshBuilder, filter);
			
		this.transform.rotation = Quaternion.Euler (0.0f, 0.0f, m_rotation);
	}
}
