using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class TreeManager : MonoBehaviour
{

    public GameObject treeRoot;
    public GameObject seasonHolder;
    public GameObject highlightRing;
    public float hexSize = 2;

    public void NewTree()
    {
        Instantiate(treeRoot);
    }

    void AddToActiveTiles(GameObject branch)
    {
        PlayerManager.Instance.activeTiles.Add(branch);
        branch.GetComponent<TreeTile>().tileRenderer.sortingLayerName = "Tree";
    }

    void DestroyTile(GameObject branch)
    {
        PlayerManager.Instance.activeTiles.Remove(branch);
        Destroy(branch);
    }

    void AddToTreeParentAndLayer(GameObject branch)
    {
        // TODO: Fix bug where Wildfire destroyed tiles no longer exist here
        branch.transform.SetParent(transform);
        branch.layer = LayerMask.NameToLayer("TreeTiles");
    }

    bool IsConnectedDownwards(GameObject branch)
    {
        return branch.GetComponent<TreeTile>().directionsDown.Any(d => d);
    }

    void ActivateAndHighlightBranch(GameObject branch)
    {
        branch.transform.SetParent(transform);
        branch.layer = LayerMask.NameToLayer("ActiveTiles");
        GameObject highlight = Instantiate(highlightRing) as GameObject;
        highlight.transform.position = branch.transform.position - new Vector3(0.0f, 0.0f, 1.0f);
        highlight.transform.SetParent(branch.transform);
    }

    void CreateSeasonHolder()
    {
        GameObject instance = Instantiate(seasonHolder);
        instance.name = "CurrentSeason";
        instance.GetComponent<SeasonController>().AddTiles(PlayerManager.Instance.activeTiles, PlayerManager.Instance.seasonTiles, 2);
    }

    public void ChangeSeason()
    {
        PlayerManager.Instance.seasonTiles.ForEach(AddToActiveTiles);
        PlayerManager.Instance.treeTiles.ForEach(AddToTreeParentAndLayer);

        // Create a temporary list of tiles that need to be destroyed later
        List<GameObject> destroyList = new List<GameObject>();

        PlayerManager.Instance.activeTiles
            .Where(IsConnectedDownwards).ToList()
            .ForEach(ActivateAndHighlightBranch);

        var nextTiles = PlayerManager.Instance.activeTiles
            .ToLookup(IsConnectedDownwards);

        nextTiles[true].ToList().ForEach(ActivateAndHighlightBranch);
        nextTiles[false].ToList().ForEach(DestroyTile);

        // Move the whole tree downwards, add new season tiles
        transform.Translate(new Vector3(0.0f, -Mathf.Sqrt(3) * hexSize, 0.0f));
        PlayerManager.Instance.ChangeSeason();
    }
}
