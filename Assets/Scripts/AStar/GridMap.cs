using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap
{
    Node[] _nodes;

    int _width;
    int _height;
    Vector2Int _origin;

    Tilemap _tilemap;

    public const int Error_Value_NonExist_Position = -1;

    public GridMap(Tilemap tilemap)
    {
        _tilemap = tilemap;
        _width = _tilemap.size.x;
        _height = _tilemap.size.y;
        _origin = (Vector2Int)_tilemap.origin;

        _nodes = new Node[_width * _height];

        Vector2Int min = new Vector2Int(_tilemap.cellBounds.xMin, _tilemap.cellBounds.yMin);
        Vector2Int max = new Vector2Int(_tilemap.cellBounds.xMax, _tilemap.cellBounds.yMax);

        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                int index = GridToIndex(x, y);
                _nodes[index] = new Node(x, y);
            }
        }
    }

    private int GridToIndex(int x, int y)
    {
        int index = Error_Value_NonExist_Position;
        if (IsVaildPosition(x, y))
        {
            index = x - _origin.x + ((_height - 1) - (y - _origin.y)) * _width;
        }
        return index;
    }

    public bool IsVaildPosition(int x, int y)
    {
        return x >= _origin.x && x < _origin.x + _width && y >= _origin.y && y < _origin.y + _height;
    }

    public Node GetNode(int x, int y)
    {
        Node result = null;
        int index = GridToIndex(x, y);
        if (index != Error_Value_NonExist_Position)
        {
            result = _nodes[index];
        }
        return result;
    }

    public Node GetNode(Vector2Int gridPos)
    {
        return GetNode(gridPos.x, gridPos.y);
    }

    public Node GetNode(Vector3 worldPos)
    {
        return GetNode(WorldToGrid(worldPos));
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return (Vector2Int)_tilemap.WorldToCell(worldPos);
    }

    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        return _tilemap.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f, 0f);
    }

    public void ClearAStarData()
    {
        foreach (var node in _nodes)
        {
            node.ClearAStarData();
        }
    }

    public bool IsWall(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.gridType == Node.GridType.Wall;
    }

    public bool IsMonster(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.gridType == Node.GridType.Monster;
    }

    public bool IsPlain(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.gridType == Node.GridType.Plain;
    }
}
