using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

	public GameObject treeRoot;
	public GameObject seasonHolder;
	public GameObject highlightRing;
	public float hexSize = 2;

	public void ChangeSeason(){
		foreach (GameObject branch in PlayerManager.Instance.seasonTiles) {
			PlayerManager.Instance.activeTiles.Add (branch);
			branch.GetComponent<TreeTile> ().tileRenderer.sortingLayerName = "Tree";
		}
		foreach (GameObject branch in PlayerManager.Instance.treeTiles) {
			branch.transform.SetParent (transform);
			branch.layer = LayerMask.NameToLayer("TreeTiles");
		}
	
		foreach (GameObject branch in PlayerManager.Instance.activeTiles) {
			branch.transform.SetParent (transform);
			branch.layer = LayerMask.NameToLayer("ActiveTiles");
			GameObject highlight = Instantiate (highlightRing, branch.transform.position, Quaternion.identity) as GameObject;
			highlight.transform.position -= new Vector3 (0.0f, 0.0f, 1.0f);
			highlight.transform.SetParent (branch.transform);
		}
		transform.Translate (new Vector3 (0.0f, -Mathf.Sqrt(3) * hexSize, 0.0f));
		PlayerManager.Instance.seasonTiles.Clear ();
		PlayerManager.Instance.ChangeSeason ();
		GameObject instance = Instantiate (seasonHolder);
		instance.GetComponent<SeasonController> ().AddTiles (PlayerManager.Instance.activeTiles, PlayerManager.Instance.seasonTiles, 2);
	}
}
