using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {

    public int x = 0;
    public int y = 0;

    public bool isWalkable = true;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode lastNode;

    public PathNode(int x, int y) {
        this.x = x;
        this.y = y;
        this.isWalkable = true;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

}
