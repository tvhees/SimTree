using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public Text waterText;
	public Text energyText;
	public Text generationText;
	public Text sizeText;
	public Text seasonText;
	public Text environmentText;
	public Text strengthText;

	private string[] seasons = new string[]{"Autumn", "Winter", "Spring", "Summer"};

	// Update is called once per frame
	void Update () {
		if (waterText != null) {
			waterText.text = "Water: " + PlayerManager.Instance.water;
			energyText.text = "Energy: " + PlayerManager.Instance.energy;
			generationText.text = "Generation: " + PlayerManager.Instance.generation;
			sizeText.text = "Size: " + PlayerManager.Instance.size;
			seasonText.text = seasons [PlayerManager.Instance.seasonIndex];
			environmentText.text = PlayerManager.Instance.environment;
			strengthText.text = "Strength: " + PlayerManager.Instance.strength;
		}
	}
}
