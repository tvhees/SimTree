using UnityEngine;
using System.Collections;

public class NewTileManager : MonoBehaviour {

	public GameObject hexTile;
	public float spacing = 4.0f;

	void Start(){
		for (int i = 0; i < 5; i++) {
			Vector3 position = transform.position + new Vector3 (spacing * i, 0.0f, 0.0f);
			AddTile (position);
		}
	}

	public void UpdateSelection(Vector3 emptyPosition){
		HexTile[] hexTiles = transform.GetComponentsInChildren<HexTile> ();
		foreach (HexTile tile in hexTiles) {
			if (tile.transform.position.x > emptyPosition.x)
				tile.transform.Translate (new Vector3 (-spacing, 0.0f, 0.0f), Space.World);
		}

		AddTile (transform.position + new Vector3 (spacing * 4.0f, 0.0f, 0.0f));

	}

	void AddTile(Vector3 tilePosition){
		GameObject instance = Instantiate (hexTile);
		instance.transform.SetParent (transform);
		bool[] directions = RandomDirections();
		instance.GetComponent<TreeTile> ().UpdateTile (0, tilePosition, directions, true, false);
	}

	bool[] RandomDirections(){
		bool[] array = new bool[3]{false, false, false};
		while (!array [0] && !array [1] && !array [2]) {
			for (int i = 0; i < array.Length; i++) {
				array [i] = Random.value > 0.66f;
			}
		}
		return(array);
	}

}
