using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RootController : MonoBehaviour {

    public GameController game;
	public GameObject root;
	public GameObject[] trunkParts, startTiles;
	public GameObject ground;

	private float hexOffset;



	// Use this for initialization
	void Awake () {
        game = PlayerManager.Instance.game;
        game.rootController = this;
		transform.SetParent (GameObject.Find ("TreeStructure").transform);

		hexOffset = Mathf.Sqrt(3) * 0.5f * PlayerManager.Instance.hexSize;
		
		for (int i = 0; i < 3; i++) {
			GameObject newRoot = Instantiate (trunkParts[i]);
			newRoot.transform.SetParent (transform);

			Vector3 tilePosition = new Vector3 (0.0f, -2 * Mathf.Sqrt (3) * i, 0.0f);
            newRoot.transform.position = tilePosition;


            if (i == 0)
                game.activeTiles.Add(newRoot);
            else
                game.treeTiles.Add (newRoot);
		}

		GameObject newGround = Instantiate (ground) as GameObject;
		newGround.transform.SetParent (transform.parent);
	}

    public void StartingTiles() {
        bool[] tileDirections = new bool[3] { true, true, true };
        bool[] rootDirections = new bool[3];
        int tileType = 4;
        Vector3 newPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    newPosition = new Vector3(-Mathf.Sqrt(3) * hexOffset, hexOffset, 0.0f);
                    tileType = 4;
                    rootDirections = new bool[3] { false, false, true };
                    break;
                case 1:
                    newPosition = new Vector3(0, hexOffset * 2, 0.0f);
                    tileType = 5;
                    rootDirections = new bool[3] { false, false, false };
                    break;
                case 2:
                    newPosition = new Vector3(Mathf.Sqrt(3) * hexOffset, hexOffset, 0.0f);
                    tileType = 3;
                    rootDirections = new bool[3] { true, false, false };
                    break;
            }

            TreeTile tile = startTiles[i].GetComponent<TreeTile>();
            tile.UpdateTile(tileType, newPosition, tileDirections, false, false, false);
            tile.directionsDown = rootDirections;
            tile.gameObject.SetActive(true);
        }

        game.activeTiles.Add(startTiles[0]);
        game.treeTiles.Add(startTiles[1]);
        game.activeTiles.Add(startTiles[2]);

        game.state = GameController.State.PLAY;

        GetComponentInParent<TreeManager>().ChangeSeason();
    }
}
