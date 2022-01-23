using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private const int MOVE_STRAIGHT = 10;
    private const int MOVE_DIAGONAL = 10;

    private Grid<GridObject> _grid;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(Grid<GridObject> grid) {
        this._grid = grid;
    }

    #region Public Methods

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {

        PathNode startNode = _grid.GetValue(startX, startY);
        PathNode endNode = _grid.GetValue(endX, endY);

        openList = new List<PathNode> { startNode };

        closedList = new List<PathNode>();

        for(int i = 0; i < _grid.GetWidth(); i++) {
            for (int j = 0; j < _grid.GetHeight(); j++) {
                PathNode pathNode = _grid.GetValue(i, j);

                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.lastNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculatteDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode.Equals(endNode)) {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbour in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbour)) continue;

                int tGCost = currentNode.gCost + CalculatteDistanceCost(currentNode, neighbour);

                if (tGCost < currentNode.gCost) {

                    neighbour.lastNode = currentNode;
                    neighbour.gCost = tGCost;
                    neighbour.hCost = CalculatteDistanceCost(neighbour, endNode);

                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }

            }
        }

        return null;
    }

    public Vector2 GetClosestNode(int x, int y) {
        PathNode node = GridManager.Instance.grid.GetValue(x, y);

        if (node == null) return _grid.GetWorldPosition(x, y);

        GridObject gridObj = (GridObject)node;

        int forwardCounter = 0;
        int upCounter = 0;
        int backwardCounter = 0;
        int downCounter = 0;
        int height = gridObj.Height;
        int weight = gridObj.Width;

        while (node == null || !node.isWalkable) {
            if (upCounter < height) {
                y += 1;
                upCounter++;
            } else if (forwardCounter < weight + 1) {
                x += 1;
                forwardCounter++;
            } else if (downCounter < height + 1) {
                y += 1;
                downCounter++;
            } else if (backwardCounter < weight + 2) {
                x += 1;
                backwardCounter++;
            } else {
                height += 2;
                weight += 2;
                forwardCounter = 0;
                upCounter = 0;
                downCounter = 0;
                backwardCounter = 0;
            }
            node = _grid.GetValue(x, y);
        }

        return new Vector2(x, y);
    }

#endregion

    #region Private Methods

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (_grid.WithinBounds(currentNode.x+1, currentNode.y)) {// Right
            neighbourList.Add((PathNode)_grid.GetValue(currentNode.x + 1, currentNode.y));
            if (_grid.WithinBounds(currentNode.x + 1, currentNode.y - 1)) // Right Down
                neighbourList.Add((PathNode)_grid.GetValue(currentNode.x + 1, currentNode.y - 1));
            if (_grid.WithinBounds(currentNode.x + 1, currentNode.y + 1)) // Right Up
                neighbourList.Add((PathNode)_grid.GetValue(currentNode.x + 1, currentNode.y + 1));
        }

        if (_grid.WithinBounds(currentNode.x - 1, currentNode.y)) { // Left
            neighbourList.Add((PathNode)_grid.GetValue(currentNode.x - 1, currentNode.y));

            if (_grid.WithinBounds(currentNode.x - 1, currentNode.y - 1)) // Left Down
                neighbourList.Add((PathNode)_grid.GetValue(currentNode.x - 1, currentNode.y - 1));

            if (_grid.WithinBounds(currentNode.x - 1, currentNode.y + 1)) // Left Up
                neighbourList.Add((PathNode)_grid.GetValue(currentNode.x - 1, currentNode.y + 1));
        }

        if (_grid.WithinBounds(currentNode.x, currentNode.y-1)) // Down
            neighbourList.Add((PathNode)_grid.GetValue(currentNode.x, currentNode.y-1));

        if (_grid.WithinBounds(currentNode.x, currentNode.y+1)) // Up
            neighbourList.Add((PathNode)_grid.GetValue(currentNode.x, currentNode.y+1));

        return neighbourList;
    }


    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();

        path.Add(endNode);

        PathNode currentNode = endNode;

        while(currentNode.lastNode != null) {
            path.Add(currentNode.lastNode);
            currentNode = currentNode.lastNode;
        }

        path.Reverse();

        return path;
    }

    private int CalculatteDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        return MOVE_DIAGONAL * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT * Mathf.Abs(xDistance - yDistance);
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes) {
        PathNode lowestFcost = pathNodes[0];

        for(int i = 1; i < pathNodes.Count; i++) {
            if(pathNodes[i].fCost < lowestFcost.fCost) {
                lowestFcost = pathNodes[i];
            }
        }

        return lowestFcost;
    }

    #endregion
}
