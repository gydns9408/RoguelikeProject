using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public  struct TileData
//{

//}

public class Node : IComparable<Node>
{
    public int x_coordinate;
    public int y_coordinate;

    public float G;
    public float H;
    public float F => G + H;

    public Node parentNode;

    public enum GridType
    {
        Plain = 0,
        Wall,
        Monster
    }

    public GridType gridType;

    //List<List<TileData>> map = new List<List<TileData>>();

    public Node(int x, int y, GridType type = GridType.Plain)
    {
        x_coordinate = x;
        y_coordinate = y;
        gridType = type;

        ClearAStarData();
    }

    public void ClearAStarData()
    {
        G = float.MaxValue;
        H = float.MaxValue;
        parentNode = null;
    }

    public int CompareTo(Node other)
    {
        if (other == null) return 1;

        return F.CompareTo(other.F);
    }

    public override bool Equals(object obj)
    {
        return obj is Node other && x_coordinate == other.x_coordinate && y_coordinate == other.y_coordinate;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x_coordinate, y_coordinate);
    }
}
