using UnityEngine;
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

	void OnMouseDown(){
		if(weather != "Inactive")
			PlayerManager.Instance.informationPanel.GetComponent<InformationPanelController>().InfoPanelOn(transform.position, season, weather);
	}

	void OnMouseExit(){
		PlayerManager.Instance.informationPanel.SetActive (false);
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
}
