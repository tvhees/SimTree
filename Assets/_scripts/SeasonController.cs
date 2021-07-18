using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeasonController : MonoBehaviour
{

    public GameObject seasonTile;

    private Vector3 newPosition;
    private float hexOffset;
    private bool[] directions = new bool[3] { true, true, true };

    void Awake()
    {
        hexOffset = Mathf.Sqrt(3) * 0.5f * PlayerManager.Instance.hexSize;
    }

    void Update()
    {
        if (transform.childCount < 1)
            Destroy(gameObject);
    }

    /// <summary>
    ///    Adds a new tile for each possible upward direction from the tiles provided in <paramref name="tileListIn" />, then clears <paramref name="tileListOut" /> and adds the new tiles to it. New tiles are of type <paramref name="tileType" /> unless no existing tile can reach them.
    /// </summary>
    /// <param name="tileListIn"></param>
    /// <param name="tileListOut"></param>
    /// <param name="tileType"></param>
    /// TODO: Return new list instead of mutating tileListOut, make tileType an enum
    public List<GameObject> AddTiles(List<GameObject> tileListIn, int tileType)
    {
        var newTiles = new List<GameObject>();

        tileListIn.ForEach(tile =>
        {
            TreeTile tileScript = tile.GetComponent<TreeTile>();
            for (int i = 0; i < 3; i++)
            {
                GameObject newTile;
                if (tileScript.directionsUp[i])
                    newTile = NewTile(i, tile.transform.position, tileType);
                else
                    newTile = NewTile(i, tile.transform.position, 5);

                if (newTile)
                    newTiles.Add(newTile);
            }
        });

        return newTiles;
    }

    public GameObject NewTile(int i, Vector3 oldPosition, int tileType)
    {
        newPosition = oldPosition;
        switch (i)
        {
            case 0:
                newPosition += new Vector3(-Mathf.Sqrt(3) * hexOffset, hexOffset, 0.0f);
                break;
            case 1:
                newPosition += new Vector3(0, hexOffset * 2, 0.0f);
                break;
            case 2:
                newPosition += new Vector3(Mathf.Sqrt(3) * hexOffset, hexOffset, 0.0f);
                break;
        }

        Collider2D hit = Physics2D.OverlapPoint(newPosition);

        if (hit)
        {
            return null;
        }

        GameObject newTile = Instantiate(seasonTile);
        newTile.transform.SetParent(transform);

        if (tileType != 5)
        {
            PlayerManager.Instance.WeatherSelector();
            int j = Random.Range(0, PlayerManager.Instance.weatherList.Count);
            tileType = PlayerManager.Instance.weatherList[j];
            PlayerManager.Instance.weatherList.RemoveAt(j);
        }

        newTile.GetComponent<TreeTile>().UpdateTile(tileType, newPosition, directions, false, true, true);
        return newTile;
    }
}
