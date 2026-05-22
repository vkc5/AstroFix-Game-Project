using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircuitPuzzleManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject puzzleWindow;
    public RectTransform gridParent;
    public GridLayoutGroup gridLayout;
    public CircuitTile tilePrefab;

    [Header("Level")]
    public OxygenPuzzleLevel levelController;

    private int size;
    private CircuitTile[,] tiles;
    private Vector2Int startPos;
    private List<Vector2Int> bulbPositions = new List<Vector2Int>();

    private bool isGeneratingPuzzle = false;
    private bool puzzleSolved = false;

    public void OpenPuzzle(int puzzleSize, int puzzleNumber)
    {
        if (puzzleSize != 5 && puzzleSize != 7)
        {
            Debug.LogError("Puzzle size must be 5 or 7. Current size: " + puzzleSize);
            return;
        }

        size = puzzleSize;
        puzzleSolved = false;
        isGeneratingPuzzle = true;

        puzzleWindow.SetActive(true);
        ClearGrid();

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = size;

        tiles = new CircuitTile[size, size];
        bulbPositions.Clear();

        CreateEmptyGrid();
        GenerateDesignedPuzzle(puzzleNumber);
        UpdateTileTypes();
        FillUnusedTiles();
        SetStartAndBulbs();
        RandomizeRotations();

        isGeneratingPuzzle = false;
        CheckPower();
    }

    void ClearGrid()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);
    }

    void CreateEmptyGrid()
    {
        startPos = new Vector2Int(size / 2, size / 2);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                CircuitTile tile = Instantiate(tilePrefab, gridParent);
                tile.Setup(this);
                tile.SetTile(TileType.Empty, false, false, false, false);
                tiles[x, y] = tile;
            }
        }
    }

    void GenerateDesignedPuzzle(int puzzleNumber)
    {
        if (puzzleNumber == 1)
        {
            bulbPositions.Add(new Vector2Int(1, 0));
            bulbPositions.Add(new Vector2Int(5, 0));
            bulbPositions.Add(new Vector2Int(4, 1));
            bulbPositions.Add(new Vector2Int(0, 4));
            bulbPositions.Add(new Vector2Int(6, 5));
            bulbPositions.Add(new Vector2Int(2, 6));

            AddPath(new Vector2Int(3, 3), new Vector2Int(2, 3), new Vector2Int(1, 3), new Vector2Int(1, 2), new Vector2Int(1, 1), new Vector2Int(1, 0));
            AddPath(new Vector2Int(1, 3), new Vector2Int(1, 4), new Vector2Int(0, 4));

            AddPath(new Vector2Int(3, 3), new Vector2Int(4, 3), new Vector2Int(4, 2), new Vector2Int(4, 1));

            AddPath(new Vector2Int(4, 3), new Vector2Int(5, 3), new Vector2Int(5, 2), new Vector2Int(5, 1), new Vector2Int(5, 0));
            AddPath(new Vector2Int(5, 3), new Vector2Int(5, 4), new Vector2Int(6, 4), new Vector2Int(6, 5));

            AddPath(new Vector2Int(3, 3), new Vector2Int(3, 4), new Vector2Int(3, 5), new Vector2Int(2, 5), new Vector2Int(2, 6));
        }
        else
        {
            bulbPositions.Add(new Vector2Int(0, 0));
            bulbPositions.Add(new Vector2Int(6, 0));
            bulbPositions.Add(new Vector2Int(0, 6));
            bulbPositions.Add(new Vector2Int(6, 6));
            bulbPositions.Add(new Vector2Int(3, 0));
            bulbPositions.Add(new Vector2Int(3, 6));

            AddPath(new Vector2Int(3, 3), new Vector2Int(3, 2), new Vector2Int(3, 1), new Vector2Int(3, 0));

            AddPath(new Vector2Int(3, 3), new Vector2Int(2, 3), new Vector2Int(1, 3), new Vector2Int(0, 3), new Vector2Int(0, 2), new Vector2Int(0, 1), new Vector2Int(0, 0));

            AddPath(new Vector2Int(3, 3), new Vector2Int(4, 3), new Vector2Int(5, 3), new Vector2Int(6, 3), new Vector2Int(6, 2), new Vector2Int(6, 1), new Vector2Int(6, 0));

            AddPath(new Vector2Int(3, 3), new Vector2Int(2, 3), new Vector2Int(2, 4), new Vector2Int(1, 4), new Vector2Int(0, 4), new Vector2Int(0, 5), new Vector2Int(0, 6));

            AddPath(new Vector2Int(3, 3), new Vector2Int(4, 3), new Vector2Int(4, 4), new Vector2Int(5, 4), new Vector2Int(6, 4), new Vector2Int(6, 5), new Vector2Int(6, 6));

            AddPath(new Vector2Int(3, 3), new Vector2Int(3, 4), new Vector2Int(3, 5), new Vector2Int(3, 6));
        }
    }

    void AddPath(params Vector2Int[] points)
    {
        for (int i = 0; i < points.Length - 1; i++)
            ConnectTiles(points[i], points[i + 1]);
    }

    void ConnectTiles(Vector2Int a, Vector2Int b)
    {
        if (!IsInside(a) || !IsInside(b))
            return;

        Vector2Int dir = b - a;

        // UI GRID NOTE:
        // y + 1 means visually DOWN
        // y - 1 means visually UP

        if (dir == new Vector2Int(0, -1)) // visual UP
        {
            tiles[a.x, a.y].up = true;
            tiles[b.x, b.y].down = true;
        }
        else if (dir == new Vector2Int(0, 1)) // visual DOWN
        {
            tiles[a.x, a.y].down = true;
            tiles[b.x, b.y].up = true;
        }
        else if (dir == Vector2Int.right)
        {
            tiles[a.x, a.y].right = true;
            tiles[b.x, b.y].left = true;
        }
        else if (dir == Vector2Int.left)
        {
            tiles[a.x, a.y].left = true;
            tiles[b.x, b.y].right = true;
        }
    }

    void UpdateTileTypes()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                CircuitTile tile = tiles[x, y];

                int count = CountConnections(tile);

                if (count == 0)
                {
                    tile.SetTile(TileType.Empty, false, false, false, false);
                }
                else if (count == 2 && IsStraight(tile))
                {
                    tile.SetTile(TileType.Straight, tile.up, tile.right, tile.down, tile.left);
                }
                else if (count == 2)
                {
                    tile.SetTile(TileType.Corner, tile.up, tile.right, tile.down, tile.left);
                }
                else
                {
                    tile.SetTile(TileType.TShape, tile.up, tile.right, tile.down, tile.left);
                }
            }
        }
    }

    void FillUnusedTiles()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                CircuitTile tile = tiles[x, y];

                if (CountConnections(tile) != 0)
                    continue;

                int randomType = Random.Range(0, 3);

                if (randomType == 0)
                    tile.SetTile(TileType.Straight, false, true, false, true);
                else if (randomType == 1)
                    tile.SetTile(TileType.Corner, true, true, false, false);
                else
                    tile.SetTile(TileType.TShape, false, true, true, true);
            }
        }
    }

    void SetStartAndBulbs()
    {
        CircuitTile startTile = tiles[startPos.x, startPos.y];

        startTile.SetTile(
            TileType.Start,
            startTile.up,
            startTile.right,
            startTile.down,
            startTile.left
        );

        foreach (Vector2Int bulbPos in bulbPositions)
        {
            CircuitTile bulbTile = tiles[bulbPos.x, bulbPos.y];

            bulbTile.SetTile(
                TileType.Bulb,
                true,
                true,
                true,
                true
            );
        }
    }

    void RandomizeRotations()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                CircuitTile tile = tiles[x, y];

                if (tile.tileType == TileType.Start ||
                    tile.tileType == TileType.Bulb)
                    continue;

                int rotateCount = Random.Range(1, 4);

                for (int i = 0; i < rotateCount; i++)
                    tile.RotateTile();
            }
        }
    }

    public void CheckPower()
    {
        if (isGeneratingPuzzle) return;
        if (puzzleSolved) return;
        if (tiles == null) return;

        foreach (CircuitTile tile in tiles)
            tile.SetPowered(false);

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(startPos);
        visited.Add(startPos);

        tiles[startPos.x, startPos.y].SetPowered(true);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            CircuitTile currentTile = tiles[current.x, current.y];

            TrySpread(current, new Vector2Int(0, -1), currentTile.up, "down", queue, visited);
            TrySpread(current, Vector2Int.right, currentTile.right, "left", queue, visited);
            TrySpread(current, new Vector2Int(0, 1), currentTile.down, "up", queue, visited);
            TrySpread(current, Vector2Int.left, currentTile.left, "right", queue, visited);
        }

        bool allBulbsPowered = true;

        foreach (Vector2Int bulbPos in bulbPositions)
        {
            CircuitTile bulbTile = tiles[bulbPos.x, bulbPos.y];

            if (!bulbTile.isPowered)
                allBulbsPowered = false;
        }

        if (allBulbsPowered)
        {
            puzzleSolved = true;
            puzzleWindow.SetActive(false);

            if (levelController != null)
                levelController.PuzzleSolved();
        }
    }

    void TrySpread(
        Vector2Int current,
        Vector2Int direction,
        bool canExit,
        string neededInput,
        Queue<Vector2Int> queue,
        HashSet<Vector2Int> visited)
    {
        if (!canExit)
            return;

        Vector2Int next = current + direction;

        if (!IsInside(next))
            return;

        if (visited.Contains(next))
            return;

        CircuitTile nextTile = tiles[next.x, next.y];

        bool canEnter = false;

        if (nextTile.tileType == TileType.Bulb)
        {
            canEnter = true;
        }
        else
        {
            if (neededInput == "up") canEnter = nextTile.up;
            if (neededInput == "right") canEnter = nextTile.right;
            if (neededInput == "down") canEnter = nextTile.down;
            if (neededInput == "left") canEnter = nextTile.left;
        }

        if (!canEnter)
            return;

        visited.Add(next);
        nextTile.SetPowered(true);
        queue.Enqueue(next);
    }

    bool IsInside(Vector2Int pos)
    {
        return pos.x >= 0 &&
               pos.x < size &&
               pos.y >= 0 &&
               pos.y < size;
    }

    int CountConnections(CircuitTile tile)
    {
        int count = 0;

        if (tile.up) count++;
        if (tile.right) count++;
        if (tile.down) count++;
        if (tile.left) count++;

        return count;
    }

    bool IsStraight(CircuitTile tile)
    {
        return (tile.up && tile.down) ||
               (tile.left && tile.right);
    }

    public void ClosePuzzlePanel()
    {
        puzzleWindow.SetActive(false);

        if (levelController != null)
            levelController.PuzzleClosedWithoutSolve();
    }
}