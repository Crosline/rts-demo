using UnityEngine;

public class Barracks : GridObject {

    public GridObject[] soldiers;
    public int selectedSoldier;
    private Vector3 deployPosition = Vector3.one * 0.5f;

    public void SpawnSoldier() {

        if (GridManager.Instance.grid.GetXY(transform.position, out var tempx, out var tempy)) {


            var gridObject = GridObjectFactory.Instance.GetGridObject($"Soldier {selectedSoldier + 1}", tempx, tempy);
            var gridObj = gridObject.GetComponent<GridObject>();

            if (GridManager.Instance.grid.GetXY(GridManager.Instance.pathfinding.GetClosestNode(x, y), out int a, out int b)) {
                var obj = Instantiate(gridObj);
                obj.transform.position = new Vector3(a, b);

                GridManager.Instance.grid.SetValueWithSize(x, y, gridObj.Width, gridObj.Height, obj.GetComponent<GridObject>());
            }
        }




    }

    private void AddSoldiers(GridObject[] soldiers) {
        this.soldiers = soldiers;
    }

    public void SetDeployPos(Vector3 pos) {
        this.deployPosition = pos;
    }
    public Vector3 GetDeployPos() {
        return this.deployPosition;
    }

}
