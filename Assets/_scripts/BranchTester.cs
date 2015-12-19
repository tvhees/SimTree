using UnityEngine;
using System.Collections;

public class BranchTester : MonoBehaviour {

	public int i;
	public int j;
	public float[] startAngle = new float[]{ -60.0f, 0.0f, 60.0f };
	public float[] endAngle = new float[]{ 60.0f, 0.0f, -60.0f };

	// Use this for initialization
	void Start () {
		BranchGenerator branch = GetComponent<BranchGenerator> ();

		Vector3[] startBranch = new Vector3[]{new Vector3(-1.50f, -Mathf.Sqrt(3)/2f, 0.0f), 
			new Vector3(0.0f, -Mathf.Sqrt(3), 0.0f),
			new Vector3(1.50f, -Mathf.Sqrt(3)/2f, 0.0f)};
		Vector3[] endBranch = new Vector3[]{new Vector3(-1.5f, Mathf.Sqrt(3)/2f, 0.0f), 
			new Vector3(0.0f, Mathf.Sqrt(3), 0.0f),
			new Vector3(1.5f, Mathf.Sqrt(3)/2f, 0.0f)};

		branch.BuildMesh(transform.position + startBranch[i],transform.position + endBranch[j], startAngle[i], endAngle[j]);
		branch.transform.SetParent(transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
