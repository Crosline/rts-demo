using UnityEngine;

public class Barracks : GridObject {

    public GridObjectScriptable[] soldiers;
    public int selectedSoldier;
    private Vector3 deployPosition = Vector3.one * 0.5f;

    public void SpawnSoldier() {

        Vector2Int temp = GridManager.Instance.pathfinding.GetClosestNode(x, y);

        var gridObject = GridObjectFactory.Instance.GetGridObject($"Soldier {selectedSoldier + 1}", temp.x, temp.y);
        var gridObj = gridObject.GetComponent<GridObject>();

        gridObj.x = temp.x;
        gridObj.y = temp.y;

        gridObj.transform.position = GridManager.Instance.grid.GetWorldPosition(temp.x, temp.y);

        GridManager.Instance.grid.SetValueWithSize(temp.x, temp.y, gridObj.Width, gridObj.Height, gridObj);

        if (deployPosition != Vector3.one * 0.5f) {
            if (GridManager.Instance.grid.GetXY(deployPosition, out var a, out var b)) {
                gridObject.GetComponent<Soldier>().Move(new Vector2Int(a, b));
            }

        }


    }

    private void AddSoldiers(GridObjectScriptable[] soldiers) {
        this.soldiers = soldiers;
    }

    public void SetDeployPos(Vector3 pos) {
        this.deployPosition = pos;
    }
    public Vector3 GetDeployPos() {
        return this.deployPosition;
    }

}
