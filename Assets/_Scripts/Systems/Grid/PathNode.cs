using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathNode : MonoBehaviour {
    public int x { get; internal set; }
    public int y { get; internal set; }

    public bool isWalkable = true;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode lastNode;

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

}
