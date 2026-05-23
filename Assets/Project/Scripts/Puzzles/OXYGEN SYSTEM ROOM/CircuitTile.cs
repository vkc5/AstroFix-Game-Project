using UnityEngine;
using UnityEngine.UI;

public enum TileType
{
    Empty,
    Straight,
    Corner,
    TShape,
    Start,
    Bulb
}

public class CircuitTile : MonoBehaviour
{
    [Header("UI Parts")]
    public Image background;
    public Image upBar;
    public Image rightBar;
    public Image downBar;
    public Image leftBar;
    public Image centerDot;

    [Header("Sprites")]
    public Sprite startGreenBulb;
    public Sprite bulb;
    public Sprite yellowPoweredBulb;

    [Header("Connections")]
    public bool up;
    public bool right;
    public bool down;
    public bool left;

    public bool isPowered;
    public TileType tileType;

    private CircuitPuzzleManager manager;

    public void Setup(CircuitPuzzleManager puzzleManager)
    {
        manager = puzzleManager;
    }

    public void SetTile(TileType type, bool u, bool r, bool d, bool l)
    {
        tileType = type;

        up = u;
        right = r;
        down = d;
        left = l;

        DrawTile();
    }

    void DrawTile()
    {
        bool isPipe = tileType == TileType.Straight ||
                      tileType == TileType.Corner ||
                      tileType == TileType.TShape;

        upBar.gameObject.SetActive(isPipe && up);
        rightBar.gameObject.SetActive(isPipe && right);
        downBar.gameObject.SetActive(isPipe && down);
        leftBar.gameObject.SetActive(isPipe && left);

        centerDot.gameObject.SetActive(tileType == TileType.Start || tileType == TileType.Bulb);

        if (tileType == TileType.Start)
            centerDot.sprite = startGreenBulb;

        if (tileType == TileType.Bulb)
            centerDot.sprite = isPowered ? yellowPoweredBulb : bulb;
    }

    public void RotateTile()
    {
        if (tileType == TileType.Start || tileType == TileType.Bulb)
            return;

        bool oldUp = up;
        bool oldRight = right;
        bool oldDown = down;
        bool oldLeft = left;

        up = oldLeft;
        right = oldUp;
        down = oldRight;
        left = oldDown;

        DrawTile();

        if (manager != null)
            manager.CheckPower();
    }

    public void SetPowered(bool powered)
    {
        isPowered = powered;

        Color pipeColor = powered ? Color.yellow : Color.white;

        upBar.color = pipeColor;
        rightBar.color = pipeColor;
        downBar.color = pipeColor;
        leftBar.color = pipeColor;

        if (tileType == TileType.Bulb)
            centerDot.sprite = powered ? yellowPoweredBulb : bulb;
    }
}