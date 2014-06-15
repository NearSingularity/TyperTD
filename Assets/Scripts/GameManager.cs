using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;
	int m_Difficulty, m_Score, m_Round, m_RoundLimit;

	float m_SpawnTime, m_SpawnTimer, m_RoundTime, m_RoundTimer, m_Shortest, m_Scalar, camSize;

	bool m_Ready, m_Done;

	Player tehPlayer;

	GameObject enemyPool, enemy, cam, ground;
	GameObject[] active, inactive;
	List<GameObject> enemies = new List<GameObject>();

	Vector3 currentScale, wantedScale;

	GridGraph graph;
	GraphUpdateScene gus;

	GUIText roundGUI;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		m_Difficulty = 5;
		m_Score = 0;
		m_Round = 1;
		m_RoundLimit = m_Round * 5;
		m_RoundTime = m_Round * 15;
		m_SpawnTime = m_Difficulty;
		m_Shortest = Mathf.Infinity;
		m_Scalar = 1;
		m_Ready = m_Done = false;

		tehPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		enemyPool = GameObject.FindGameObjectWithTag("Container");
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		camSize = cam.camera.orthographicSize;
		ground = GameObject.FindGameObjectWithTag("Ground");
		currentScale = ground.transform.localScale;

		graph = (GridGraph)GameObject.FindGameObjectWithTag("Graph").GetComponent<AstarPath>().graphs[0];
		gus = GameObject.FindGameObjectWithTag ("Update").GetComponent<GraphUpdateScene> ();
		roundGUI = GameObject.FindGameObjectWithTag ("RoundGUI").GetComponent<GUIText> ();
	}

	//Manager will spawn enemies at a set interval at a random width, fixed height
	//Manager then will set the active enemy's index for a sorted array based of what spawned first -- soon change to distance.
	//

	public GameObject GetClosestEnemy()
	{
		m_Shortest = Mathf.Infinity;
		if(enemies.Count > 0)
		{
			if(enemies.Count == 1)
				return enemies[0];

			for(int i = 0; i < enemies.Count; i++)
			{
				GameObject temp = enemies[i];
			
				if(temp.activeSelf)
				{
					float dist = Vector3.Distance(temp.transform.position, tehPlayer.transform.position);

					if(dist < m_Shortest)
					{
						enemy = temp;
						m_Shortest = dist;
					}
				}
			}
			return enemy;

		}
		else
			return null;
	}

	public void AddKill()
	{
		m_Score++;
	}

	public void RemoveEnemy(GameObject go)
	{
		for(int i = 0; i < enemies.Count; i++)
		{
			if(enemies[i] == go)
			{
				enemies.RemoveAt(i);
				return;
			}
		}
	}

	void SpawnEnemy()
	{
		float random = UnityEngine.Random.Range(0f, 1f);
		GameObject go;
		if(random > .5f){
			go = ObjectPool.instance.GetObjectForType("ZombieGreen", true);
			enemies.Add(go);
		}
		else {
			go = ObjectPool.instance.GetObjectForType("ZombieRed", true);
			enemies.Add(go);
		}

	}

	void Restart()
	{
		m_Difficulty = 5;
		m_Score = 0;
		m_Round = 1;
		m_RoundLimit = m_Round * 5;
		m_RoundTime = m_Round * 30;
		m_SpawnTime = m_Difficulty;
	}
	void UpdateLevel()
	{
		//resize graph nodes.
		graph.width = 30;
		graph.depth = 30;
		graph.UpdateSizeFromWidthDepth();
		graph.center = new Vector3(0, -1, 0);
		AstarPath.active.Scan();

		for(var i = 0; i < enemies.Count; i++)
		{
			enemies[i].GetComponent<Enemy>().StartNewPath();
		}
	}
	void NextRound()
	{
		m_Difficulty = 5;
		m_Round++;
		m_RoundLimit = m_Score + (m_Round * 50);
		m_RoundTime = m_Round * 15;
		if(m_RoundTime >= 30)
			m_RoundTime = 30;
		m_SpawnTime = m_Difficulty;
		m_Done = false;

		roundGUI.fontSize = 35;
		UpdateLevel();
		roundGUI.enabled = true;


	}


	// Update is called once per frame
	void Update () 
	{
		m_SpawnTimer += Time.deltaTime;
		m_RoundTimer += Time.deltaTime;

		float currentSize = cam.camera.orthographicSize;

		roundGUI.text = "Round " + m_Round;
		roundGUI.fontSize++;
		if(roundGUI.fontSize >= 125)
		{
			roundGUI.enabled = false;
			m_Done = true;
		}

		wantedScale = new Vector3(currentScale.x + 0.5f, 1.0f, currentScale.z + 0.5f);
		cam.camera.orthographicSize = Mathf.Lerp (currentSize, m_Round + 5, Time.deltaTime);
		ground.transform.localScale = Vector3.Lerp(currentScale, wantedScale, 1);

		if(m_Score >= m_RoundLimit)
		{
			NextRound(); //New Round
			m_Score = 0; //Reset Score
			m_RoundTimer = 0;
			currentScale = ground.transform.localScale;
		}

		if(m_SpawnTimer >= m_SpawnTime)
		{

			SpawnEnemy();
			
			m_SpawnTimer = 0;
		}

	}
}
