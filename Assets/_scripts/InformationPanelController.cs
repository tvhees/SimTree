using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InformationPanelController : MonoBehaviour {

	public Text seasonText;
	public Text weatherText;
	public Text weatherDescription;

    void Start() {
        PlayerManager.Instance.informationPanel = gameObject;
    }

	public void InfoPanelOn(Vector3 position, string season, string weather){
		if (position.x > 0)
			GetComponent<RectTransform> ().pivot = new Vector2 (1, 0);
		else
			GetComponent<RectTransform> ().pivot = new Vector2 (0, 0);
		
		transform.position = PlayerManager.Instance.mainCamera.WorldToScreenPoint(position);
		seasonText.text = season;
		weatherText.text = weather;

		switch (weather) {
		case "Fair":
			weatherDescription.text = "No special weather conditions";
			break;
		case "Sunshine":
			weatherDescription.text = "Convert up to 2 water in to 2 energy each. If you have no water available, instead lose 1 additional energy";
			break;
		case "Rain":
			weatherDescription.text = "Gain 4 water";
			break;
		case "Frost":
			weatherDescription.text = "Lose one additional energy";
			break;
		}

		gameObject.SetActive (true);
	}
}
