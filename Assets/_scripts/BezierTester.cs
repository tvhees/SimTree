using UnityEngine;
using System.Collections;

public class BezierTester : MonoBehaviour {

	public int i;
	public int j;

	// Use this for initialization
	void Start () {
		BezierGenerator branch = GetComponent<BezierGenerator> ();

		Vector3[] startBranch = new Vector3[]{new Vector3(-1.50f, -Mathf.Sqrt(3)/2f, 0.0f), 
			new Vector3(0.0f, -Mathf.Sqrt(3), 0.0f),
			new Vector3(1.50f, -Mathf.Sqrt(3)/2f, 0.0f)};
		Vector3[] endBranch = new Vector3[]{new Vector3(-1.5f, Mathf.Sqrt(3)/2f, 0.0f), 
			new Vector3(0.0f, Mathf.Sqrt(3), 0.0f),
			new Vector3(1.5f, Mathf.Sqrt(3)/2f, 0.0f)};

		Vector3[] startTangent = new Vector3[]{ new Vector3 (Mathf.Sqrt(3)/2f, 0.5f, 0.0f), new Vector3(0.0f, 1f/Mathf.Sqrt(3), 0.0f), new Vector3(-Mathf.Sqrt(3)/2f, 0.5f, 0.0f) };
		Vector3[] endTangent = new Vector3[]{ new Vector3 (-Mathf.Sqrt(3)/2f, 0.5f, 0.0f), new Vector3(0.0f, 1f/Mathf.Sqrt(3), 0.0f), new Vector3(Mathf.Sqrt(3)/2f, 0.5f, 0.0f) };

		branch.BuildMesh(startBranch[i],endBranch[j], startTangent[i], endTangent[j]);
		branch.transform.SetParent(transform);
	}
}
