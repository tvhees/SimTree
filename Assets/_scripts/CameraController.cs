using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	//[SerializeField] 
	//public List<Transform> targets = new List<Transform>();

	float boundingBoxPadding = 2f;

	float minimumOrthographicSize = 20f;

	float zoomSpeed = 2f;

	private Camera mainCamera;

	void Awake ()
	{
		mainCamera = GetComponent<Camera> ();
		mainCamera.orthographicSize = 20;
	}

	public void ZoomOut()
	{
		Rect boundingBox = CalculateTargetsBoundingBox(PlayerManager.Instance.treeTiles);
		transform.position = CenterCameraPosition(boundingBox.center);
		CalculateOrthographicSize(boundingBox);
	}

	public void ZoomFit(){
		Rect boundingBox = CalculateTargetsBoundingBox(PlayerManager.Instance.seasonTiles);
		CalculateOrthographicSize(boundingBox);
		Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
		Vector3 topLeft = new Vector3(boundingBox.x, boundingBox.y, 0f);

		if (mainCamera.WorldToViewportPoint(topRight).x > 1 || mainCamera.WorldToViewportPoint(topLeft).x < 0)
			transform.position = new Vector3(CenterCameraPosition(boundingBox.center).x, transform.position.y, transform.position.z);
	}

	/// <summary>
	/// Calculates a bounding box that contains all the targets.
	/// </summary>
	/// <returns>A Rect containing all the targets.</returns>
	Rect CalculateTargetsBoundingBox(List<GameObject> targetList)
	{
		float minX = Mathf.Infinity;
		float maxX = Mathf.NegativeInfinity;
		float minY = Mathf.Infinity;
		float maxY = Mathf.NegativeInfinity;

		foreach (GameObject target in targetList) {
			Vector3 position = target.transform.position;

			minX = Mathf.Min(minX, position.x);
			minY = Mathf.Min(minY, position.y);
			maxX = Mathf.Max(maxX, position.x);
			maxY = Mathf.Max(maxY, position.y);
		}

		return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
	}

	/// <summary>
	/// Calculates a camera position given the a bounding box containing all the targets.
	/// </summary>
	/// <param name="boundingBox">A Rect bounding box containg all targets.</param>
	/// <returns>A Vector3 in the center of the bounding box.</returns>
	Vector3 CenterCameraPosition(Vector3 centerPosition)
	{
		return new Vector3(centerPosition.x, centerPosition.y, mainCamera.transform.position.z);
	}

	/// <summary>
	/// Calculates a new orthographic size for the camera based on the target bounding box.
	/// </summary>
	/// <param name="boundingBox">A Rect bounding box containg all targets.</param>
	/// <returns>A float for the orthographic size.</returns>
	void CalculateOrthographicSize(Rect boundingBox)
	{
		float orthographicSize = mainCamera.orthographicSize;
		Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
		Vector3 topRightAsViewport = mainCamera.WorldToViewportPoint(topRight);

		if (topRightAsViewport.x >= topRightAsViewport.y)
			orthographicSize = 1.2f*Mathf.Abs (boundingBox.width) / mainCamera.aspect / 2f;
		else
			orthographicSize = 1.2f*Mathf.Abs (boundingBox.height) / 2f;

		int check = 0;
		while (Mathf.Abs(mainCamera.orthographicSize - orthographicSize) > Mathf.Epsilon){
			mainCamera.orthographicSize = Mathf.Clamp (Mathf.Lerp (mainCamera.orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, Mathf.Infinity);
			check++;
			if (check > 1000){
				break;	
			}

		}
	}
}