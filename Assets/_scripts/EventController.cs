using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityExtensions;
using System.Linq;

public class EventController : MonoBehaviour
{

    public List<string> springEvents = new List<string>();
    public List<string> summerEvents = new List<string>();
    public List<string> autumnEvents = new List<string>();
    public List<string> winterEvents = new List<string>();

    private GameObject branch;
    private GameObject tile;
    private List<bool> eventTrigger = new List<bool>();

    public void ResolveEvent(string eventName, GameObject currentTile, GameObject currentBranch)
    {
        branch = currentBranch;

        switch (eventName)
        {
            case "Wildfire":
                Wildfire();
                break;
            case "Flood":
                Flood();
                break;
            case "Disease":
                Disease();
                break;
            case "Wind":
                Wind();
                break;
            case "Lightning":
                Lightning();
                break;
            case "Insect":
                Insect();
                break;
            case "Bird":
                Bird();
                break;
        }

    }

    void Wildfire()
    {
        // Prevent current branch from growing anywhere
        branch.GetComponent<TreeTile>().destroyTag = true;
    }

    void Flood()
    {
        // Double the amount of water given
        PlayerManager.Instance.water = Mathf.FloorToInt(PlayerManager.Instance.water * 3 / 2);
    }

    void Disease()
    {
        // Weaken the tree
        PlayerManager.Instance.strength -= 3;

        if (PlayerManager.Instance.strength < 0)
            PlayerManager.Instance.strength = 0;
    }

    void Wind()
    {
        GetComponentInParent<WeatherController>().Wind();

        // Destroy the tree if it is weak
        if (PlayerManager.Instance.strength < 3)
            PlayerManager.Instance.EndGame();
        // Pollinate in spring
        else if (PlayerManager.Instance.season == "Spring")
            PlayerManager.Instance.seedStart = true;
    }

    void Lightning()
    {
        // Destroy the tree if it is weak
        if (PlayerManager.Instance.strength < 3)
            PlayerManager.Instance.EndGame();
        // Create wildfire if the tree is dry
        if (PlayerManager.Instance.water < 1)
            Wildfire();
    }

    void Insect()
    {
        // Pollinate in spring, otherwise weaken the tree
        if (PlayerManager.Instance.season == "Spring")
            PlayerManager.Instance.seedStart = true;
        else
            PlayerManager.Instance.strength--;

        if (PlayerManager.Instance.strength < 0)
            PlayerManager.Instance.strength = 0;
    }

    void Bird()
    {
        // Remove insect events from all visible tiles
    }

    public string GetEvent()
    {
        if (eventTrigger.Count < 1)
            eventTrigger.AddRange(new bool[3] { false, false, true });

        var (trigger, index) = eventTrigger.Random();
        eventTrigger.RemoveAt(index);

        if (!trigger)
        {
            return "None";
        }

        switch (PlayerManager.Instance.nextSeason)
        {
            case "Spring":
                return Spring();
            case "Summer":
                return Summer();
            case "Autumn":
                return Autumn();
            case "Winter":
                return Winter();
        }

        throw new System.Exception("Could not get appropriate event name for season " + PlayerManager.Instance.nextSeason);
    }

    private string PopEvent(ref List<string> events)
    {
        var (sEvent, index) = events.Random();
        events.RemoveAt(index);

        return sEvent;
    }

    // TODO: Store event string arrays in data
    private string Spring()
    {
        if (!springEvents.Any())
        {
            springEvents.AddRange(new string[] { "Wind", "Lightning", "Flood", "Insect", "Insect", "Insect", "Insect", "Disease" });
        }

        return PopEvent(ref springEvents);
    }

    private string Summer()
    {
        if (!summerEvents.Any())
            summerEvents.AddRange(new string[] { "Lightning", "Wildfire", "Wildfire", "Wildfire", "Wind", "Wind", "Insect", "Insect" });

        return PopEvent(ref summerEvents);
    }

    private string Autumn()
    {
        if (!autumnEvents.Any())
            autumnEvents.AddRange(new string[] { "Lightning", "Lightning", "Wind", "Wind", "Flood", "Flood", "Disease", "Insect" });

        return PopEvent(ref autumnEvents);
    }

    private string Winter()
    {
        if (!winterEvents.Any())
            winterEvents.AddRange(new string[] { "Lightning", "Wind", "Wind", "Flood", "Flood", "Flood", "Disease", "Disease" });

        return PopEvent(ref winterEvents);
    }
}
