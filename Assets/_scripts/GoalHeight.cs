using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalHeight : MonoBehaviour
{

    public GameController game;
    public Text textString;

    void Update()
    {
        textString.text = "Goal: " + game.goalSize.ToString("F2") + " m";
    }
}
