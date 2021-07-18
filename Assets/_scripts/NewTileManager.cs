using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityExtensions;

public class NewTileManager : MonoBehaviour
{
    public GameObject[] hexTiles;

    // TODO: Tile spacing should take global tile scale in to account
    public float spacing = 4.0f;
    public int tilesToDisplay = 5;
    public int draggableTiles = 3;
    public GameObject unavailableTilesMarker;

    private float dragBoundary { get { return transform.position.x + spacing * (draggableTiles + 0.5f); } }
    private List<int> spawnableTileIndices = new List<int>();
    private Vector3 newTilePosition { get { return transform.position + (tilesToDisplay - 0.5f) * spacing * Vector3.right; } }

    void Start()
    {
        foreach (int i in Enumerable.Range(0, tilesToDisplay))
        {
            Vector3 position = transform.position + spacing * (i + 0.5f) * Vector3.right;
            AddTile(position);
        };

        unavailableTilesMarker.transform.localPosition = new Vector3
        {
            x = 0.5f * (tilesToDisplay + draggableTiles) * spacing,
            z = 1
        };
        unavailableTilesMarker.transform.localScale = new Vector3
        {
            x = (tilesToDisplay - draggableTiles) * spacing,
            y = spacing,
            z = 1
        };
    }

    public void UpdateSelection(Vector3 emptyPosition)
    {
        HexTile[] hexTiles = transform.GetComponentsInChildren<HexTile>();
        foreach (HexTile tile in hexTiles)
        {
            if (tile.transform.position.x > emptyPosition.x)
            {
                tile.transform.Translate(spacing * Vector3.left, Space.World);
                if (tile.transform.position.x < dragBoundary)
                    tile.GetComponent<TreeTile>().draggable = true;
            }
        }

        AddTile(newTilePosition);
    }

    void AddTile(Vector3 tilePosition)
    {
        if (!spawnableTileIndices.Any())
        {
            spawnableTileIndices = Enumerable.Range(0, hexTiles.Length).ToList();
        }

        (int tileIndex, int i) = spawnableTileIndices.Random();
        spawnableTileIndices.RemoveAt(i);

        GameObject instance = Instantiate(hexTiles[tileIndex], transform);
        // TODO: Check if tagging is appropriate or used
        instance.tag = "InactiveBranch";

        instance.GetComponent<TreeTile>().UpdateTile(TileType.NewTile, tilePosition);

        if (instance.transform.position.x < dragBoundary)
            instance.GetComponent<TreeTile>().draggable = true;
    }
}
