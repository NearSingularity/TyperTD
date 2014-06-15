using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

/*
 * Spawn. (Top, farthest away from player , wherever I decide that to be)
 * Find Player, Look at him.
 * Move towards Player at given speed.
 * If hit destroy-pool.
*/

public class Enemy : MonoBehaviour 
{
	const int nextNodeDist = 1;
	const int LAYER_MASK = 9;

	List<string> WORDS = new List<string>();

	public Sprite[] sprites;
	public GameObject coin;
	GameObject player;
	TextMesh m_Word;

	Vector3 m_Pos, m_Target, m_Velocity, difference, maxVelocity;

	float m_Speed;
	string myWord;
	public char curChar;
	int activeIndex, currentNode;

	CharacterController controller;

	AIPath p;
	Seeker seek;
	Path path;
	GameManager manager;

	bool isPissed;
	
	void Awake()
	{
		//Switch to text files separated by length of word.
		WORDS.Add("Act");
		WORDS.Add("Add");
		WORDS.Add("Ace");
		WORDS.Add("Ape");
		WORDS.Add("Ark");
		WORDS.Add("Ash");
		WORDS.Add("Bog");
		WORDS.Add("Bid");
		WORDS.Add("Bob");
		WORDS.Add("Box");
		WORDS.Add("Car");
		WORDS.Add("Cot");
		WORDS.Add("Dam");
		WORDS.Add("Dew");
		WORDS.Add("Dig");

	}

	// Use this for initialization
	void Start () {

		player 		= GameObject.FindGameObjectWithTag("Player");
		manager 	= GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	
		m_Pos 		= transform.position;
		m_Target 	= player.transform.position;
		m_Velocity 	= Vector3.zero;
		maxVelocity = new Vector3(5.0f, 5.0f, 5.0f);

		m_Speed 	= 50.0f;

		controller = GetComponent<CharacterController>();
		seek = GetComponent<Seeker>();
		seek.StartPath(m_Pos, m_Target, OnPathComplete);
		//seek.pathCallback += OnPathComplete;
		//seek.StartPath(m_Pos, m_Target, OnPathComplete);
		p = GetComponent<AIPath> ();
		p.target = player.transform;

		isPissed = false;
	}

	public void StartNewPath()
	{
		seek = GetComponent<Seeker>();
		seek.StartPath(m_Pos, m_Target, OnPathComplete);
		p = GetComponent<AIPath> ();
		p.target = player.transform;
	}

	public char GetCurrentChar()
	{
		return curChar;
	}

	void ToggleAttack(bool f)
	{
		isPissed = !f;
	}
	
	public void OnPathComplete(Path p)
	{
		//Debug.Log ("Error? -" + p.error);
		if(!p.error) //if no error
		{
			path = p; //set path to correct path
			currentNode = 0; //reset index
			path.Claim(this);
		}
		else{
			ToggleAttack(isPissed);
		}
	}

	void GenerateNewWord()
	{
		int r = Random.Range (0, WORDS.Count);
		myWord = WORDS[r];

		m_Word 		= transform.GetChild(0).GetComponent<TextMesh>();
		m_Word.text = myWord;

		activeIndex = currentNode = 0;
		curChar = myWord[activeIndex]; 	//set currentChar (the target)
	}

	void OnEnable()
	{
		//int i = Random.Range(0, sprites.Length);

		//GetComponentInChildren<SpriteRenderer>().sprite = sprites[i];
	
		GenerateNewWord();
		float r = Random.Range(-10.0f, 10.0f);
		//spawn at random point at the top of the screen.
		m_Pos = new Vector3(r, .75f, 2);
		m_Target = GameObject.FindGameObjectWithTag("Player").transform.position;
		transform.position = m_Pos;

		//StartCoroutine ("GetCompon");
		seek = GetComponent<Seeker> ();
		//ask instead of being told.
		seek.GetNewPath (m_Pos, m_Target);
	}

	void DropCoin()
	{
		int r = Random.Range(0, 100);
		
		if(r <= 40)
		{
			GameObject coin = ObjectPool.instance.GetObjectForType("Coin", true);
			coin.transform.position = transform.position;
		}
	}

	void OnDisable()
	{
	}


	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		string tag = hit.collider.tag;

		switch(tag)
		{
		case "Player":
			hit.collider.gameObject.GetComponent<Player>().Hit();
			ObjectPool.instance.PoolObject (gameObject);
			break;
		case "Bullet":
			if(curChar.GetHashCode() == hit.collider.gameObject.GetComponent<BulletUpdate>().GetId())
			{
				//drop the first character
				myWord = myWord.Substring(1);

				if(myWord.Length <= 0)
				{
					DropCoin();
					manager.AddKill();
					if(gameObject)
					{
						ObjectPool.instance.PoolObject(gameObject);
					}
				}
				else{
					//update current active character
					curChar = myWord[0];
					
					//update text mesh
					m_Word.text = myWord;
				}
				
				ObjectPool.instance.PoolObject(hit.collider.gameObject);
			}
			else
				ObjectPool.instance.PoolObject(hit.collider.gameObject);
			break;
		}
	}
	/*
	void OnCollisionEnter(Collision col)
	{
		//print ("Collided");
		//I hit something.
		switch(col.collider.tag)
		{
		case "Player":
			//I hit player Pool/Destroy myself
			col.gameObject.GetComponent<Player>().Hit();
			manager.RemoveEnemy(gameObject);
			ObjectPool.instance.PoolObject(gameObject);
			//GameObject.Destroy(gameObject);
			break;
		case "Bullet":
			if(curChar.GetHashCode() == col.gameObject.GetComponent<BulletUpdate>().GetId())
			{
				if(myWord.Length <= 1)
				{
					manager.RemoveEnemy(gameObject);
					ObjectPool.instance.PoolObject(gameObject);
				}
				//drop the first character
				myWord = myWord.Substring(1);
				
				//update current active character
				curChar = myWord[0];
				
				//update text mesh
				m_Word.text = myWord;
				ObjectPool.instance.PoolObject(col.gameObject);
			}
			else{
				ObjectPool.instance.PoolObject(col.gameObject);
			}

			break;
		}
	}
*/
	public bool CheckInput(string input)
	{
		//check to see if input matches current active character
		if(input[0] == curChar)
		{
		
			//we got a match!
			if(myWord.Length <= 1)
			{
				ObjectPool.instance.PoolObject(gameObject);
				return true;
			}
			//drop the first character
			myWord = myWord.Substring(1);

			//update current active character
			curChar = myWord[0];

			//update text mesh
			m_Word.text = myWord;

			return true;

		}
		else
			return false;
	}

	void FixedUpdate()
	{
		m_Pos = transform.position;

		if(path == null)
			return;
		if(currentNode >= path.vectorPath.Count) //hit last node?
			return;
		
		Vector3 dir = (path.vectorPath[currentNode]-transform.position).normalized;
		dir *= m_Speed * Time.fixedDeltaTime;
		
		//rigidbody.AddForce(dir);
		controller.SimpleMove(dir);

		if(Vector3.Distance(m_Pos, path.vectorPath[currentNode]) < nextNodeDist)
		{
			currentNode++;
			return;
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		//update Position, Target, and Velocity
		m_Pos 		= transform.position;
		m_Target	= player.transform.position;
		//m_Velocity  = rigidbody2D.velocity;
		//m_Velocity = rigidbody.velocity;

		difference = m_Target - m_Pos;
		difference.Normalize();

		Vector2 dif = new Vector2(difference.x, difference.y);

		if(isPissed)
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position, transform.forward, out hit, 15))
			{
				if(hit.collider.gameObject.layer == LAYER_MASK)
				{
					//DESTROY TOWER
					hit.rigidbody.gameObject.SendMessage("TakeDamage", SendMessageOptions.RequireReceiver);
				}
			}
		}

		//rigidbody2D.AddForce(dif * m_Speed);

	}
}
