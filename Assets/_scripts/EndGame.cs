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
            winLoseText.text = "your tree survived!";
        else
            winLoseText.text = "your tree did not survive";
    }
}
