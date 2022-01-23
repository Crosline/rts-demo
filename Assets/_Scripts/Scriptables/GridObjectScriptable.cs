using UnityEngine;

[CreateAssetMenu(fileName = "GridObject", menuName = "ScriptableObjects/GridObject")]
public class GridObjectScriptable : ScriptableObject {

	public string objName;
	public string info;

	public int maxHealth;

	public int width;
	public int height;

	public Sprite sprite;



}

[CreateAssetMenu(fileName = "GridObjSoldier", menuName = "ScriptableObjects/GridObjSoldier")]

public class SoldierScriptable : GridObjectScriptable {
	public int damage;
}
