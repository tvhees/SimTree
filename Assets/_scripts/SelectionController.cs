using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionController : HexTile {

	public GameObject treeManager;
	public GameObject tileManager;
	public GameObject branchGenerator;
	public float flickerSpeed = 1.0f;
	public Camera mainCamera;

	private Renderer tileRenderer;
	private float hexOffset;
	private float index = 0;
	private Vector3 home;
	private GameObject selectedTile;
	private TreeTile selectedScript;
	private bool selecting = true;
	private List<Vector3> placePositions = new List<Vector3>();
	private int placeIndex = 0;
	private TreeManager treeScript;

	void Awake(){
		home = transform.position;
		tileRenderer = GetComponent<MeshRenderer> ();
		tileRenderer.sortingLayerName = "SelectionBox";
		hexOffset = Mathf.Sqrt(3) * 0.5f * PlayerManager.Instance.hexSize;
		treeScript = treeManager.GetComponent<TreeManager> ();
	}

	// Update is called once per frame
	void Update () {
		Color newColour = tileRenderer.material.color;
		newColour.a = 0.5f + Mathf.PingPong (Time.time * flickerSpeed, 0.5f);
		tileRenderer.material.color = newColour;

		if (Input.GetKeyDown (KeyCode.W)) {
			if (selecting) {
				MoveSelector ();
			} else {
				MoveTile ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			if (selecting) {
				SelectTile ();
			} else {
				PlaceTile ();
			}
			selecting = !selecting;

			if (PlayerManager.Instance.seasonTiles.Count < 1) {
				PlayerManager.Instance.EndGame ();
			}
			else if (PlayerManager.Instance.energy < 1) {
				PlayerManager.Instance.EndGame ();
			}
			else if (PlayerManager.Instance.activeTiles.Count < 1) {
				treeScript.ChangeSeason ();
				mainCamera.GetComponent<CameraController> ().ZoomFit ();
			}
		}
	}

	void MoveSelector(){
		index++;
		index = Mathf.Repeat (index, 3);
		transform.position = home + new Vector3 (2 * index * PlayerManager.Instance.hexSize, 0.0f, 0.0f);
	}

	void SelectTile(){
		// Get a reference to the selected tile
		Collider2D tile = Physics2D.OverlapPoint (transform.position);
		selectedTile = tile.gameObject;
		selectedScript = tile.GetComponent<TreeTile> ();

		// Build list of potential placements starting with the 'deselect' position
		placePositions.Clear ();
		foreach(GameObject activeTile in PlayerManager.Instance.activeTiles){
			placePositions.Add (activeTile.transform.position);
		}
		placePositions.Add(transform.position);
		placeIndex = placePositions.Count-1;

		// Move the tile to the first potential position
		MoveTile();

	}

	void MoveTile (){
		placeIndex = (int)Mathf.Repeat (placeIndex + 1, placePositions.Count);

		selectedTile.transform.position = placePositions [placeIndex];
		transform.position = placePositions [placeIndex];
	}

	void PlaceTile(){
		if (placeIndex + 1 == placePositions.Count) {
			// do nothing if deselecting the tile
		} else {
			// Attach selected tile to the tree, change its material and set it as active for next season
			selectedTile.transform.SetParent (treeManager.transform);
			selectedScript.ChangeMaterial (0);
			PlayerManager.Instance.treeTiles.Add (selectedTile);

			// Run any player effects from the season tile
			GameObject seasonTile = PlayerManager.Instance.activeTiles[placeIndex];
			PlayerManager.Instance.ResolveTile (seasonTile);

			// Destroy tiles that now cannot be accessed
			PruneTree();

			// Remove the sprite attached to the tree
			Destroy(selectedTile.transform.GetChild(0).gameObject);

			// Procedurally Generate Branches as required
			for (int i = 0; i < 3; i++) {
				if (seasonTile.GetComponent<TreeTile> ().directionsDown [i]) {
					for (int j = 0; j < 3; j++) {
						if (selectedScript.directionsUp [j])
							MakeBranches (i, j);
						}
				}
			}

			// Destroy the season tile that was there
			Destroy(seasonTile);
			PlayerManager.Instance.activeTiles.RemoveAt (placeIndex);

			// Place a new tile at the end of the tile manager
			tileManager.GetComponent<NewTileManager>().UpdateSelection(placePositions[placePositions.Count - 1]);

			// Return the selection marker to the start
			transform.position = home;
			index = 0;

			PlayerManager.Instance.size++;

		}

		selectedTile = null;
	}

	void PruneTree(){
		Vector3 prunePosition = selectedTile.transform.position;
		for (int i = 0; i < 3; i++) {
			switch (i) {
			case 0:
				prunePosition = selectedTile.transform.position + new Vector3 (-Mathf.Sqrt(3)*hexOffset, hexOffset, 0.0f);
				break;
			case 1:
				prunePosition = selectedTile.transform.position + new Vector3 (0, hexOffset*2, 0.0f);
				break;
			case 2:
				prunePosition = selectedTile.transform.position + new Vector3 (Mathf.Sqrt(3)*hexOffset, hexOffset, 0.0f);
				break;
			}

			Collider2D hit = Physics2D.OverlapPoint (prunePosition, LayerMask.NameToLayer("ActiveTiles"));	
			if (hit) {
				if (!selectedScript.directionsUp [i]) {
					PlayerManager.Instance.seasonTiles.Remove (hit.gameObject);
					PlayerManager.Instance.treeTiles.Add (hit.gameObject);
					hit.GetComponent<TreeTile> ().ChangeMaterial (5);
				} else {
					hit.GetComponent<TreeTile> ().directionsDown [2 - i] = true;
				}
			}	
		}
	}

	void MakeBranches(int i, int j){
		Vector3[] startBranch = new Vector3[]{new Vector3(-1.50f, -Mathf.Sqrt(3)/2f, 0.0f), 
												new Vector3(0.0f, -Mathf.Sqrt(3), 0.0f),
												new Vector3(1.50f, -Mathf.Sqrt(3)/2f, 0.0f)};
		Vector3[] endBranch = new Vector3[]{new Vector3(-1.5f, Mathf.Sqrt(3)/2f, 0.0f), 
												new Vector3(0.0f, Mathf.Sqrt(3), 0.0f),
												new Vector3(1.5f, Mathf.Sqrt(3)/2f, 0.0f)};

		float[] startAngle = new float[]{ -60.0f, 0.0f, 60.0f };
		float[] endAngle = new float[]{ 60.0f, 0.0f, -60.0f };

		GameObject branch = Instantiate (branchGenerator);

		branch.GetComponent<BranchGenerator> ().BuildMesh(selectedTile.transform.position + startBranch[i],selectedTile.transform.position + endBranch[j], startAngle[i], endAngle[j]);
		branch.transform.SetParent(selectedTile.transform);

	}
		
}
