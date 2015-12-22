using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager> {

	// Declare variables to track across the game
	public int water;
	public int energy;
	public int generation;
	public int size;
	public string season;
	public string nextSeason;
	public List<GameObject> activeTiles = new List<GameObject> ();
	public List<GameObject> seasonTiles = new List<GameObject> ();
	public List<GameObject> treeTiles = new List<GameObject> ();
	public int[] masterIndex = new int[]{0,1,2,3,4,5,6};
	public List<int> tileIndex = new List<int> ();
	public float hexSize = 2.0f;
	public List<int> weatherList = new List<int> ();
	public GameObject camera1;
	public GameObject informationPanel;
	public GameObject camera2;
	public int seasonIndex = 0;
	public GameObject seasonText;

	private string[] seasons = new string[4]{"Spring", "Summer", "Autumn", "Winter"};
	private GameObject mainCamera;
	private GameObject uiCamera;

	void Start(){
		RestartGame ();
		/*water = 3;
		energy = 3;
		generation = 1;
		size = 3;*/

		Debug.Log ("Inbetween");

		Debug.Log (FindObjectsOfType<Camera> ().Length);

		seasonText = GameObject.Find ("SeasonText");
		informationPanel = GameObject.Find ("InformationPanel");
		informationPanel.SetActive (false);

		ChangeSeason ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			RestartGame();
		}
	}

	public void CreateTileIndex(){
		tileIndex.Clear();
		tileIndex.AddRange (masterIndex);
	}

	public void ResolveTile(GameObject tile){
		// 1 energy required for growth
		energy--;

		// 1 water evaporation, tax of one energy if not available
		if (water > 0)
			water--;
		else
			energy--;

		int tileWeather = tile.GetComponent<TreeTile> ().type;

		switch (tileWeather) {
		case 0:
		case 1:
		case 2:
		case 5:
			break;
		case 3:
			Rain();
			break;
		case 4:
			Sunshine();
			break;
		case 6:
			Frost();
			break;
		}
	}

	void Rain(){
		water += 4;
	}

	void Sunshine(){
		if (water > 0) {
			for (int i = 0; i < 2; i++) {
				if (water > 0) {
					water--;
					energy += 2;
				} 
			}
		}
		else
			energy--;
	}

	void Flower(){
		if (energy > 3) {
			// Start new tree!
		}
	}

	void Frost(){
		energy -= 1;
	}

	public void ChangeSeason(){
		season = seasons [seasonIndex];
		seasonIndex++;
		seasonIndex = (int)Mathf.Repeat (seasonIndex, 4);
		nextSeason = seasons [seasonIndex];
		weatherList.Clear ();
	}

	public void WeatherSelector(){
		if(weatherList.Count == 0){
			switch (season) {
			case "Spring":
				weatherList.AddRange (new int[4] {4, 4, 3, 2});
				break;
			case "Summer":
				weatherList.AddRange (new int[4] {4, 4, 2, 2});
				break;
			case "Autumn":
				weatherList.AddRange (new int[4] {4, 3, 2, 2});
				break;
			case "Winter":
				weatherList.AddRange (new int[4] {3, 3, 2, 6});
				break;
			}
		}
	}

	public void EndGame(){
		uiCamera.gameObject.SetActive(false);
		seasonText.SetActive (false);
		mainCamera.GetComponent<CameraController>().ZoomOut ();
		foreach (GameObject tile in activeTiles)
			Destroy(tile);
		foreach (GameObject tile in seasonTiles)
			Destroy(tile);
	}

	void RestartGame(){
		SceneManager.LoadScene ("Main");
		water = 3;
		energy = 3;
		generation = 1;
		size = 3;
		tileIndex.Clear ();
		activeTiles.Clear ();
		seasonTiles.Clear ();
		treeTiles.Clear ();
		weatherList.Clear ();
	}
}
