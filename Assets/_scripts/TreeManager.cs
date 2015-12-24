using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeManager : MonoBehaviour {

	public GameObject treeRoot;
	public GameObject seasonHolder;
	public GameObject highlightRing;
	public float hexSize = 2;

	public void NewTree(){
		Instantiate (treeRoot);
	}

	public void ChangeSeason(){
		foreach (GameObject branch in PlayerManager.Instance.seasonTiles) {
			PlayerManager.Instance.activeTiles.Add (branch);
			branch.GetComponent<TreeTile> ().tileRenderer.sortingLayerName = "Tree";
		}
		foreach (GameObject branch in PlayerManager.Instance.treeTiles) {
			branch.transform.SetParent (transform);
			branch.layer = LayerMask.NameToLayer("TreeTiles");
		}

		// Create a temporary list of tiles that need to be destroyed later
		List<GameObject> destroyList = new List<GameObject>();

		foreach (GameObject branch in PlayerManager.Instance.activeTiles) {
			bool[] directionsDown = branch.GetComponent<TreeTile> ().directionsDown;
			if (directionsDown [0] || directionsDown [1] || directionsDown [2]) {
				branch.transform.SetParent (transform);
				branch.layer = LayerMask.NameToLayer ("ActiveTiles");
				GameObject highlight = Instantiate (highlightRing) as GameObject;
				highlight.transform.position = branch.transform.position - new Vector3 (0.0f, 0.0f, 1.0f);
				highlight.transform.SetParent (branch.transform);
			} else {
				Debug.Log ("Destroying unreachable active tile");
				// Add an active tile that has no roots to the destroy list
				destroyList.Add (branch);
			}
		}

		// Destroy tiles after removing them from the active list
		foreach (GameObject branch in destroyList){
			PlayerManager.Instance.activeTiles.Remove (branch);
			Destroy (branch);
		}

		// Move the whole tree downwards, add new season tiles
		transform.Translate (new Vector3 (0.0f, -Mathf.Sqrt(3) * hexSize, 0.0f));
		PlayerManager.Instance.seasonTiles.Clear ();
		PlayerManager.Instance.ChangeSeason ();
		GameObject instance = Instantiate (seasonHolder);
		instance.name = "CurrentSeason";
		instance.GetComponent<SeasonController> ().AddTiles (PlayerManager.Instance.activeTiles, PlayerManager.Instance.seasonTiles, 2);
	}
}
