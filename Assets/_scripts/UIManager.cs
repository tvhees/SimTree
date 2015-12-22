using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public Text waterText;
	public Text energyText;
	public Text generationText;
	public Text sizeText;
	public Text seasonText;
	public Text nextSeasonText;

	private string[] seasons = new string[]{"Autumn", "Winter", "Spring", "Summer"};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		waterText.text = "Water: " + PlayerManager.Instance.water;
		energyText.text = "Energy: " + PlayerManager.Instance.energy;
		generationText.text = "Generation: " + PlayerManager.Instance.generation;
		sizeText.text = "Size: " + PlayerManager.Instance.size;
		seasonText.text = seasons[PlayerManager.Instance.seasonIndex];
	}
}
