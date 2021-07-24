using UnityEngine;
using System.Collections;

public class TreeTile : HexTile
{
    public TileType type;
    public Material[] materials;
    public bool[] directionsUp = new bool[3];
    public bool[] directionsDown = new bool[3] { false, false, false };
    public Vector3[] tangentsDown = new Vector3[3];
    public Sprite sprite1;
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;
    public Sprite sprite6;
    public Sprite sprite8;
    public Sprite sprite9;
    public string weather = "Inactive";
    public string season = "Spring";
    public MeshRenderer tileRenderer;
    public bool draggable;
    public GameObject branchGenerator;
    public string eventType = "None";
    public bool destroyTag = false;

    private GameObject treeManager;
    private Vector3[] tangentsUp = new Vector3[3];
    private Vector3 m_pos;
    private Vector3 screenPoint;
    private Vector3 home;
    private int activeMask;

    void Awake()
    {
        treeManager = GameObject.Find("TreeStructure");
        float size = PlayerManager.Instance.hexSize;
        tangentsUp = new Vector3[] { new Vector3(-1 * size * Mathf.Sqrt(3) / 4f, size * 0.25f, 0.0f), new Vector3(0.0f, size * 0.5f / Mathf.Sqrt(3), 0.0f), new Vector3(size * Mathf.Sqrt(3) / 4f, size * 0.25f, 0.0f) };
        tangentsDown = new Vector3[] { new Vector3(size * Mathf.Sqrt(3) / 4f, size * 0.25f, 0.0f), new Vector3(0.0f, size * 0.5f / Mathf.Sqrt(3), 0.0f), new Vector3(-1 * size * Mathf.Sqrt(3) / 4f, size * 0.25f, 0.0f) };
    }

    void OnMouseDown()
    {
        if (weather != "Inactive")
            PlayerManager.Instance.informationPanel.GetComponent<InformationPanelController>().InfoPanelOn(transform.position, season, weather);
        else if (draggable)
        {
            home = transform.position;
        }
    }

    void OnMouseDrag()
    {
        if (draggable)
        {
            Vector2 target = PlayerManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cursorPoint = new Vector3(target.x, target.y, transform.position.z);
            transform.position = cursorPoint;
        }
    }

    void OnMouseUp()
    {
        PlayerManager.Instance.informationPanel.SetActive(false);

        if (draggable)
        {
            Vector2 target = PlayerManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            activeMask = 1 << LayerMask.NameToLayer("ActiveTiles");
            Collider2D hit = Physics2D.OverlapPoint(target, activeMask);
            if (hit != null)
            {
                transform.position = hit.transform.position;
                PlaceTile(hit.gameObject);
                PlayerManager.Instance.CheckState();
                draggable = false;
            }
            else
                transform.position = home;
        }
    }

    // TODO: This code smells, especially when used to initialise new tiles. See if there's a better way to handle this. 
    public void UpdateTile(TileType newType, Vector3 newPos, bool[] newDirections = null, bool changeSprite = false, bool changeSeason = false, bool changeEvent = false)
    {
        ChangeMaterial(newType);

        ChangePosition(newPos);

        if (newDirections != null)
            ChangeDirections(newDirections);

        if (changeSprite)
            ChangeSprite();

        if (changeSeason)
            ChangeSeason();

        if (changeEvent)
            ChangeEvent();
    }

    public void ChangeMaterial(TileType newType)
    {
        type = newType;
        GetComponent<MeshRenderer>().material = materials[(int)newType];
        switch (newType)
        {
            case TileType.NewTile:
            case TileType.Leaves:
                weather = "Inactive";
                break;
            case TileType.Fair:
                weather = "Fair";
                break;
            case TileType.Rain:
                weather = "Rain";
                break;
            case TileType.Sunshine:
                weather = "Sunshine";
                break;
            case TileType.Frost:
                weather = "Frost";
                break;
        }
    }

    void ChangePosition(Vector3 newPos)
    {
        m_pos = newPos;
        transform.position = m_pos;
    }

    void ChangeDirections(bool[] newDirections)
    {
        directionsUp = newDirections;

    }

    void ChangeRoots(bool[] newRoots)
    {
        directionsDown = newRoots;
    }

    void ChangeSprite()
    {
        int spriteNumber = 0;

        if (directionsUp[0])
            spriteNumber += 1;
        if (directionsUp[1])
            spriteNumber += 3;
        if (directionsUp[2])
            spriteNumber += 5;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        switch (spriteNumber)
        {
            case 1:
                spriteRenderer.sprite = sprite1;
                break;
            case 3:
                spriteRenderer.sprite = sprite3;
                break;
            case 4:
                spriteRenderer.sprite = sprite4;
                break;
            case 5:
                spriteRenderer.sprite = sprite5;
                break;
            case 6:
                spriteRenderer.sprite = sprite6;
                break;
            case 8:
                spriteRenderer.sprite = sprite8;
                break;
            case 9:
                spriteRenderer.sprite = sprite9;
                break;
        }
    }

    void ChangeEvent()
    {
        eventType = GetComponentInChildren<EventSpriteManager>().SetEvent();
    }

    void ChangeSeason()
    {
        season = PlayerManager.Instance.nextSeason;
    }

    void PlaceTile(GameObject seasonTile)
    {
        // Get reference to the tile selection holder
        NewTileManager newTileScript = transform.parent.GetComponent<NewTileManager>();

        // Attach selected tile to the tree, change its material and set it as active for next season
        transform.SetParent(treeManager.transform);
        ChangeMaterial(0);
        PlayerManager.Instance.treeTiles.Add(gameObject);

        // Run any player effects from the season tile
        PlayerManager.Instance.ResolveTile(seasonTile, gameObject);

        // This tile needs to know relative branch directions now
        directionsDown = seasonTile.GetComponent<TreeTile>().directionsDown;

        // Destroy tiles that now cannot be accessed
        PruneTree();

        // Remove the sprite attached to the tree
        Destroy(transform.GetChild(0).gameObject);

        // Procedurally Generate Branches as required
        for (int i = 0; i < 3; i++)
        {
            if (directionsDown[i])
            {
                for (int j = 0; j < 3; j++)
                {
                    if (directionsUp[j])
                        MakeBranches(i, j);
                }
            }
        }

        // Destroy the season tile that was there
        PlayerManager.Instance.activeTiles.Remove(seasonTile);
        Destroy(seasonTile);

        // Place a new tile at the end of the tile manager
        newTileScript.UpdateSelection(home);

        PlayerManager.Instance.size++;


    }

    void PruneTree()
    {
        Vector3 prunePosition = transform.position;
        Vector3 adjustPosition = transform.position;
        float hexOffset = Mathf.Sqrt(3) * 0.5f * PlayerManager.Instance.hexSize;
        Vector3 hexUp = hexOffset * Vector3.up;
        Vector3 hexRight = Mathf.Sqrt(3) * hexOffset * Vector3.right;

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    prunePosition = transform.position - hexRight + hexUp;
                    adjustPosition = transform.position - hexRight - hexUp;
                    break;
                case 1:
                    prunePosition = transform.position + 2 * hexUp;
                    adjustPosition = transform.position - 2 * hexUp;
                    break;
                case 2:
                    prunePosition = transform.position + hexRight + hexUp;
                    adjustPosition = transform.position + hexRight - hexUp;
                    break;
            }

            Collider2D hit = Physics2D.OverlapPoint(prunePosition);
            if (hit)
            {
                if (destroyTag && hit.tag != "InactiveBranch")
                {
                    // For fires etc we don't want to leave branches behind
                    PlayerManager.Instance.seasonTiles.Remove(hit.gameObject);
                    PlayerManager.Instance.activeTiles.Remove(hit.gameObject);
                    Destroy(hit.gameObject);
                }
                else if (!directionsUp[i] && hit.tag != "InactiveBranch")
                {
                    // Next season tile is no longer connected - turn it in to a leaf tile
                    PlayerManager.Instance.seasonTiles.Remove(hit.gameObject);
                    PlayerManager.Instance.activeTiles.Remove(hit.gameObject);
                    PlayerManager.Instance.treeTiles.Add(hit.gameObject);

                    hit.gameObject.layer = LayerMask.NameToLayer("TreeTiles");
                    hit.GetComponent<TreeTile>().ChangeMaterial(TileType.Leaves);
                    int childCount = hit.transform.childCount;
                    for (int c = 0; c < childCount; c++)
                        Destroy(hit.transform.GetChild(c).gameObject);
                }
                else
                {
                    // Next season tile is connected to this one - give it the correct connection variables
                    hit.GetComponent<TreeTile>().directionsDown[2 - i] = true;
                    hit.GetComponent<TreeTile>().tangentsDown[2 - i] = tangentsUp[i];
                }
            }

            hit = Physics2D.OverlapPoint(adjustPosition);
            if (hit && directionsDown[i])
            {
                // Previous season tile is connected to this - build branch connections
                if (hit.transform.childCount > 0)
                {
                    BezierGenerator[] branches = hit.GetComponentsInChildren<BezierGenerator>();
                    foreach (BezierGenerator branch in branches)
                        branch.BuildMesh(false);
                }
            }
        }
    }

    void MakeBranches(int i, int j)
    {
        Vector3[] startBranch = new Vector3[]{new Vector3(-1.50f, -Mathf.Sqrt(3)/2f, 0.0f),
            new Vector3(0.0f, -Mathf.Sqrt(3), 0.0f),
            new Vector3(1.50f, -Mathf.Sqrt(3)/2f, 0.0f)};
        Vector3[] endBranch = new Vector3[]{new Vector3(-1.5f, Mathf.Sqrt(3)/2f, 0.0f),
            new Vector3(0.0f, Mathf.Sqrt(3), 0.0f),
            new Vector3(1.5f, Mathf.Sqrt(3)/2f, 0.0f)};

        GameObject branch = Instantiate(branchGenerator);

        branch.GetComponent<BezierGenerator>().GetReference(transform.position + startBranch[i], transform.position + endBranch[j], tangentsDown[i], tangentsUp[j]);
        branch.GetComponent<BezierGenerator>().BuildMesh(true);
        branch.transform.SetParent(transform);

        tileRenderer.enabled = false;

    }
}
