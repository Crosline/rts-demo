using _Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager> {

    public static Action<GridObject> OnGridObjectSelectEvent;

    public static Action<Vector3> OnDeployPositionSet;

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    [SerializeField]
    private float cellSize;

    public Pathfinding pathfinding;

    public Grid<GridObject> grid { get; private set; }

    [SerializeField]
    private Transform gridCells;

    [SerializeField]
    private GameObject gridPrefab;

    [SerializeField]
    private GameObject deployGhost;

    public float CellSize => cellSize;

    public void GridSetup() {
        grid = new Grid<GridObject>(width, height, new Vector3Int(-width, -height, 0), cellSize);

        pathfinding = new Pathfinding(width, height);

        grid.OnGridValueChanged += UpdatePathNodes;

        DrawGrid();
    }

    public void CleanGrid() {
        grid = null;
        gridCells.DestroyChildren();

        grid.OnGridValueChanged -= UpdatePathNodes;
    }

    private void UpdatePathNodes<T>(Grid<T>.GridEventArgs args) {
        pathfinding.UpdateGrid(args.x, args.y, args.value);
    }

    public void SetDeployLocation(Vector3 pos, bool remove = false) {
        DestroyDeployPointer();


        if (remove) return;


        deployGhost = Instantiate(gridPrefab);
        if (grid.GetXY(pos, out int x, out int y)) {

            var temp = grid.GetValue(x, y);
            if (temp == null) {
                var deployPos = grid.GetWorldPosition(x, y);
                deployGhost.transform.position = deployPos;

                OnDeployPositionSet?.Invoke(deployPos);
            } else {

                OnDeployPositionSet?.Invoke(grid.GetWorldPosition(x, y));
            }
        }
        if (deployGhost.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer)) {
            renderer.color = Color.blue;
            renderer.Fade(0.5f);
        }

        Destroy(deployGhost, 2f);

    }

    public bool GetGridObject(Vector3 pos) {
        if (grid.GetXY(pos, out int x, out int y)) {
            var gridObj = grid.GetValue(x, y);
            OnGridObjectSelectEvent?.Invoke(gridObj);
            if (gridObj == null) {
                DestroyDeployPointer();
                return false;
            } else {
                if (gridObj.CompareTag("Barracks")) {
                    Barracks barracks = (Barracks)gridObj;
                    var deployPos = barracks.GetDeployPos();
                    if (!deployPos.y.Equals(0.5f))
                        SetDeployLocation(barracks.GetDeployPos());
                }
                return true;
            }
        } else {
            DestroyDeployPointer();
            OnGridObjectSelectEvent?.Invoke(null);
            return false;
        }
    }

    public void DestroyDeployPointer() {
        if (deployGhost != null) {
            Destroy(deployGhost);
        }
    }

    private void DrawGrid() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                GameObject gridCell = Instantiate(gridPrefab, grid.GetWorldPosition(i, j), Quaternion.identity, gridCells);
            }
        }
    }





}