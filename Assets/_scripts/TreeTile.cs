﻿using UnityEngine;
using System.Collections;

public class TreeTile : HexTile {

	public int type;
	public Material[] materials;
	public bool[] directionsUp = new bool[3];
	public bool[] directionsDown = new bool[3]{false, false, false};
	public Sprite sprite1;
	public Sprite sprite3;
	public Sprite sprite4;
	public Sprite sprite5;
	public Sprite sprite6;
	public Sprite sprite8;
	public Sprite sprite9;
	public string weather = "Inactive";
	public string season = "Spring";
	public MeshRenderer tileRenderer;
	public bool draggable;
	private GameObject treeManager;
	public GameObject branchGenerator;

	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 home;
	private int activeMask;

	void Awake(){
		treeManager = GameObject.Find ("TreeStructure");
	}

	void OnMouseDown(){
		if (weather != "Inactive")
			PlayerManager.Instance.informationPanel.GetComponent<InformationPanelController> ().InfoPanelOn (transform.position, season, weather);
		else if (draggable) {
			home = transform.position;
			screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
	}

	void OnMouseDrag(){
		if (draggable) {
			Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
			transform.position = cursorPosition;
		}
	}

	void OnMouseUp(){
		PlayerManager.Instance.informationPanel.SetActive (false);
	}

	void OnMouseUpAsButton(){
		if (draggable) {
			Vector2 screenPosition = PlayerManager.Instance.uiCamera.WorldToScreenPoint (transform.position);
			Vector2 target = PlayerManager.Instance.mainCamera.ScreenToWorldPoint (screenPosition);
			activeMask = 1 << LayerMask.NameToLayer ("ActiveTiles");
			Collider2D hit = Physics2D.OverlapPoint (target, activeMask);
			if (hit != null) {
				transform.position = hit.transform.position;
				PlaceTile (hit.gameObject);
				PlayerManager.Instance.CheckState ();
			}
			else
				transform.position = home;
		}
	}

	public void UpdateTile(int newType, Vector3 newPos, bool[] newDirections, bool changeSprite, bool changeSeason){
		ChangeMaterial (newType);

		ChangePosition (newPos);

		if (newDirections!=null)
			ChangeDirections (newDirections);

		if (changeSprite)
			ChangeSprite ();

		if (changeSeason)
			ChangeSeason ();
	}

	public void ChangeMaterial(int newType){
		type = newType;
		GetComponent<MeshRenderer>().material = materials[type];
		switch (type) {
		case 0:
		case 1:
		case 5:
			weather = "Inactive";
			break;
		case 2:
			weather = "Fair";
			break;
		case 3:
			weather = "Rain";
			break;
		case 4:
			weather = "Sunshine";
			break;
		case 6:
			weather = "Frost";
			break;
		}
	}

	void ChangePosition (Vector3 newPos){
		m_pos = newPos;
		transform.position = m_pos;
	}

	void ChangeDirections(bool[] newDirections){
		directionsUp = newDirections;

	}

	void ChangeRoots(bool[] newRoots){
		directionsDown = newRoots;
	}

	void ChangeSprite (){
		int spriteNumber = 0;

		if (directionsUp[0])
			spriteNumber += 1;
		if (directionsUp [1])
			spriteNumber += 3;
		if (directionsUp [2])
			spriteNumber += 5;

		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		switch (spriteNumber) {
		case 1:
			spriteRenderer.sprite = sprite1;
			break;
		case 3:
			spriteRenderer.sprite = sprite3;
			break;
		case 4:
			spriteRenderer.sprite = sprite4;
			break;
		case 5:
			spriteRenderer.sprite = sprite5;
			break;
		case 6:
			spriteRenderer.sprite = sprite6;
			break;
		case 8:
			spriteRenderer.sprite = sprite8;
			break;
		case 9:
			spriteRenderer.sprite = sprite9;
			break;
		}
	}

	void ChangeSeason(){
		season = PlayerManager.Instance.nextSeason;
	}

	void PlaceTile(GameObject seasonTile){
		// Get reference to the tile selection holder
		NewTileManager newTileScript = transform.parent.GetComponent<NewTileManager>();

		// Attach selected tile to the tree, change its material and set it as active for next season
		transform.SetParent (treeManager.transform);
		ChangeMaterial (0);
		PlayerManager.Instance.treeTiles.Add (gameObject);

		// Run any player effects from the season tile
		PlayerManager.Instance.ResolveTile (seasonTile);

		// Destroy tiles that now cannot be accessed
		PruneTree();

		// Remove the sprite attached to the tree
		Destroy(transform.GetChild(0).gameObject);

		// Procedurally Generate Branches as required
		for (int i = 0; i < 3; i++) {
			if (seasonTile.GetComponent<TreeTile> ().directionsDown [i]) {
				for (int j = 0; j < 3; j++) {
					if (directionsUp [j])
						MakeBranches (i, j);
				}
			}
		}

		// Destroy the season tile that was there
		PlayerManager.Instance.activeTiles.Remove (seasonTile);
		Destroy(seasonTile);

		// Place a new tile at the end of the tile manager
		newTileScript.UpdateSelection(home);

		PlayerManager.Instance.size++;


	}

	void PruneTree(){
		Vector3 prunePosition = transform.position;
		float hexOffset = Mathf.Sqrt(3) * 0.5f * PlayerManager.Instance.hexSize;
		for (int i = 0; i < 3; i++) {
			switch (i) {
			case 0:
				prunePosition = transform.position + new Vector3 (-Mathf.Sqrt(3)*hexOffset, hexOffset, 0.0f);
				break;
			case 1:
				prunePosition = transform.position + new Vector3 (0, hexOffset*2, 0.0f);
				break;
			case 2:
				prunePosition = transform.position + new Vector3 (Mathf.Sqrt(3)*hexOffset, hexOffset, 0.0f);
				break;
			}

			Collider2D hit = Physics2D.OverlapPoint (prunePosition);	
			if (hit) {
				if (!directionsUp [i]) {
					PlayerManager.Instance.seasonTiles.Remove (hit.gameObject);
					PlayerManager.Instance.activeTiles.Remove (hit.gameObject);
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

		branch.GetComponent<BranchGenerator> ().BuildMesh(transform.position + startBranch[i],transform.position + endBranch[j], startAngle[i], endAngle[j]);
		branch.transform.SetParent(transform);

	}
}
