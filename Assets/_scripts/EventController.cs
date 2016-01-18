using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController : MonoBehaviour {

	public List<string> springEvents = new List<string> ();
	public List<string> summerEvents = new List<string> ();
	public List<string> autumnEvents = new List<string> ();
	public List<string> winterEvents = new List<string> ();

	private GameObject branch;
	private GameObject tile;
	private List<bool> eventTrigger = new List<bool>();

	public void ResolveEvent(string eventName, GameObject currentTile, GameObject currentBranch){
		branch = currentBranch;

		switch (eventName) {
		case "Wildfire":
			Wildfire ();
			break;
		case "Flood":
			Flood ();
			break;
		case "Disease":
			Disease ();
			break;
		case "Wind":
			Wind ();
			break;
		case "Lightning":
			Lightning ();
			break;
		case "Insect":
			Insect ();
			break;
		case "Bird":
			Bird ();
			break;
		}

	}

	void Wildfire(){
		// Prevent current branch from growing anywhere
		branch.GetComponent<TreeTile>().destroyTag = true;
		Debug.Log ("fire");
	}

	void Flood(){
		// Double the amount of water given
		PlayerManager.Instance.water = Mathf.FloorToInt (PlayerManager.Instance.water * 3 / 2);
	}

	void Disease(){
		// Weaken the tree
		PlayerManager.Instance.strength -= 3;

		if (PlayerManager.Instance.strength < 0)
			PlayerManager.Instance.strength = 0;
	}

	void Wind(){
		GetComponentInParent<WeatherController> ().Wind ();

		// Destroy the tree if it is weak
		if (PlayerManager.Instance.strength < 3)
			PlayerManager.Instance.EndGame ();
		// Pollinate in spring
		else if (PlayerManager.Instance.season == "Spring")
			PlayerManager.Instance.seedStart = true;
	}

	void Lightning(){
		// Destroy the tree if it is weak
		if (PlayerManager.Instance.strength < 3)
			PlayerManager.Instance.EndGame ();
		// Create wildfire if the tree is dry
		if (PlayerManager.Instance.water < 1)
			Wildfire ();
	}

	void Insect(){
		// Pollinate in spring, otherwise weaken the tree
		if (PlayerManager.Instance.season == "Spring")
			PlayerManager.Instance.seedStart = true;
		else
			PlayerManager.Instance.strength--;

		if (PlayerManager.Instance.strength < 0)
			PlayerManager.Instance.strength = 0;
	}

	void Bird(){
		// Remove insect events from all visible tiles
	}

	public string GetEvent(){
		if (eventTrigger.Count < 1)
			eventTrigger.AddRange (new bool[3]{ false, false, true });

		int index = Random.Range (0, eventTrigger.Count);

		string eventString = "None";

		if (eventTrigger[index]){
			switch (PlayerManager.Instance.nextSeason) {
			case "Spring":
				eventString = Spring ();
				break;
			case "Summer":
				eventString = Summer ();
				break;
			case "Autumn":
				eventString = Autumn ();
				break;
			case "Winter":
				eventString = Winter ();
				break;
			}
		}

		eventTrigger.RemoveAt (index);

		return eventString;
	}

	private string Spring(){
		if (springEvents.Count < 1)
			springEvents.AddRange (new string[]{ "Wind", "Lightning", "Flood", "Insect", "Insect", "Insect", "Insect", "Disease" });

		int index = Random.Range (0, springEvents.Count);
		string returnEvent = springEvents [index];
		springEvents.RemoveAt (index);
		return returnEvent;
	}

	private string Summer(){
		if (summerEvents.Count < 1)
			summerEvents.AddRange (new string[] {"Lightning", "Wildfire", "Wildfire", "Wildfire", "Wind", "Wind", "Insect",	"Insect" });

		int index = Random.Range (0, summerEvents.Count);
		string returnEvent = summerEvents [index];
		summerEvents.RemoveAt (index);
		return returnEvent;
	}

	private string Autumn(){
		if (autumnEvents.Count < 1)
			autumnEvents.AddRange (new string[]{ "Lightning", "Lightning", "Wind", "Wind", "Flood", "Flood", "Disease", "Insect" });

		int index = Random.Range (0, autumnEvents.Count);
		string returnEvent = autumnEvents [index];
		autumnEvents.RemoveAt (index);
		return returnEvent;
	}

	private string Winter(){
		if (winterEvents.Count < 1)
			winterEvents.AddRange (new string[]{ "Lightning", "Wind", "Wind", "Flood", "Flood", "Flood", "Disease", "Disease" });

		int index = Random.Range (0, winterEvents.Count);
		string returnEvent = winterEvents [index];
		winterEvents.RemoveAt (index);
		return returnEvent;
	}
}
