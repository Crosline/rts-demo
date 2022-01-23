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
				temp.SetGridObjectCoord(x, y);
				return objects[1];
			case "House":
				temp.SetObjectDetails(scriptables[(int)Scriptables.House]);
				temp.SetGridObjectCoord(x, y);
				return objects[0];
			case "Mine":
				temp.SetObjectDetails(scriptables[(int)Scriptables.Mine]);
				temp.SetGridObjectCoord(x, y);
				return objects[0];
			case "Powerplant":
				temp.SetObjectDetails(scriptables[(int)Scriptables.Powerplant]);
				temp.SetGridObjectCoord(x, y);
				return objects[0];
			case "Town Center":
				temp.SetObjectDetails(scriptables[(int)Scriptables.TownCenter]);
				temp.SetGridObjectCoord(x, y);
				return objects[0];
			case "Wind Mill":
				temp.SetObjectDetails(scriptables[(int)Scriptables.WindMill]);
				temp.SetGridObjectCoord(x, y);
				return objects[0];
			case "Soldier 1":
				temp = objects[2].GetComponent<GridObject>();
				temp.SetObjectDetails(scriptables[(int)Soldiers.Soldier1]);
				temp.SetGridObjectCoord(x, y);
				return objects[2];
			case "Soldier 2":
				temp = objects[2].GetComponent<GridObject>();
				temp.SetObjectDetails(scriptables[(int)Soldiers.Soldier2]);
				temp.SetGridObjectCoord(x, y);
				return objects[2];
			case "Soldier 3":
				temp = objects[2].GetComponent<GridObject>();
				temp.SetObjectDetails(scriptables[(int)Soldiers.Soldier3]);
				temp.SetGridObjectCoord(x, y);
				return objects[2];
			default:
				return null;
		}
	}

}