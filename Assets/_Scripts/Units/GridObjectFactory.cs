using _Utilities;
using UnityEngine;

public class GridObjectFactory : Singleton<GridObjectFactory> {

	public GameObject[] objects;    

	public GridObjectScriptable[] scriptables;

	enum Scriptables {
		Barracks = 0,
		House = 1,
		Mine = 2,
		Powerplant = 3,
		TownCenter = 4,
		WindMill = 5,
	}
	enum Soldiers {
		Soldier1 = 6,
		Soldier2 = 7,
		Soldier3 = 8,
	}

	public GameObject GetGridObject(string objectName, int x, int y){
		GridObject temp = objects[0].GetComponent<GridObject>();

		switch (objectName) {
			case "Barracks":
				temp = objects[1].GetComponent<GridObject>();
				temp.SetObjectDetails(scriptables[(int) Scriptables.Barracks]);
				temp.x = x;
				temp.y = y;
				return objects[1];
			case "House":
				temp.SetObjectDetails(scriptables[(int)Scriptables.House]);
				temp.x = x;
				temp.y = y;
				return objects[0];
			case "Mine":
				temp.SetObjectDetails(scriptables[(int)Scriptables.Mine]);
				temp.x = x;
				temp.y = y;
				return objects[0];
			case "Powerplant":
				temp.SetObjectDetails(scriptables[(int)Scriptables.Powerplant]);
				temp.x = x;
				temp.y = y;
				return objects[0];
			case "Town Center":
				temp.SetObjectDetails(scriptables[(int)Scriptables.TownCenter]);
				temp.x = x;
				temp.y = y;
				return objects[0];
			case "Wind Mill":
				temp.SetObjectDetails(scriptables[(int)Scriptables.WindMill]);
				temp.x = x;
				temp.y = y;
				return objects[0];
			case "Soldier":
				temp = objects[2].GetComponent<GridObject>();
				temp.SetObjectDetails(scriptables[(int)Soldiers.Soldier1]);
				temp.x = x;
				temp.y = y;
				((Soldier)temp).damage = 10;

				return objects[2];
			case "Soldier 1":
				GameObject soldier = PoolManager.Instance.SpawnObject(objects[2], GridManager.Instance.grid.GetWorldPosition(temp.x, temp.y), Quaternion.identity);
				temp = soldier.GetComponent<GridObject>();
				temp.SetObjectDetails(scriptables[(int)Soldiers.Soldier1]);
				temp.x = x;
				temp.y = y;
				((Soldier)temp).damage = 10;
				return soldier;
			case "Soldier 2":

				soldier = PoolManager.Instance.SpawnObject(objects[2], GridManager.Instance.grid.GetWorldPosition(temp.x, temp.y), Quaternion.identity);
				temp = soldier.GetComponent<GridObject>();
				temp.SetObjectDetails(scriptables[(int)Soldiers.Soldier2]);
				temp.x = x;
				temp.y = y;
				((Soldier)temp).damage = 5;
				return soldier;
			case "Soldier 3":

				soldier = PoolManager.Instance.SpawnObject(objects[2], GridManager.Instance.grid.GetWorldPosition(temp.x, temp.y), Quaternion.identity);
				temp = soldier.GetComponent<GridObject>();

				temp.SetObjectDetails(scriptables[(int)Soldiers.Soldier3]);
				temp.x = x;
				temp.y = y;
				((Soldier)temp).damage = 2;
				return soldier;
			default:
				return null;
		}
	}

}