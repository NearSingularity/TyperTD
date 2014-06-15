using UnityEngine;
using System.Collections;
using Pathfinding;

public class Building : MonoBehaviour 
{
	private BuildingUpdate curBuilding, lastBuilding;
	private Transform currentBuilding;

	bool hasPlaced;

	Vector3 m_MousePos;

	GameObject myBuilding;

	NavUpdate navUpdate;

	public void SetItem(GameObject go)
	{
		hasPlaced = false;

		myBuilding = (GameObject)GameObject.Instantiate(go);
		currentBuilding = myBuilding.transform;
		curBuilding = myBuilding.GetComponent<BuildingUpdate>();
	}

	bool ValidPlacement()
	{
		GraphNode n = AstarPath.active.GetNearest(currentBuilding.position).node;

		if (n.walkable)
		{	
			Vector3 temp = new Vector3((n.position.x / 1000) + .5f, .5f, (n.position.z / 1000) - .5f); //snap it like its hot
			currentBuilding.position = temp;

			n.Walkable = false;

			//AstarPath.active.Scan();
			AstarPath.active.UpdateGraphs(currentBuilding.collider.bounds);
	
			return true;
		}
		else
			return false;

		//update Astar!
	}

	// Use this for initialization
	void Start () {
		navUpdate = GetComponent<NavUpdate> ();
	}
	
	// Update is called once per frame
	void Update () 
	{	
		m_MousePos = Input.mousePosition;
		Vector3 p = camera.ScreenToWorldPoint(m_MousePos);

		int buildingMask = 9;

		if(currentBuilding != null && !hasPlaced)
		{
			currentBuilding.position = new Vector3(p.x , .5f, p.z);

			if(Input.GetMouseButtonDown(0))
			{
				if(ValidPlacement())
					hasPlaced = true;
			}
		}
		else {
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = new Ray(new Vector3(p.x, 15, p.z), Vector3.down);

				if(Physics.Raycast(ray, out hit, Mathf.Infinity))
				{
					if(lastBuilding != null)
						lastBuilding.SetSelected(false);
					if(hit.collider.tag == "Tower")
					{
						hit.collider.gameObject.GetComponent<BuildingUpdate>().SetSelected(true);
						lastBuilding = hit.collider.gameObject.GetComponent<BuildingUpdate>();
					}

				}
				else{
					if(lastBuilding != null)
						lastBuilding.SetSelected(false);
				}
			}
		}
	
	}
}
