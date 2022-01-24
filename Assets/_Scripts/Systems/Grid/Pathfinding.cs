using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private const int MOVE_STRAIGHT = 10;
    private const int MOVE_DIAGONAL = 14;

    private PathNode[,] _grid;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(int width, int height) {
        this._grid = new PathNode[width, height];

        for (int i = 0; i < _grid.GetLength(0); i++) {
            for (int j = 0; j < _grid.GetLength(1); j++) {
                _grid[i, j] = new PathNode(i, j);
            }
        }
    }

    public void UpdateGrid<T>(int x, int y, T t) {

        if (t == null) {

            _grid[x, y].isWalkable = true;

        } else {

            _grid[x, y].isWalkable = false;

        }

    }

    #region Public Methods

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {


        PathNode startNode = _grid[startX, startY];
        PathNode endNode = _grid[endX, endY];


        openList = new List<PathNode> { startNode };

        closedList = new List<PathNode>();

        for (int i = 0; i < _grid.GetLength(0); i++) {
            for (int j = 0; j < _grid.GetLength(1); j++) {
                PathNode pathNode = _grid[i, j];

                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.lastNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0) {

            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode.Equals(endNode)) {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbour in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbour)) continue;

                if (!neighbour.isWalkable) {
                    closedList.Add(neighbour);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);

                if (tentativeGCost < neighbour.gCost) {
                    neighbour.lastNode = currentNode;
                    neighbour.gCost = tentativeGCost;
                    neighbour.hCost = CalculateDistanceCost(neighbour, endNode);

                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }

            }
        }

        return null;
    }

    public Vector2Int GetClosestNode(int x, int y) {

        GridObject node = GridManager.Instance.grid.GetValue(x, y);

        if (node == null) return new Vector2Int(x, y);

        int rightCounter = 0;
        int upCounter = 0;
        int leftCounter = 0;
        int downCounter = 0;
        int height = node.Height;
        int width = node.Width;


        y--;
        x--;

        while (true) {

            bool changed = true;

            if (rightCounter < width + 1) {
                x++;
                rightCounter++;
            } else if (upCounter < height + 1) {
                y++;
                upCounter++;
            } else if (leftCounter < width + 1) {
                x--;
                leftCounter++;
            } else if (downCounter < height + 2) {
                y--;
                downCounter++;
            } else {
                height += 2;
                width += 2;

                changed = false;

                x--;

                rightCounter = 0;
                upCounter = 0;
                downCounter = 0;
                leftCounter = 0;
            }
            if (changed) {
                node = GridManager.Instance.grid.GetValue(x, y);

                if (node == null) {
                    break;
                }
            }

        }

        return new Vector2Int(x, y);
    }

    #endregion

    #region Private Methods

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        var grid = GridManager.Instance.grid;

        if (grid.WithinBounds(currentNode.x + 1, currentNode.y)) {// Right
            neighbourList.Add(_grid[currentNode.x + 1, currentNode.y]);

            if (grid.WithinBounds(currentNode.x + 1, currentNode.y - 1)) {  // Right Down
                neighbourList.Add(_grid[currentNode.x + 1, currentNode.y - 1]);
            }

            if (grid.WithinBounds(currentNode.x + 1, currentNode.y + 1)) { // Right Up
                neighbourList.Add(_grid[currentNode.x + 1, currentNode.y + 1]);
            }

        }

        if (grid.WithinBounds(currentNode.x - 1, currentNode.y)) { // Left
            neighbourList.Add(_grid[currentNode.x -1, currentNode.y]);

            if (grid.WithinBounds(currentNode.x - 1, currentNode.y - 1)) { // Left Down
                neighbourList.Add(_grid[currentNode.x - 1, currentNode.y - 1]);
            }

            if (grid.WithinBounds(currentNode.x - 1, currentNode.y + 1)) { // Left Up
                neighbourList.Add(_grid[currentNode.x - 1, currentNode.y + 1]);
            }
        }

        if (grid.WithinBounds(currentNode.x, currentNode.y - 1)) { // Down
            neighbourList.Add(_grid[currentNode.x, currentNode.y - 1]);
        }

        if (grid.WithinBounds(currentNode.x, currentNode.y + 1)) { // Up
            neighbourList.Add(_grid[currentNode.x, currentNode.y + 1]);
        }

        return neighbourList;
    }


    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();

        path.Add(endNode);

        PathNode currentNode = endNode;

        while (currentNode.lastNode != null) {
            path.Add(currentNode.lastNode);
            currentNode = currentNode.lastNode;
        }

        path.Reverse();

        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        return MOVE_DIAGONAL * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT * Mathf.Abs(xDistance - yDistance);
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodes) {
        PathNode lowestFcost = pathNodes[0];

        for (int i = 1; i < pathNodes.Count; i++) {
            if (pathNodes[i].fCost < lowestFcost.fCost) {
                lowestFcost = pathNodes[i];
            }
        }

        return lowestFcost;
    }

    #endregion
}
