using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	int lSelection, rSelection;
	public string[] leftTypes = new string[] { "Frost Tower", "Burn Tower" };
	public string[] rightTypes = new string[] { "Poison Tower", "Earth Tower" };

	public GameObject[] towers;

	void OnGUI()
	{
		//print (lSelection);
		//print (rSelection);
		//lSelection = GUI.SelectionGrid (new Rect(Screen.width * .1f, Screen.height - (Screen.height * .15f) - 5, Screen.width / 4, Screen.height * .15f), lSelection, leftTypes, leftTypes.Length); 
		//rSelection = GUI.SelectionGrid (new Rect(Screen.width * .65f, Screen.height - (Screen.height * .15f) - 5, Screen.width / 4, Screen.height * .15f), rSelection, rightTypes, rightTypes.Length);  

		//lSelection == 0 ? building.SetItem(towers[lSelection]);
	
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
