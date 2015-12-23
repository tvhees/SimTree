using UnityEngine;
using System.Collections;

public class NewTileManager : MonoBehaviour {

	public GameObject[] hexTiles;
	public float spacing = 4.0f;
	public float dragBoundary;

	void Start(){
		dragBoundary = transform.position.x + spacing * 2.5f;
		for (int i = 0; i < 5; i++) {
			Vector3 position = transform.position + new Vector3 (spacing * i, 0.0f, 0.0f);
			AddTile (position);
		}
	}

	public void UpdateSelection(Vector3 emptyPosition){
		HexTile[] hexTiles = transform.GetComponentsInChildren<HexTile> ();
		foreach (HexTile tile in hexTiles) {
			if (tile.transform.position.x > emptyPosition.x) {
				tile.transform.Translate (new Vector3 (-spacing, 0.0f, 0.0f), Space.World);
				if (tile.transform.position.x < dragBoundary)
					tile.GetComponent<TreeTile> ().draggable = true;
			}
		}

		AddTile (transform.position + new Vector3 (spacing * 4.0f, 0.0f, 0.0f));

	}

	void AddTile(Vector3 tilePosition){
		if (PlayerManager.Instance.tileIndex.Count < 1)
			PlayerManager.Instance.CreateTileIndex ();
		int i = Random.Range (0, PlayerManager.Instance.tileIndex.Count);
		int j = PlayerManager.Instance.tileIndex [i];
		PlayerManager.Instance.tileIndex.RemoveAt(i);
		GameObject instance = Instantiate (hexTiles[j]);
		instance.transform.SetParent (transform);
		bool[] directions = null;
		instance.GetComponent<TreeTile> ().UpdateTile (0, tilePosition, directions, false, false);
		if (instance.transform.position.x < dragBoundary)
			instance.GetComponent<TreeTile> ().draggable = true;
	}
}
