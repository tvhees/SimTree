using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeManager : MonoBehaviour {

    public GameController game;
	public GameObject treeRoot;
	public GameObject seasonHolder;
	public GameObject highlightRing;
	public float hexSize = 2;

    void Awake() {
        PlayerManager.Instance.treeManager = gameObject;
    }

	public void NewTree(){
		Instantiate (treeRoot);
	}

	public void ChangeSeason(){
		foreach (GameObject branch in game.seasonTiles) {
			game.activeTiles.Add (branch);
			branch.GetComponent<TreeTile> ().tileRenderer.sortingLayerName = "Tree";
		}
		foreach (GameObject branch in game.treeTiles) {
			branch.transform.SetParent (transform);
			branch.layer = LayerMask.NameToLayer("TreeTiles");
		}

		// Create a temporary list of tiles that need to be destroyed later
		List<GameObject> destroyList = new List<GameObject>();

		foreach (GameObject branch in game.activeTiles) {
			bool[] directionsDown = branch.GetComponent<TreeTile> ().directionsDown;
			if (directionsDown [0] || directionsDown [1] || directionsDown [2]) {
				branch.transform.SetParent (transform);
				branch.layer = LayerMask.NameToLayer ("ActiveTiles");
				GameObject highlight = Instantiate (highlightRing) as GameObject;
				highlight.transform.position = branch.transform.position - new Vector3 (0.0f, 0.0f, 1.0f);
				highlight.transform.SetParent (branch.transform);
			} else {
				// Add an active tile that has no roots to the destroy list
				destroyList.Add (branch);
			}
		}

		// Destroy tiles after removing them from the active list
		foreach (GameObject branch in destroyList){
			game.activeTiles.Remove (branch);
			Destroy (branch);
		}

		// Move the whole tree downwards, add new season tiles
		//transform.Translate (new Vector3 (0.0f, -Mathf.Sqrt(3) * hexSize, 0.0f));
		game.seasonTiles.Clear ();
		game.ChangeSeason ();
		GameObject instance = Instantiate (seasonHolder);
		instance.name = "CurrentSeason";
		instance.GetComponent<SeasonController> ().AddTiles (game.activeTiles, game.seasonTiles, 2);
	}
}
