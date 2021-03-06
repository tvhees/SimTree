﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RootController : MonoBehaviour
{

    public GameObject root;
    public GameObject[] trunkParts;
    public GameObject ground;

    private float hexOffset;



    // Use this for initialization
    void Awake()
    {
        transform.SetParent(GameObject.Find("TreeStructure").transform);

        hexOffset = Mathf.Sqrt(3) * 0.5f * PlayerManager.Instance.hexSize;
        bool[] tileDirections = new bool[3] { true, true, true };
        bool[] rootDirections = new bool[3];
        TileType tileType = TileType.Sunshine;
        Vector3 newPosition = new Vector3(0, 0, 0);
        TreeTile[] startingTiles = GetComponentsInChildren<TreeTile>();
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    newPosition = new Vector3(-Mathf.Sqrt(3) * hexOffset, hexOffset, 0.0f);
                    tileType = TileType.Sunshine;
                    rootDirections = new bool[3] { false, false, true };
                    break;
                case 1:
                    newPosition = new Vector3(0, hexOffset * 2, 0.0f);
                    tileType = TileType.Leaves;
                    rootDirections = new bool[3] { false, false, false };
                    break;
                case 2:
                    newPosition = new Vector3(Mathf.Sqrt(3) * hexOffset, hexOffset, 0.0f);
                    tileType = TileType.Rain;
                    rootDirections = new bool[3] { true, false, false };
                    break;
            }

            startingTiles[i].UpdateTile(tileType, newPosition, tileDirections, false, false, false);
            startingTiles[i].directionsDown = rootDirections;
        }

        PlayerManager.Instance.activeTiles.Add(startingTiles[0].gameObject);
        PlayerManager.Instance.treeTiles.Add(startingTiles[1].gameObject);
        PlayerManager.Instance.activeTiles.Add(startingTiles[2].gameObject);

        for (int i = 0; i < 3; i++)
        {
            GameObject newRoot = Instantiate(root);
            newRoot.transform.SetParent(transform);
            Destroy(newRoot.transform.GetChild(0).gameObject);
            if (i == 0)
            {
                tileDirections = new bool[3] { true, false, true };
            }
            else
            {
                newRoot.tag = "InactiveBranch";
                tileDirections = new bool[3] { false, false, false };
            }
            Vector3 tilePosition = new Vector3(0.0f, -2 * Mathf.Sqrt(3) * i, 0.0f);
            PlayerManager.Instance.treeTiles.Add(newRoot);

            newRoot.GetComponent<TreeTile>().UpdateTile(0, tilePosition, tileDirections, true, false, false);

            GameObject trunk = Instantiate(trunkParts[i], newRoot.transform.position, Quaternion.Euler(0.0f, 0.0f, -30.0f)) as GameObject;
            trunk.transform.SetParent(newRoot.transform);
            newRoot.GetComponent<MeshRenderer>().enabled = false;
        }

        GameObject newGround = Instantiate(ground) as GameObject;
        newGround.transform.SetParent(transform.parent);

        GetComponentInParent<TreeManager>().ChangeSeason();
    }
}
