using UnityEngine;
using System.Collections;

public class BuildingManager : MonoBehaviour 
{
	public GameObject[] buildings; //Fire Frost Ground Poison

	int[] costChoice = { 150, 100, 50, 100 };

	Building building;
	Player player;

	// Use this for initialization
	void Start () 
	{
		building = GetComponent<Building>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	void OnGUI()
	{

		for(int i = 0, scalar = 1; i < buildings.Length; i++, scalar++)
		{

			//new Rect(Screen.width * .1f, Screen.height - (Screen.height * .15f) - 5, Screen.width / 4, Screen.height * .15f

			if(i == 2)
			{
				scalar+=2;
			}

			if(GUI.Button(new Rect( (Screen.width * .125f) * scalar, Screen.height - (Screen.height * .15f) - 10, Screen.width * .125f, Screen.height * .15f),  buildings[i].name + "\n" + costChoice[i].ToString()))
			{
				if(player.GetCurrency() >= costChoice[i])
				{
					building.SetItem(buildings[i]);
					player.ChargePlayer(costChoice[i]);
				}
			}
		}
	}


	// Update is called once per frame
}
