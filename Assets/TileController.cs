using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using IdleFarmer;
using Unity.UNetWeaver;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    #region Private
    private Grid _grid;
    private Tilemap _tm;
    private Dictionary<Vector3Int, ExtTileData> _map;
    
    private const int XTileCount= 20;
    private const int YTileCount = 20;
    
    private DayTimer _dayTimer;
    #endregion

    #region Public
    public Tile grass;
    public Tile grassSelected;
    public Tile grassHighlight;
    
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    #endregion
    
    private void ProcessDay(int day)
    {
        Debug.Log($"DAY {day}");
    }
    
    private void SetupTiles()
    {
        for (var x = 0; x < XTileCount; x++)
        {
            for (var y = 0; y < YTileCount; y++)
            {
                var p = new Vector3Int(x, y, 0);
                var td = new ExtTileData {Tile = grass};
                _map.Add(p, td);
            }
        }
    }

    private void SetupCursor()
    {
        var hsX = cursorTexture.width / 2;
        var hsY = cursorTexture.height / 2;
        var hotSpot = new Vector2(hsX, hsY);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    private void SetupDayTimer()
    {
        DayTimer.GetInstance().NotifyDaysOnElapse += ProcessDay;
        DayTimer.DayLengthInSeconds = 3;
        DayTimer.GetInstance().Start();
    }

    void Start()
    {
        _grid = gameObject.GetComponent<Grid>();
        _map = new Dictionary<Vector3Int, ExtTileData>();
        _tm = gameObject.GetComponentInChildren<Tilemap>();

        SetupTiles();
        SetupCursor();
        SetupDayTimer();
    }

    private static void LeftMouseClickOnTile(ExtTileData tileData)
    {
        tileData.IsSelected = !tileData.IsSelected;
        if (tileData.IsSelected)
        {
            tileData.State = TileState.GrassSelected;
        }
        else
        {
            tileData.UndoStateChange();
        }
    }

    private Tile UpdateTileTexture(ExtTileData tileData)
    {
        switch (tileData.State)
        {
            case TileState.Grass:
                return grass;
            case TileState.Highlighted:
                return grassHighlight;
            case TileState.Road:
                break;
            case TileState.HoedField:
                break;
            case TileState.Planted:
                break;
            case TileState.GrassSelected:
                return grassSelected;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return grass;
    }

    void Update()
    {
        var mousePos = GetMousePosition();
        foreach (var t in _map)
        {
            mousePos.z = 0;
            if (t.Key == mousePos)
            {
                t.Value.Tile = grassHighlight;
                if (Input.GetMouseButtonDown(0))
                {
                    LeftMouseClickOnTile(t.Value);
                }
            }
            else
            {
                t.Value.Tile =  UpdateTileTexture(t.Value);
            }

            _tm.SetTile(t.Key, t.Value.Tile);
        }
    }
    private Vector3Int GetMousePosition()
    {
        if (Camera.main is null) return new Vector3Int(0, 0, 0);

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        var currentCell = _grid.WorldToCell(pos);
        return currentCell;

    }
}