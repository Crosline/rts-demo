using _Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager> {

	public Transform root;

	private Dictionary<GameObject, Pool<GameObject>> prefabLookup;
	private Dictionary<GameObject, Pool<GameObject>> instanceLookup;

	public void Init() {
		prefabLookup = new Dictionary<GameObject, Pool<GameObject>>();
		instanceLookup = new Dictionary<GameObject, Pool<GameObject>>();

		WarmPool(GridObjectFactory.Instance.objects[2], 50);
	}

	public void WarmPool(GameObject prefab, int size) {
		if (prefabLookup.ContainsKey(prefab)) {
			Debug.Log("Pool for prefab " + prefab.name + " has already been created");
		}
		var pool = new Pool<GameObject>(size, () => { var obj = InstantiatePrefab(prefab); obj.SetActive(false); return obj; });
		prefabLookup[prefab] = pool;

	}

	public GameObject SpawnObject(GameObject prefab) {
		return this.SpawnObject(prefab, Vector3.zero, Quaternion.identity);
	}

	public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation) {
		if (!prefabLookup.ContainsKey(prefab)) {
			WarmPool(prefab, 50);
		}

		var pool = prefabLookup[prefab];

		var clone = pool.GetItem();
		clone.transform.SetPositionAndRotation(position, rotation);
		clone.SetActive(true);

		instanceLookup.Add(clone, pool);

		return clone;
	}

	public void ReleaseObject(GameObject clone) {
		clone.SetActive(false);

		if (instanceLookup.ContainsKey(clone)) {
			instanceLookup[clone].ReleaseItem(clone);
			instanceLookup.Remove(clone);

		} else {
			Debug.LogWarning("No pool contains the object: " + clone.name);
		}
	}


	private GameObject InstantiatePrefab(GameObject prefab) {
		var go = Instantiate(prefab) as GameObject;
		if (root != null) go.transform.parent = root;
		return go;
	}

}
