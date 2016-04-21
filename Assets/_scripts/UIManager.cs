using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameController game;
    public ResourceMeter waterMeter, energyMeter, strengthMeter;

	// Update is called once per frame
	void Update () {
            waterMeter.UpdateMeter(game.water);
            energyMeter.UpdateMeter(game.energy);
            strengthMeter.UpdateMeter(game.strength);
	}

    public void ShowMenu() {
        PlayerManager.Instance.menuCamera.enabled = true;
    }
}
