using UnityEngine;
using System.Collections;

public class EventSpriteManager : MonoBehaviour {

	public Sprite diseaseSprite;
	public Sprite windSprite;
	public Sprite lightningSprite;
	public Sprite insectSprite;
	public SpriteRenderer eventSprite;

	public string SetEvent(){
		int eventType = Random.Range (0, 10);

		if (eventType == 6) {
			eventSprite.sprite = diseaseSprite;
			return "Disease";
		} else if (eventType == 7) {
			eventSprite.sprite = windSprite;
			return "Wind";
		} else if (eventType == 8) {
			eventSprite.sprite = lightningSprite;
			return "Lightning";
		} else if (eventType == 9) {
			eventSprite.sprite = insectSprite;
			return "Insect";
		}
		else
			return "None";
	}


}
