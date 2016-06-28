using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InformationPanelController : MonoBehaviour {

	public Text seasonText;
	public Text weatherText;
	public Text weatherDescription;
    public Text eventText;
    public Text eventDescription;

    void Start() {
        PlayerManager.Instance.informationPanel = gameObject;
    }

	public void InfoPanelOn(Vector3 position, string season, string weather, string eventType){
		if (position.x > 0)
			GetComponent<RectTransform> ().pivot = new Vector2 (0.75f, -0.15f);
		else
			GetComponent<RectTransform> ().pivot = new Vector2 (0.25f, -0.15f);
		
        Vector3 screenPos = PlayerManager.Instance.mainCamera.WorldToScreenPoint(position);
        transform.position = screenPos;

		seasonText.text = season;
		weatherText.text = weather;
        eventText.text = eventType;

		switch (weather) {
		case "Fair":
			weatherDescription.text = "No special weather conditions";
			break;
		case "Sunshine":
			weatherDescription.text = "-2 water --> +4 energy. If water = 0, instead -1 energy";
			break;
		case "Rain":
			weatherDescription.text = "+4 water";
			break;
		case "Frost":
			weatherDescription.text = "-1 energy";
			break;
		}

        switch (eventType)
        {
            case "None":
                eventText.text = "";
                eventDescription.text = "";
                break;
            case "Disease":
                eventDescription.text = "-3 tree strength";
                break;
            case "Wind":
                eventDescription.text = "If tree strength < 4, destroy the tree";
                break;
            case "Lightning":
                eventDescription.text = "If tree strength is < 4, destroy the tree. If water = 0, start a wildfire (destroy the branch)";
                break;
            case "Insect":
                eventDescription.text = "-1 tree strength in Summer, Autumn or Winter";
                break;
            case "Wildfire":
                eventDescription.text = "Destroys all tiles growing from this branch.";
                break;
            case "Flood":
                eventDescription.text = "+50% total water";
                break;
        }

		gameObject.SetActive (true);
	}
}
