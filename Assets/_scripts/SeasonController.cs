using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonController : MonoBehaviour {

	public GameObject seasonTile;
    public GameController game;

	private Vector3 newPosition;
	private float hexOffset;
	private bool[] directions = new bool[3]{true, true, true};

	void Awake(){
        game = PlayerManager.Instance.game;
		hexOffset = Mathf.Sqrt(3) * 0.5f * PlayerManager.Instance.hexSize;
	}

	void Update(){
		if (transform.childCount < 1)
			Destroy (gameObject);
	}

	public void AddTiles (List<GameObject> tileListIn, List<GameObject> tileListOut, int tileType){

		foreach (GameObject tile in tileListIn) {
			TreeTile tileScript = tile.GetComponent<TreeTile> ();
			for (int i = 0; i < 3; i++) {
				if (tileScript.directionsUp [i])
					NewTile (i, tile.transform.position, tileType);
				else
					NewTile (i, tile.transform.position, 5);
			}
		}

		tileListOut.Clear();

		for (int i = 0; i < transform.childCount; i++) {
			tileListOut.Add(transform.GetChild (i).gameObject);
		}
	}

	public void NewTile (int i, Vector3 oldPosition, int tileType){
		newPosition = oldPosition;
		switch (i) {
		case 0:
			newPosition += new Vector3 (-Mathf.Sqrt(3)*hexOffset, hexOffset, 0.0f);
			break;
		case 1:
			newPosition += new Vector3 (0, hexOffset*2, 0.0f);
			break;
		case 2:
			newPosition += new Vector3 (Mathf.Sqrt(3)*hexOffset, hexOffset, 0.0f);
			break;
		}

		Collider2D hit = Physics2D.OverlapPoint (newPosition);

		if (!hit) {
			GameObject newTile = Instantiate(seasonTile);
			newTile.transform.SetParent (transform);

			if (tileType != 5) {
				game.WeatherSelector ();
				int j = Random.Range (0, game.weatherList.Count);
				tileType = game.weatherList [j];
				game.weatherList.RemoveAt (j);
			}

			newTile.GetComponent<TreeTile> ().UpdateTile (tileType, newPosition, directions, false, true, true);

		}
	}
}
