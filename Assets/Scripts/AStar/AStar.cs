using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    const float distanceToSideNode = 1.0f;
    const float distanceToSideDiagonalNode = 1.4f;

    public static bool IsPossiblePath(GridMap gridMap, Vector2Int startPos, Vector2Int endPos)
    {
        bool result = false;
        gridMap.ClearAStarData();

        if (gridMap.IsVaildPosition(startPos) && gridMap.IsVaildPosition(endPos))
        {
            List<Node> open = new List<Node>();
            List<Node> close = new List<Node>();

            Node current = gridMap.GetNode(startPos);
            current.G = 0f;
            current.H = GetEstimatedDistance(current, endPos);
            open.Add(current);

            while (open.Count > 0)
            {
                open.Sort();
                current = open[0];
                open.RemoveAt(0);
                if (current != endPos)
                {
                    close.Add(current);

                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = gridMap.GetNode(current.x_coordinate + x, current.y_coordinate + y);
                            if (node == null) continue;
                            if (node == current) continue;
                            if (node.gridType == Node.GridType.Wall) continue;
                            if (close.Contains(node)) continue;
                            bool isDiagonal = Mathf.Abs(x) == Mathf.Abs(y);
                            if (isDiagonal && (gridMap.GetNode(current.x_coordinate + x, current.y_coordinate).gridType == Node.GridType.Wall || gridMap.GetNode(current.x_coordinate, current.y_coordinate + y).gridType == Node.GridType.Wall))
                            {
                                continue;
                            }
                            float distance;
                            if (isDiagonal)
                            {
                                distance = distanceToSideDiagonalNode;
                            }
                            else
                            {
                                distance = distanceToSideNode;
                            }

                            if (node.G > current.G + distance)
                            {
                                if (node.parentNode == null)
                                {
                                    node.H = GetEstimatedDistance(node, endPos);
                                    open.Add(node);
                                }
                                node.G = current.G + distance;
                                node.parentNode = current;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }

            }

            if (current == endPos)
            {
                result = true;
            }

        }

        return result;
    }

    public static bool IsPossiblePath(GridMap gridMap, Node start, Node end)
    {
        return IsPossiblePath(gridMap, new Vector2Int(start.x_coordinate, start.y_coordinate), new Vector2Int(end.x_coordinate, end.y_coordinate));
    }


        static float GetEstimatedDistance(Node current, Vector2Int endPos)
    {
        return Mathf.Abs(current.x_coordinate - endPos.x) + Mathf.Abs(current.y_coordinate - endPos.y);
    }
}
