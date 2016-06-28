﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    // Game data variables
    public int water, energy, size, strength, seasonIndex, generation, goalSize;
    public bool seedStart, seedGrowth;
    public string season, nextSeason;

    // Script and object references
    public TreeManager treeManager;
    public RootController rootController;
    public WeatherController weatherController;
    public EnvironmentController environmentController;
    public EventController eventController;
    public NewTileManager newTileManager;
    public Camera uiCamera;
    public GameObject informationPanel;

    // Tracking lists
    public List<GameObject> activeTiles = new List<GameObject>(),
        seasonTiles = new List<GameObject>(),
        treeTiles = new List<GameObject>();
    public List<int> weatherList = new List<int>(), tileIndex = new List<int>();
    public int[] masterIndex = new int[] { 0, 1, 2, 3, 4, 5, 6 };
    private string[] seasons = new string[4] { "Spring", "Summer", "Autumn", "Winter" };
    private List<GameObject> finishedTrees = new List<GameObject>();

    // Game state variable
    public enum State {
        INTRO,
        PLAY,
        WIN,
        LOSE
    }

    public State state;

    void Start() {
        state = State.INTRO;
        
        PlayerManager.Instance.game = this;

        water = 4;
        energy = 4;
        size = 1;
        strength = 1;
        seasonIndex = 0;
        generation = 1;
        season = "Spring";
        goalSize = PlayerManager.Instance.goalSize;

        PlayerManager.Instance.mainCamera.transform.position = new Vector3(0, 1, -10);
        PlayerManager.Instance.uiCamera = uiCamera;

        treeManager.NewTree();
        seedStart = false;
        seedGrowth = false;
        weatherController.Fair();
        environmentController.NewEnvironment();

        informationPanel.SetActive(false);

        newTileManager.CreateTileSelection();
    }

    public void CreateTileIndex()
    {
        tileIndex.Clear();
        tileIndex.AddRange(masterIndex);
    }

    public void WeatherSelector()
    {
        if (weatherList.Count == 0)
        {
            switch (nextSeason)
            {
                case "Spring":
                    weatherList.AddRange(new int[4] { 4, 4, 3, 2 });
                    break;
                case "Summer":
                    weatherList.AddRange(new int[4] { 4, 4, 2, 2 });
                    break;
                case "Autumn":
                    weatherList.AddRange(new int[4] { 4, 3, 2, 2 });
                    break;
                case "Winter":
                    weatherList.AddRange(new int[4] { 3, 3, 2, 6 });
                    break;
            }
        }
    }

    public void ResolveTile(GameObject tile, GameObject branch)
    {
        // Tiles have a base cost for growth: 1 energy + 1 water or 1 additional energy. Seed growth requires additional energy.
        energy--;
        if (seedGrowth)
            energy--;
        if (water > 0)
            water--;
        else
            energy--;

        // Tiles may have a weather type which further modifies resources or graphics
        int tileWeather = tile.GetComponent<TreeTile>().type;

        switch (tileWeather)
        {
            // No special effect for new tiles, neutral tiles, or leaf tiles
            case 0:
            case 1:
            case 5:
                break;
            case 2:
                weatherController.Fair();
                break;
            case 3:
                weatherController.Rain();
                break;
            case 4:
                weatherController.Sunshine();
                break;
            case 6:
                weatherController.Frost();
                break;
        }

        // Tiles may have a special event which typically modifies tree growth or status
        string tileEvent = tile.GetComponent<TreeTile>().eventType;

        if (tileEvent != "None")
            eventController.ResolveEvent(tileEvent, tile, branch);
    }

    public void CheckState()
    {
        if (state == State.INTRO)
            rootController.StartingTiles();
        else if (seasonTiles.Count < 1)
            EndGame();
        else if (energy < 1)
            EndGame();
        else if (season == "Spring" && seedGrowth)
            Reproduce();
        else if (activeTiles.Count < 1)
        {
            treeManager.ChangeSeason();
            if (size >= goalSize)
                EndGame(State.WIN);
            else
                StartCoroutine(PlayerManager.Instance.mainCamera.GetComponent<CameraController>().ZoomFit(2*Mathf.Sqrt(3)));
        }
    }

    public void ChangeSeason()
    {
        season = seasons[seasonIndex];
        seasonIndex++;
        seasonIndex = (int)Mathf.Repeat(seasonIndex, 4);
        nextSeason = seasons[seasonIndex];

        weatherList.Clear();
        strength++;
        size++;
        if (seedStart)
            seedGrowth = true;
    }

    public void Reproduce()
    {
        if (generation < 3)
            NewTree();
    }

    public void EndGame(State result = State.LOSE)
    {
        state = result;

        if (result == State.WIN)
        {
            PlayerManager.Instance.goalSize++;
        }
        else
        {
            PlayerManager.Instance.goalSize = Mathf.Max(PlayerManager.Instance.goalSize - 1, 3);
        }

        uiCamera.gameObject.SetActive(false);
        StartCoroutine(PlayerManager.Instance.mainCamera.GetComponent<CameraController>().ZoomOut());
        foreach (GameObject tile in activeTiles)
            Destroy(tile);
        foreach (GameObject tile in seasonTiles)
            Destroy(tile);
    }

    void NewTree()
    {
        GameObject seasonHolder = GameObject.Find("CurrentSeason");

        if (seasonHolder != null)
            Destroy(seasonHolder);

        foreach (GameObject child in activeTiles)
            Destroy(child);

        Destroy(GameObject.Find("Ground (Clone)"));

        finishedTrees.Add(new GameObject("FinishedTree"));

        GameObject oldTree = finishedTrees[finishedTrees.Count - 1];

        Transform[] treeComponents = treeManager.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in treeComponents)
        {
            if (child != child.root)
                child.SetParent(oldTree.transform);
        }


        oldTree.transform.Translate(new Vector3(-60f, 0f, 0f));

        oldTree.SetActive(false);

        generation++;

        StartGame();
    }

    void StartGame()
    {
        activeTiles.Clear();
        seasonTiles.Clear();
        treeTiles.Clear();
        weatherList.Clear();
    }

    public void ResetGoal()
    {
        goalSize = PlayerManager.Instance.goalSize = 5;
    }
}