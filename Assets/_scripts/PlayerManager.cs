using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager> {

	// Declare variables to track across the game
	public float hexSize = 2.0f;
    public int goalSize = 5;
	public GameObject informationPanel, treeManager;
	public Camera mainCamera, menuCamera, uiCamera, endCamera;
	public string environment = "No environment";
    public GameController game;


	public IEnumerator Start(){
        yield return StartCoroutine(LoadScenes());

        mainCamera.transform.position = new Vector3(0f, 1f, -10f);
        mainCamera.orthographicSize = 20f;
	}

    IEnumerator LoadScenes() {
        if (SceneManager.GetSceneByName("Menu").isLoaded)
            SceneManager.UnloadScene("Menu");

        if (SceneManager.GetSceneByName("EndGame").isLoaded)
            SceneManager.UnloadScene("EndGame");

        if (SceneManager.GetSceneByName("Game").isLoaded)
            SceneManager.UnloadScene("Game");

        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        SceneManager.LoadScene("EndGame", LoadSceneMode.Additive);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.0001f);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
    }

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
            
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			game.Reproduce ();
		}
	}
}
