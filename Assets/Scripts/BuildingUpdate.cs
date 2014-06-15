using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingUpdate : MonoBehaviour {
	
 	List<Collider> colliders = new List<Collider>();

	//Get a List of A* nodes. 

	private bool isSelected;

	int index, health, damage, offset;

	GameObject bullet, target;
	Sprite currentBullet;
	Player player;
	BulletManager bulletManager;


	float currentTime, reloadTime;

	List<Sprite> m_Ammo = new List<Sprite>();

	char targetLetter;


	void OnGUI()
	{
		if(isSelected)
		{
			guiText.text = name;
			guiText.pixelOffset = new Vector2(transform.position.x, transform.position.z);
		}
	}
	
	// Use this for initialization
	void Start () {
		health = 100;
		damage = 50;
		bulletManager = GetComponent<BulletManager>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();;

		for(int i = 0; i < player.ammo.Length; i++)
		{
			m_Ammo.Add(player.ammo[i]);
		}

		switch(gameObject.tag)
		{
		case "EarthTower":
			reloadTime = 2.0f;
			break;
		case "FireTower":
			reloadTime = .75f;
			break;
		case "FrostTower":
			reloadTime = 1.5f;
			break;
		case "PoisonTower":
			reloadTime = 1.0f;
			break;
		}

	}

	void TakeDamage()
	{
		health -= damage;
	}

	void OnTriggerEnter(Collider c)
	{
		//print (c.name);
		//print (c.tag);
		//print (c.gameObject.tag);

		if(c.tag == "Enemy")
		{
			print ("ENEMY IN SIGHT" );
			target = c.gameObject;
			targetLetter = c.gameObject.GetComponent<Enemy>().curChar;
			index = targetLetter.GetHashCode();
			if(index < 91 && index > 64)
			{
				offset = index%65;
				currentBullet = m_Ammo[offset];
			}
			else if(index < 123 && index > 96)
			{
				offset = index%97;
				currentBullet = m_Ammo[offset];
			}
		}

		if(c.tag == "Tower")
		{
			colliders.Add(c);
		}
	}

	void OnTriggerExit(Collider c)
	{
		if(c.tag == "Enemy")
		{
			target = null;
		}

		if(c.tag == "Tower")
		{
			colliders.Remove(c);
		}
	}

	public void SetSelected(bool s)
	{
		isSelected = s;
	}

	// Update is called once per frame
	void Update()
	{
		if(health <= 0)
			Destroy(gameObject);

		if(target)
		{
			if(target.activeSelf)
			{
				currentTime += Time.deltaTime;

				if(currentTime >= reloadTime)
				{
					targetLetter = target.GetComponent<Enemy>().GetCurrentChar();
					index = targetLetter.GetHashCode();
					if(index < 91 && index > 64)
					{
						offset = index%65;
						currentBullet = m_Ammo[offset];
					}
					else if(index < 123 && index > 96)
					{
						offset = index%97;
						currentBullet = m_Ammo[offset];
					}

					bulletManager.Fire(currentBullet, target, index, gameObject);
					currentTime = 0;
				}
			}
		}

	}

}
