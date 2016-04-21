using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Height : MonoBehaviour {

    public GameController game;
    public Text textString;

    private float value;

    void Update() {
        value = Mathf.MoveTowards(value, game.size, 2 * Time.deltaTime);
        textString.text = value.ToString("F2") + " m";
    }
}
