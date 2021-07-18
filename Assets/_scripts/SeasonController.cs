using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    public List<GameObject> AddTiles(List<GameObject> tileListIn, TileType tileType)
    {
        var newTiles = new List<GameObject>();

        tileListIn.Select(tile => tile.GetComponent<TreeTile>())
            .SelectMany(tile => tile.directionsUp
                .Select((connected, i) =>
                new
                {
                    type = connected ? tileType : TileType.Leaves,
                    tile.transform.position,
                    i
                })
            ).ToList()
            .ForEach(direction =>
            {
                newTiles.Add(
                    NewTile(direction.i, direction.position, direction.type)
                );
            });

        return newTiles.Where(tile => tile).ToList();
    }

    public GameObject NewTile(int i, Vector3 oldPosition, TileType tileType)
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

        if (tileType != TileType.Leaves)
        {
            PlayerManager.Instance.WeatherSelector();
            int j = Random.Range(0, PlayerManager.Instance.weatherList.Count);
            tileType = (TileType)PlayerManager.Instance.weatherList[j];
            PlayerManager.Instance.weatherList.RemoveAt(j);
        }

        newTile.GetComponent<TreeTile>().UpdateTile(tileType, newPosition, directions, false, true, true);
        return newTile;
    }
}
