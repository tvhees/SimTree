using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventSpriteManager : MonoBehaviour {

	public Sprite diseaseSprite;
	public Sprite windSprite;
	public Sprite lightningSprite;
	public Sprite insectSprite;
	public Sprite wildfireSprite;
	public Sprite floodSprite;
	public SpriteRenderer eventSprite;

	private GameObject gameManager;
	private EventController eventController;

	public string SetEvent(){
		gameManager = GameObject.Find ("GameManager");
		eventController = gameManager.GetComponent<EventController> ();
		string eventString = eventController.GetEvent ();

		eventSprite = GetComponent<SpriteRenderer> ();

		switch (eventString) {
		case "Disease":
			eventSprite.sprite = diseaseSprite;
			break;
		case "Wind":
			eventSprite.sprite = windSprite;
			break;
		case "Lightning":
			eventSprite.sprite = lightningSprite;
			break;
		case "Insect":
			eventSprite.sprite = insectSprite;
			break;
		case "Wildfire":
			eventSprite.sprite = wildfireSprite;
			break;
		case "Flood":
			eventSprite.sprite = floodSprite;
			break;
		}

		return eventString;
	}
}
