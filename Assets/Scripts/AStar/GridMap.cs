using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridMap
{
    Node[] _nodes;
    List<Node> _plainNodes;
    public List<Node> PlainNodes => _plainNodes;
    List<Node> _notPlainNodes;

    List<Node> _possibleNorthDoorNodes;
    public List<Node> PossibleNorthDoorNodes => _possibleNorthDoorNodes;
    List<Node> _impossibleNorthDoorNodes;
    List<Node> _possibleEastDoorNodes;
    public List<Node> PossibleEastDoorNodes => _possibleEastDoorNodes;
    List<Node> _impossibleEastDoorNodes;
    List<Node> _possibleSouthDoorNodes;
    public List<Node> PossibleSouthDoorNodes => _possibleSouthDoorNodes;
    List<Node> _impossibleSouthDoorNodes;
    List<Node> _possibleWestDoorNodes;
    public List<Node> PossibleWestDoorNodes => _possibleWestDoorNodes;
    List<Node> _impossibleWestDoorNodes;
    int _width;
    int _height;
    Vector2Int _origin;

    Tilemap _tilemap;

    public const int Error_Value_NonExist_Position = -1;
    const int Side_Impossible_DoorRange = 2;
    const int Side_Impossible_DoorRange_Half = Side_Impossible_DoorRange / 2;

    public GridMap(Tilemap tilemap)
    {
        _tilemap = tilemap;
        _width = _tilemap.size.x;
        _height = _tilemap.size.y;
        _origin = (Vector2Int)_tilemap.origin;

        _nodes = new Node[_width * _height];
        _plainNodes = new List<Node>(_nodes.Length);
        _notPlainNodes = new List<Node>(_nodes.Length);
        int north_and_south_possibleDoorRange = Mathf.Max(_width - Side_Impossible_DoorRange, 0);
        _possibleNorthDoorNodes = new List<Node>(north_and_south_possibleDoorRange);
        _possibleSouthDoorNodes = new List<Node>(north_and_south_possibleDoorRange);
        _impossibleNorthDoorNodes = new List<Node>(north_and_south_possibleDoorRange);
        _impossibleSouthDoorNodes = new List<Node>(north_and_south_possibleDoorRange);
        int east_and_west_possibleDoorRange = Mathf.Max(_height - Side_Impossible_DoorRange, 0);
        _possibleEastDoorNodes = new List<Node>(east_and_west_possibleDoorRange);
        _possibleWestDoorNodes = new List<Node>(east_and_west_possibleDoorRange);
        _impossibleEastDoorNodes = new List<Node>(east_and_west_possibleDoorRange);
        _impossibleWestDoorNodes = new List<Node>(east_and_west_possibleDoorRange);

        Vector2Int min = new Vector2Int(_tilemap.cellBounds.xMin, _tilemap.cellBounds.yMin);
        Vector2Int max = new Vector2Int(_tilemap.cellBounds.xMax, _tilemap.cellBounds.yMax);

        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                int index = GridToIndex(x, y);
                _nodes[index] = new Node(x, y);
                _plainNodes.Add(_nodes[index]);
                if (x >= min.x + Side_Impossible_DoorRange_Half && x < max.x - Side_Impossible_DoorRange_Half)
                {
                    if (y == min.y)
                    {
                        _possibleSouthDoorNodes.Add(_nodes[index]);
                    }
                    else if (y == max.y - 1)
                    {
                        _possibleNorthDoorNodes.Add(_nodes[index]);
                    }
                }
                else if (y >= min.y + Side_Impossible_DoorRange_Half && y < max.y - Side_Impossible_DoorRange_Half)
                {
                    if (x == min.x)
                    {
                        _possibleWestDoorNodes.Add(_nodes[index]);
                    }
                    else if (x == max.x - 1)
                    {
                        _possibleEastDoorNodes.Add(_nodes[index]);
                    }
                }
                
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

    public Vector2 GridToWorld(int x, int y)
    {
        return GridToWorld(new Vector2Int (x, y));
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
    public void PlainNodes_Restore()
    {
        int count = _notPlainNodes.Count;
        for (int i = 0; i < count; i++) 
        {
            Node node = _notPlainNodes[0];
            node.gridType = Node.GridType.Plain;
            _plainNodes.Add(node);
            _notPlainNodes.RemoveAt(0);
        }

        count = _impossibleNorthDoorNodes.Count;
        for (int i = 0; i < count; i++)
        {
            Node node = _impossibleNorthDoorNodes[0];
            _possibleNorthDoorNodes.Add(node);
            _impossibleNorthDoorNodes.RemoveAt(0);
        }

        count = _impossibleEastDoorNodes.Count;
        for (int i = 0; i < count; i++)
        {
            Node node = _impossibleEastDoorNodes[0];
            _possibleEastDoorNodes.Add(node);
            _impossibleEastDoorNodes.RemoveAt(0);
        }

        count = _impossibleSouthDoorNodes.Count;
        for (int i = 0; i < count; i++)
        {
            Node node = _impossibleSouthDoorNodes[0];
            _possibleSouthDoorNodes.Add(node);
            _impossibleSouthDoorNodes.RemoveAt(0);
        }

        count = _impossibleWestDoorNodes.Count;
        for (int i = 0; i < count; i++)
        {
            Node node = _impossibleWestDoorNodes[0];
            _possibleWestDoorNodes.Add(node);
            _impossibleWestDoorNodes.RemoveAt(0);
        }
    }

    public void PlainNodes_Remove(Node node)
    { 
        _plainNodes.Remove(node);
        _notPlainNodes.Add(node);
    }

    public void PossibleNorthDoorNodes_Remove(Node node)
    {
        if (_possibleNorthDoorNodes.Contains(node))
        {
            _possibleNorthDoorNodes.Remove(node);
            _impossibleNorthDoorNodes.Add(node);
        }
    }

    public void PossibleEastDoorNodes_Remove(Node node)
    {
        if (_possibleEastDoorNodes.Contains(node))
        {
            _possibleEastDoorNodes.Remove(node);
            _impossibleEastDoorNodes.Add(node);
        }
    }

    public void PossibleSouthDoorNodes_Remove(Node node)
    {
        if (_possibleSouthDoorNodes.Contains(node))
        {
            _possibleSouthDoorNodes.Remove(node);
            _impossibleSouthDoorNodes.Add(node);
        }
    }

    public void PossibleWestDoorNodes_Remove(Node node)
    {
        if (_possibleWestDoorNodes.Contains(node))
        {
            _possibleWestDoorNodes.Remove(node);
            _impossibleWestDoorNodes.Add(node);
        }
    }
}
