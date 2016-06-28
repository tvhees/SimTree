using UnityEngine;
using System.Collections;

public class NewTileManager : MonoBehaviour {

    public GameController game;
	public GameObject[] hexTiles;
	public float spacing = 4.0f;
    public float spacingFactor = 0.8f;
    public float scaleFactor = 0.5f;
	public float dragBoundary;

	public void CreateTileSelection(){
		dragBoundary = transform.position.x + spacing * 2.5f;
        Vector3 position = transform.position - spacing * Vector3.right;
		for (int i = 0; i < 5; i++) {
            if (i < 4)
            {
                position += spacing * Vector3.right;
            }
            else
            {
                position += spacingFactor * spacing * Vector3.right;
            }
            AddTile(position);
        }
	}

	public void UpdateSelection(Vector3 emptyPosition){
		TreeTile[] hexTiles = transform.GetComponentsInChildren<TreeTile> ();
		foreach (TreeTile tile in hexTiles) {
            if (tile.transform.position.x > emptyPosition.x)
            {
                // Move tiles left first - a normal distance if they're selectable tiles
                // a smaller distance if they're upcoming tiles
                if (tile.transform.position.x < dragBoundary + spacing)
                {
                    tile.transform.Translate(spacing * Vector3.left, Space.World);
                }
                else
                    tile.transform.Translate(spacing * spacingFactor * Vector3.left, Space.World);

                // Scale all selectable tiles appropriately
                if (tile.transform.position.x < dragBoundary)
                {
                    tile.GetComponent<TreeTile>().draggable = true;
                    tile.transform.localScale = Vector3.one;
                }
            }
        }

		AddTile (transform.position + new Vector3 (spacing * (3f + spacingFactor), 0.0f, 0.0f));
	}

	void AddTile(Vector3 tilePosition){
		if (game.tileIndex.Count < 1)
			game.CreateTileIndex ();
		int i = Random.Range (0, game.tileIndex.Count);
		int j = game.tileIndex [i];
		game.tileIndex.RemoveAt(i);
		GameObject instance = Instantiate (hexTiles[j]);
		instance.transform.SetParent (transform);
		instance.tag = "InactiveBranch";
		bool[] directions = null;
		instance.GetComponent<TreeTile> ().UpdateTile (0, tilePosition, directions, false, false, false);
        if (instance.transform.position.x < dragBoundary)
            instance.GetComponent<TreeTile>().draggable = true;
        else
            instance.transform.localScale = scaleFactor * Vector3.one;
	}
}
