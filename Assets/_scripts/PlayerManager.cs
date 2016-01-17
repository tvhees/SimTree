using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager> {

	// Declare variables to track across the game
	public int water;
	public int energy;
	public int generation = 1;
	public int size;
	public string season = "Spring";
	public string nextSeason;
	public List<GameObject> activeTiles = new List<GameObject> ();
	public List<GameObject> seasonTiles = new List<GameObject> ();
	public List<GameObject> treeTiles = new List<GameObject> ();
	public int[] masterIndex = new int[]{0,1,2,3,4,5,6};
	public List<int> tileIndex = new List<int> ();
	public float hexSize = 2.0f;
	public List<int> weatherList = new List<int> ();
	public int seasonIndex = 0;
	public GameObject seasonText;
	public GameObject informationPanel;
	public Camera mainCamera;
	public Camera uiCamera;
	public string environment = "No environment";
	public bool seedStart;
	public int strength;

	private GameObject treeStructure;
	private GameObject gameManager;
	private WeatherController weatherController;
	private EnvironmentController environmentController;
	private EventController eventController;
	private string[] seasons = new string[4]{"Spring", "Summer", "Autumn", "Winter"};
	private bool seedGrowth = false;
	private List<GameObject> finishedTrees = new List<GameObject> ();


	void Start(){
		Camera[] cameras = FindObjectsOfType<Camera> ();
		mainCamera = cameras [0];
		uiCamera = cameras [1];

		seasonText = GameObject.Find ("SeasonText");
		informationPanel = GameObject.Find ("InformationPanel");
		informationPanel.SetActive (false);
		treeStructure = GameObject.Find ("TreeStructure");
		gameManager = GameObject.Find ("GameManager");
		weatherController = gameManager.GetComponent<WeatherController> ();
		environmentController = gameManager.GetComponent<EnvironmentController> ();
		eventController = gameManager.GetComponent<EventController> ();

		StartGame ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			RestartGame();
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			Reproduce ();
		}
	}

	public void CreateTileIndex(){
		tileIndex.Clear();
		tileIndex.AddRange (masterIndex);
	}

	public void ResolveTile(GameObject tile, GameObject branch){
		// Tiles have a base cost for growth: 1 energy + 1 water or 1 additional energy. Seed growth requires additional energy.
		energy--;
		if (seedGrowth)
			energy--;
		if (water > 0)
			water--;
		else
			energy--;

		// Tiles may have a weather type which further modifies resources or graphics
		int tileWeather = tile.GetComponent<TreeTile> ().type;

		switch (tileWeather) {
		// No special effect for new tiles, neutral tiles, or leaf tiles
		case 0:
		case 1:
		case 5:
			break;
		case 2:
			weatherController.Fair ();
			break;
		case 3:
			weatherController.Rain ();
			break;
		case 4:
			weatherController.Sunshine ();
			break;
		case 6:
			weatherController.Frost ();
			break;
		}

		// Tiles may have a special event which typically modifies tree growth or status
		string tileEvent = tile.GetComponent<TreeTile>().eventType;

		if (tileEvent!="None")
			eventController.ResolveEvent (tileEvent, tile, branch);
	}

	void Reproduce(){
		if(generation < 3)
			NewTree ();
	}

	public void ChangeSeason(){
		season = seasons [seasonIndex];
		seasonIndex++;
		seasonIndex = (int)Mathf.Repeat (seasonIndex, 4);
		nextSeason = seasons [seasonIndex];

		weatherList.Clear ();
		strength++;
		if (seedStart)
			seedGrowth = true;
	}

	public void WeatherSelector(){
		if(weatherList.Count == 0){
			switch (nextSeason) {
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

	public void CheckState(){
		if (seasonTiles.Count < 1)
			EndGame ();
		else if (energy < 1)
			EndGame ();
		else if (season == "Spring" && seedGrowth)
			Reproduce();
		else if (activeTiles.Count < 1) {
			treeStructure.GetComponent<TreeManager> ().ChangeSeason ();
			mainCamera.GetComponent<CameraController> ().ZoomFit ();
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

	void StartGame(){
		activeTiles.Clear ();
		seasonTiles.Clear ();
		treeTiles.Clear ();
		weatherList.Clear ();
		uiCamera.gameObject.SetActive (true);
		seasonText.SetActive (true);

		water = 4;
		energy = 4;
		size = 3;
		strength = 1;
		seasonIndex = 0;

		mainCamera.orthographicSize = 20;
		mainCamera.transform.position = new Vector3 (0, 1, -10);

		treeStructure.GetComponent<TreeManager> ().NewTree();
		seedStart = false;
		seedGrowth = false;
		weatherController.Fair ();
		environmentController.NewEnvironment ();
	}

	void RestartGame(){
		foreach (Transform child in treeStructure.transform)
			Destroy (child.gameObject);

		GameObject seasonHolder = GameObject.Find ("CurrentSeason");
		if (seasonHolder != null)
			Destroy (seasonHolder);

		foreach (GameObject tree in finishedTrees)
			Destroy (tree);

		finishedTrees.Clear();

		generation = 1;

		StartGame ();
	}

	void NewTree(){
		GameObject seasonHolder = GameObject.Find ("CurrentSeason");

		if (seasonHolder != null)
			Destroy (seasonHolder);

		foreach (GameObject child in activeTiles)
			Destroy (child);

		Destroy(GameObject.Find ("Ground (Clone)"));

		finishedTrees.Add(new GameObject ("FinishedTree"));

		GameObject oldTree  = finishedTrees [finishedTrees.Count - 1];

		Transform[] treeComponents = treeStructure.transform.GetComponentsInChildren<Transform> ();
		foreach (Transform child in treeComponents) {
			if(child != child.root)
				child.SetParent (oldTree.transform);
		}


		oldTree.transform.Translate(new Vector3(-60f, 0f, 0f));

		oldTree.SetActive(false);

		generation++;

		StartGame ();
	}
}
