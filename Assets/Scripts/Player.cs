using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	char characterPressed;
	int m_Life, m_Currency, offset;
	string input;

	Collider2D[] enemyColliders;

	public Collider[] enemyCols;

	public Sprite[] ammo;
	Sprite currentBullet;

	List<GameObject> enemies;
	List<float> distances;

	GameObject target;

	BulletManager bullets;
	GameManager manager;

	Vector3 m_Pos, temp, prevTemp;

	//Use this for initialization
	void Start () 
	{
		m_Currency = m_Life = 100;
		m_Pos = transform.position;
		bullets = GetComponent<BulletManager>();
		manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		guiText.text = "Currency : " + m_Currency;
	}

	public GameObject GetTarget()
	{
		return target;
	}

	public void Hit()
	{
		m_Life--;
		//print (m_Life);
	}

	public int GetCurrency()
	{
		return m_Currency;
	}

	public void ChargePlayer(int val)
	{
		m_Currency -= val;
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		m_Life--;
		//print (m_Life);
	}

	void OnCollisionEnter(Collision col)
	{
		m_Life--;
		print (m_Life);
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		m_Life--;
		print (m_Life);
	}
	
	// Update is called once per frame
	 void Update () 
	{
		m_Pos = transform.position;
		guiText.text = "Currency : " + m_Currency;
		target = manager.GetClosestEnemy();//Update list of enemies
		
		//Handle mouse/keyboard for now

		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit, 10.0f))
			{
				switch(hit.collider.tag)
				{
				case "Coin":
					ObjectPool.instance.PoolObject(hit.collider.gameObject);
					m_Currency += 150;
					break;
				default:
					break;
				}
			}
		}

		//some keyboard input was detected only ASCII can be represented which is fine.
		input = Input.inputString;
		//if input was captured
		if(input != "")
		{
			//check to see if any active enemy contains this.
			//check each active enemy linearly.
			//or
			//closest distance first **
			//char key = input;

			if(target)
			{
				if(target.activeSelf)
				{
					//Fire a 2D letter at that enemy
					int index = input.GetHashCode();

					if(index < 91 && index > 64)
					{
						offset = index%65;
						currentBullet = ammo[offset];
					}
					else if(index < 123 && index > 96)
					{
						offset = index%97;
						currentBullet = ammo[offset];
					}

					bullets.Fire (currentBullet, target, index, this.gameObject);
				}
			}

		}
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;

		Gizmos.DrawWireSphere (transform.position, 100);
	}
}
