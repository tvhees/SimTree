using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGame : MonoBehaviour {

    public Camera endCamera;
    public Text winLoseText;

    void Awake()
    {
        PlayerManager.Instance.endCamera = endCamera;
    }

    void Update()
    {
        if (PlayerManager.Instance.game.state == GameController.State.WIN)
            winLoseText.text = "Target height of " + PlayerManager.Instance.game.goalSize.ToString() + " m reached. Your tree survived!";
        else
            winLoseText.text = "Target height of " + PlayerManager.Instance.game.goalSize.ToString() + " m not reached. Your tree did not survive";
    }
}
