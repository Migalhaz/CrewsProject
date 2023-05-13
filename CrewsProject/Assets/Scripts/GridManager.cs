using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] Tile tile;
    [ContextMenuItem("Gerar Grid", nameof(GridGenerator))]
    [SerializeField] GridSettings gridSettings;
    [ContextMenuItem("Centralizar Camera", nameof(UpdateCamera))]
    [SerializeField] Camera mainCamera;
    Vector2 gridCenter = new();
    [field:SerializeField] public List<Tile> CurrentGrid { get; private set; }

    private void Awake()
    {
        Instance = this;
        GridGenerator();
    }

    void GridGenerator()
    {
        Vector2 _cellPosition = new();
        CurrentGrid = new();
        Transform _newGrid = new GameObject("GridParent").transform;
        for (int x = 0; x < gridSettings.X; x++)
        {
            for (int y = 0; y < gridSettings.Y; y++)
            {
                _cellPosition.Set(x, y);
                Tile _currentCell = Instantiate(tile, _cellPosition, Quaternion.identity, _newGrid);
                _currentCell.gameObject.name = $"Tile {x} {y}";
                _currentCell.ColorSettings.SetColor((x + y) % 2 != 1);
                _currentCell.SetPosition(_cellPosition);
                CurrentGrid.Add(_currentCell);
            }
        }
        UpdateCamera();
    }

    void UpdateCamera()
    {
        mainCamera.transform.position = gridCenter.ToVector3(-10f);
    }

    private void OnValidate()
    {
        gridCenter = gridSettings.Grid.Center();
    }

    private void OnDrawGizmos()
    {
        if (gridSettings.DrawGridSettings.DrawGizmos == OnDrawGizmosSwitch.OnDrawGizmosSelected) return;
        gridSettings.DrawGridSettings.Draw(gridSettings.Grid);
    }

    private void OnDrawGizmosSelected()
    {
        if (gridSettings.DrawGridSettings.DrawGizmos == OnDrawGizmosSwitch.OnDrawGizmos) return;
        gridSettings.DrawGridSettings.Draw(gridSettings.Grid);
    }

    [System.Serializable]
    class GridSettings
    {

        [SerializeField, Min(1)] int x = 1;
        [SerializeField, Min(1)] int y = 1;
        public Vector2 Grid { get { return new(x, y); } }
        public int X => x;
        public int Y => y;

        [SerializeField] DrawGridSettings drawGridSettings;
        public DrawGridSettings DrawGridSettings => drawGridSettings;

        
    }
}

[System.Serializable]
public class DrawGridSettings
{
    [SerializeField] OnDrawGizmosSwitch drawGizmos = OnDrawGizmosSwitch.OnDrawGizmosSelected;
    [SerializeField] bool drawTiles = true;
    [SerializeField] bool drawEdges = true;
    [SerializeField] Color gridEdgeColor = Color.white;
    [SerializeField] Color gridTileColor = Color.white;
    public Color GridEdgeColor => gridEdgeColor;
    public Color GridTileColor => gridTileColor;
    public bool DrawTiles => drawTiles;
    public bool DrawEdges => drawEdges;
    public OnDrawGizmosSwitch DrawGizmos => drawGizmos;

    public void Draw(Vector2 _gridSize)
    {
        if (DrawTiles)
        {
            Gizmos.color = GridTileColor;
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Gizmos.DrawWireCube(new(x, y, 0f), Vector3.one);
                }
            }
        }

        if (DrawEdges)
        {
            Gizmos.color = GridEdgeColor;
            Gizmos.DrawWireCube(_gridSize.Center(), _gridSize);
        }
    }
}

[System.Serializable]
public enum OnDrawGizmosSwitch
{
    OnDrawGizmos, OnDrawGizmosSelected
}


public static class Vector2Extend
{
    public static Vector3 ToVector3(this Vector2 _vector2)
    {
        return _vector2;
    }

    public static Vector3 ToVector3(this Vector2 _vector2, float _zValue)
    {
        return new Vector3(_vector2.x, _vector2.y, _zValue);
    }

    public static Vector2 Center(this Vector2 _vector2)
    {
        return new Vector2((_vector2.x * 0.5f) - 0.5f, (_vector2.y * 0.5f) - 0.5f);
    }

}