using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

	public GameObject treeRoot;
	public GameObject seasonHolder;
	public float hexSize = 2;

	public void ChangeSeason(){
		
		foreach (GameObject branch in PlayerManager.Instance.seasonTiles) {
			PlayerManager.Instance.activeTiles.Add (branch);
			branch.GetComponent<TreeTile> ().tileRenderer.sortingLayerName = "Tree";
		}
		foreach (GameObject branch in PlayerManager.Instance.treeTiles) {
			branch.transform.SetParent (transform);
		}
		foreach (GameObject branch in PlayerManager.Instance.activeTiles) {
			branch.transform.SetParent (transform);
		}
		transform.Translate (new Vector3 (0.0f, -Mathf.Sqrt(3) * hexSize, 0.0f));
		PlayerManager.Instance.seasonTiles.Clear ();
		PlayerManager.Instance.ChangeSeason ();
		GameObject instance = Instantiate (seasonHolder);
		instance.GetComponent<SeasonController> ().AddTiles (PlayerManager.Instance.activeTiles, PlayerManager.Instance.seasonTiles, 2);
	}
}
