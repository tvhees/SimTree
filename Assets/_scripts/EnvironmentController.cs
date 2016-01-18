using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour {

	private string[] enviroList = new string[]{"Desert", "Tundra", "Forest", "Savannah"};

	public void NewEnvironment(){
		string environment = enviroList [Random.Range (0, enviroList.Length)];

		Invoke (environment, 0.0f);
	}

	public void Desert(){
		PlayerManager.Instance.environment = "Desert";
	}

	public void Tundra(){
		PlayerManager.Instance.environment = "Tundra";
	}

	public void Forest(){
		PlayerManager.Instance.environment = "Forest";
	}

	public void Savannah(){
		PlayerManager.Instance.environment = "Savannah";
	}

}
