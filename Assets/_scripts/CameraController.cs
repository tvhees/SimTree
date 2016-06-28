using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	float boundingBoxPadding = 2f;

	float minimumOrthographicSize = 20f;

	float zoomSpeed = 2f;

	private Camera mainCamera;

	void Awake ()
	{
		mainCamera = GetComponent<Camera> ();
		mainCamera.orthographicSize = 20;
	}

    public IEnumerator ZoomOut()
    {
        Rect boundingBox = CalculateTargetsBoundingBox(PlayerManager.Instance.game.treeTiles);
        float newSize = CalculateOrthographicSize(boundingBox), oldSize = mainCamera.orthographicSize;
        Vector3 newPos = CenterCameraPosition(boundingBox.center), oldPos = transform.position;

        yield return StartCoroutine(MoveCamera(oldPos, newPos, oldSize, newSize));
	}

	public IEnumerator ZoomFit(float adjustment = 0f){
		Rect boundingBox = CalculateTargetsBoundingBox(PlayerManager.Instance.game.seasonTiles);
        float newSize = CalculateOrthographicSize(boundingBox), oldSize = mainCamera.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
		Vector3 topLeft = new Vector3(boundingBox.x, boundingBox.y, 0f);

        Vector3 newPos = transform.position, oldPos = transform.position;

		if (mainCamera.WorldToViewportPoint(topRight).x > 1 || mainCamera.WorldToViewportPoint(topLeft).x < 0)
			newPos = new Vector3(CenterCameraPosition(boundingBox.center).x, transform.position.y, transform.position.z);

        // Adjusting camera to move tree 'downwards' relatively when desired.
        newPos = newPos + new Vector3(0f, adjustment, 0f);

        yield return StartCoroutine(MoveCamera(oldPos, newPos, oldSize, newSize));
    }

    IEnumerator MoveCamera(Vector3 oldPos, Vector3 newPos, float oldSize, float newSize) {
        newSize = Mathf.Max(newSize, minimumOrthographicSize);
        float sqrDistance = (transform.position - newPos).sqrMagnitude, linDistance = Mathf.Abs(newSize - mainCamera.orthographicSize);

        int breakNo = 0;
        float t = Time.time;
        while (sqrDistance > Mathf.Epsilon || linDistance > 0.01f)
        {
            transform.position = Vector3.Lerp(oldPos, newPos, (Time.time - t) * zoomSpeed);
            mainCamera.orthographicSize = Mathf.Lerp(oldSize, newSize, (Time.time - t) * zoomSpeed);
            yield return null;
            sqrDistance = (transform.position - newPos).sqrMagnitude;
            linDistance = Mathf.Abs(newSize - mainCamera.orthographicSize);
            breakNo++;
            if (breakNo > 1000) {
                Debug.Log("breaking loop");
                yield break;
            }
        }
        Debug.Log("zoom success");
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
	float CalculateOrthographicSize(Rect boundingBox)
	{
		float orthographicSize = mainCamera.orthographicSize;
		Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
		Vector3 topRightAsViewport = mainCamera.WorldToViewportPoint(topRight);

		if (topRightAsViewport.x >= topRightAsViewport.y)
			orthographicSize = 1.2f*Mathf.Abs (boundingBox.width) / mainCamera.aspect / 2f;
		else
			orthographicSize = 1.2f*Mathf.Abs (boundingBox.height) / 2f;

        return orthographicSize;
	}
}