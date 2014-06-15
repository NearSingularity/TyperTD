using UnityEngine;
using System.Collections;
using Pathfinding;

public class NavUpdate : MonoBehaviour {

	GraphUpdateScene graphUpdate;
	GraphUpdateObject guo;

	const int LAYER_MASK = 9;

	// Use this for initialization
	void Start () {
		graphUpdate = GameObject.FindGameObjectWithTag("Update").GetComponent<GraphUpdateScene> ();

	}

	public void UpdateGraph(GameObject go)
	{
		if(go.layer == LAYER_MASK )
			AstarPath.active.UpdateGraphs (go.collider.bounds);

		guo = new GraphUpdateObject(go.collider.bounds);

		AstarPath.active.UpdateGraphs(guo);

		graphUpdate.Apply();
		AstarPath.active.FloodFill();
		AstarPath.active.Scan();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
