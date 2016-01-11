using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour {

	private GameObject tile;

	public void ResolveEvent(string eventName, GameObject currentTile){
		Invoke (eventName, 0f);
	}

	void Wildfire(){
		// Destroy the current branch
	}

	void Flooding(){
		// Double the amount of water given
	}

	void Disease(){
		// Weaken the tree
		PlayerManager.Instance.strength--;
	}

	void Wind(){
		// Destroy the tree if it is weak
		if (PlayerManager.Instance.strength * 3 < PlayerManager.Instance.size)
			PlayerManager.Instance.EndGame ();
		// Pollinate in spring
		else if (PlayerManager.Instance.season == "Spring")
			PlayerManager.Instance.seedGrowth = true;
	}

	void Lightning(){
		// Destroy the tree if it is weak
		if (PlayerManager.Instance.strength * 3 < PlayerManager.Instance.size)
			PlayerManager.Instance.EndGame ();
		// Create wildfire if the tree is dry
		if (PlayerManager.Instance.water < 1)
			Wildfire ();
	}

	void Insect(){
		// Pollinate in spring, otherwise weaken the tree
		if (PlayerManager.Instance.season == "Spring")
			PlayerManager.Instance.seedGrowth = true;
		else
			PlayerManager.Instance.strength--;
	}

	void Bird(){
		// Remove insect events from all visible tiles
	}
}
