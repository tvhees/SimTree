using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameController game;
    public Camera uiCamera;
    public ResourceMeter waterMeter, energyMeter, strengthMeter;
    public GameObject optionsPanel;

	// Update is called once per frame
	void Update () {
        switch (game.state)
        {
            case GameController.State.INTRO:
                waterMeter.UpdateMeter(game.water);
                energyMeter.UpdateMeter(game.energy);
                strengthMeter.UpdateMeter(game.strength);
                ShowCameras(true, false, false);
                break;
            case GameController.State.PLAY:
                waterMeter.UpdateMeter(game.water);
                energyMeter.UpdateMeter(game.energy);
                strengthMeter.UpdateMeter(game.strength);
                ShowCameras(false, true, false);
                break;
            case GameController.State.WIN:
            case GameController.State.LOSE:
                ShowCameras(false, false, true);
                break;
        }
	}

    public void ShowCameras(bool menu, bool ui, bool end)
    {
        if(PlayerManager.Instance.menuCamera != null)
            PlayerManager.Instance.menuCamera.enabled = menu;
        if (uiCamera != null)
            uiCamera.enabled = ui;
        if (PlayerManager.Instance.endCamera != null)
            PlayerManager.Instance.endCamera.enabled = end;
    }

    public void ToggleOptions()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }
}
