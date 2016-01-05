using UnityEngine;
using System.Collections;

	/// <summary>
	/// A branch with semi-random curve based on start and end points
	/// </summary>
public class BezierGenerator : ProcBase	{

	public int timeSteps = 5;
	public int radialSteps = 8;
	public float radius = 0.6f;
	public bool taper = true;

	private Vector3[] R;
	private Vector3[] curvePoints;
	private Vector3 lastPoint;
	private float posRadius;

	public void GetReference(Vector3 vIn, Vector3 vOut, Vector3 tangentIn, Vector3 tangentOut){
		R = new Vector3[4];
		R [0] = vIn;
		R [1] = vIn + Random.value * tangentIn;
		R [2] = vOut - Random.value * tangentOut;
		R [3] = vOut;
	}

	public Vector3 GetPoint(float t){
		t = Mathf.Clamp01 (t);
		float mT = 1.0f - t;

		return mT * mT * mT * R[0] + 3.0f * mT * mT * t * R[1] + 3.0f * mT * t * t * R[2] + t * t * t * R[3];
	}

	public Vector3 GetTangent(float t){
		t = Mathf.Clamp01 (t);
		float mT = 1.0f - t;

		return 3.0f * mT * mT * (R [1] - R [0]) + 6.0f * mT * t * (R [2] - R [1]) + 3.0f * t * t * (R [3] - R [2]);
	}

	//Build the mesh:
	public void BuildMesh(Vector3 start, Vector3 end, Vector3 startTangent, Vector3 endTangent)
	{
		GetReference (start, end, startTangent, endTangent);
		//Create a new mesh builder:
		MeshBuilder meshBuilder = new MeshBuilder();

		//bent cylinder:
		//build the rings:
		for (int i = 0; i <= timeSteps; i++)
		{
			
			//Position on the Bezier Curve at this timestep:
			Vector3 centrePos = GetPoint((float)i/timeSteps);
	
			Debug.Log (centrePos);

			if (i > 0) {
				Debug.DrawLine (lastPoint, centrePos, Color.red, Mathf.Infinity);
			}

			lastPoint = centrePos;

			//rotation at that position on the circle:
			Vector3 tangent = GetTangent((float)i/timeSteps);
			float zAngleDegrees = Mathf.Rad2Deg * Mathf.Atan (tangent.y / tangent.x);
			if (zAngleDegrees >= 0)
				zAngleDegrees = -(90 - zAngleDegrees);
			else
				zAngleDegrees = 90 + zAngleDegrees;
			
			Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, zAngleDegrees);

			//Debug.Log (zAngleDegrees);

			//V coordinate is based on height:
			float v = (float)i / timeSteps;

			//New branches should taper towards the end

			if (taper)
				posRadius = radius * (1 - v/2);
			else
				posRadius = radius;

			//build the ring:
			BuildRing(meshBuilder, radialSteps, centrePos, posRadius, v, i > 0, rotation);
		}

		Mesh mesh = meshBuilder.CreateMesh();

		//Look for a MeshFilter component attached to this GameObject:
		MeshFilter filter = GetComponent<MeshFilter>();

		//If the MeshFilter exists, attach the new mesh to it.
		//Assuming the GameObject also has a renderer attached, our new mesh will now be visible in the scene.
		if (filter != null)
		{
			filter.sharedMesh = mesh;
		}
	}
}