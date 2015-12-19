using UnityEngine;
using System.Collections;

	/// <summary>
	/// A branch with semi-random curve based on start and end points
	/// </summary>
public class BranchGenerator : ProcBase	{

	//the radius and height of the cylinder:
	public float m_Radius = 0.6f;
	private float m_Height;

	//the angle to bend the cylinder:
	[SerializeField]
	private float bendAngleRadians;

	//the number of radial segments:
	public int m_RadialSegmentCount = 10;

	//the number of height segments:
	public int m_HeightSegmentCount = 10;

	[SerializeField]
	private int inversion;

	//public Vector3 start = new Vector3(0, -Mathf.Sqrt(3), 0);

	//public Vector3 end = new Vector3(-1.5f, Mathf.Sqrt(3)/2f, 0);

	[SerializeField]
	private float bendRadius;

	//public float startRotation = 0;

	//public float endRotation = 60;

	[SerializeField]
	private Vector3 circleCenter;


	//Build the mesh:
	public void BuildMesh(Vector3 start, Vector3 end, float startRotation, float endRotation)
	{
		//Create a new mesh builder:
		MeshBuilder meshBuilder = new MeshBuilder();

		// Find rotation required per ring to match start and end points
		float zAngleChange = endRotation - startRotation;

		// Set curve to left or right turn
		if (zAngleChange > 0)
			inversion = 1;
		else if (zAngleChange < 0)
			inversion = -1;
		else
			inversion = 2 * Random.Range (0, 1) - 1;

		// Calculate m_BendAngle from bendRadius and chord length
		Vector3 chord = start - end;
		Vector3 normal;
		Vector3 midpoint = (start + end) / 2;
		float chordLength = chord.magnitude;
		bendRadius = Random.Range (chordLength/2, Mathf.Sqrt(7)*chordLength);
		bendAngleRadians = Mathf.Acos (1.0f - ((chordLength * chordLength) / (2.0f * bendRadius * bendRadius)));

		normal = new Vector3 (inversion*chord.y, -inversion*chord.x, chord.z).normalized;

		float bisectorLength = Mathf.Sqrt (bendRadius * bendRadius - chordLength * chordLength / 4);
		circleCenter = midpoint + bisectorLength * normal;
		float initialAngleRadians = Mathf.Asin ((start.y - circleCenter.y) / bendRadius);

				Vector3 posOffset = new Vector3 (inversion * bendRadius*(1-Mathf.Cos(initialAngleRadians)), -bendRadius * Mathf.Sin (initialAngleRadians), 0.0f);

		transform.position = start + posOffset;

				//our bend code breaks if m_BendAngle is zero:
		if (bendAngleRadians < Mathf.Epsilon)
		{
			m_Height = end.y - start.y;
			//straight cylinder:
			float heightInc = m_Height / m_HeightSegmentCount;

			for (int i = 0; i <= m_HeightSegmentCount; i++)
			{
				Vector3 centrePos = Vector3.up * heightInc * i;
				float v = (float)i / m_HeightSegmentCount;

				BuildRing(meshBuilder, m_RadialSegmentCount, centrePos, m_Radius, v, i > 0);
			}
		}
		else
		{
			//bent cylinder:

			//get the angle in radians: (not necessary - Matf.Acos does this already)
			//float bendAngleRadians = m_BendAngle * Mathf.Deg2Rad;

			//the radius of our bend (vertical) circle:
			//float bendRadius = m_Height / bendAngleRadians;

			//the angle increment per height segment (based on arc length):
			float angleInc = bendAngleRadians / m_HeightSegmentCount;
			float zAngleInc = zAngleChange / m_HeightSegmentCount;

			//calculate a start offset that will place the centre of the first ring (angle 0.0f) on the mesh origin:
			//(x = cos(0.0f) * bendRadius, y = sin(0.0f) * bendRadius)
			Vector3 startOffset = new Vector3(inversion * bendRadius, 0.0f, 0.0f);


			//build the rings:
			for (int i = 0; i <= m_HeightSegmentCount; i++)
			{
				//unit position along the edge of the vertical circle:
				Vector3 centrePos = Vector3.zero;
				centrePos.x = inversion * Mathf.Cos(angleInc * i + initialAngleRadians);
				centrePos.y = Mathf.Sin(angleInc * i + initialAngleRadians);

				//rotation at that position on the circle:
				float zAngleDegrees = (zAngleInc * i);
				Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, zAngleDegrees + startRotation);

				//multiply the unit postion by the radius:
				centrePos *= bendRadius;

				//offset the position so that the base ring (at angle zero) centres around zero:
				centrePos -= startOffset;

				//V coordinate is based on height:
				float v = (float)i / m_HeightSegmentCount;

				//build the ring:
				BuildRing(meshBuilder, m_RadialSegmentCount, centrePos, m_Radius, v, i > 0, rotation);
			}
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