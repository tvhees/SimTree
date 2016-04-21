using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager> {

	// Declare variables to track across the game
	public float hexSize = 2.0f;
	public GameObject informationPanel;
	public Camera mainCamera, menuCamera, uiCamera;
	public string environment = "No environment";
    public GameController gameController;


	IEnumerator Start(){
        yield return StartCoroutine(LoadScenes());
	}

    IEnumerator LoadScenes() {
        if (SceneManager.GetSceneByName("Game").isLoaded)
            SceneManager.UnloadScene("Game");

        SceneManager.LoadScene("Game", LoadSceneMode.Additive);

        if (SceneManager.GetSceneByName("Menu").isLoaded)
            SceneManager.UnloadScene("Menu");

        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.0001f);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
    }

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
            
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			gameController.Reproduce ();
		}
	}
}
