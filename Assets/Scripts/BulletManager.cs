using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour 
{
	Vector3 s_Pos, m_Pos, m_Target, m_Velocity;

	float m_Speed;

	SpriteRenderer bulletRenderer;
	Sprite bullet;

	BulletUpdate update;

	GameObject bulletObj, targetObj, player;

	GameObject[] bullets;

	void OnEnable()
	{
		//set any stats.
		//check any pre-reqs.

		//find target.

		//Fire();
	}
	void OnDisable()
	{

	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		s_Pos = m_Pos = player.transform.position;
		
		m_Speed = 50;
	}



	public void Fire(Sprite sprite, GameObject target, int id, GameObject origin)
	{
		bullet = sprite;
		targetObj = target;
		m_Target = target.transform.position;

		bulletObj = ObjectPool.instance.GetObjectForType("Bullet", true);
		bulletRenderer = bulletObj.GetComponent<SpriteRenderer>();

		if(bulletRenderer)
			bulletRenderer.sprite = bullet;
		Vector3 offSet = new Vector3(origin.transform.position.x, origin.transform.position.y, origin.transform.position.z + 2);

		bulletObj.transform.position = offSet;
		bulletObj.GetComponent<BulletUpdate>().SetId(id);

		float t = Vector3.Distance(m_Target, offSet);
		bulletObj.GetComponent<BulletUpdate> ().SetTime (t / 10);
		bulletObj.GetComponent<BulletUpdate> ().SetTarget (targetObj);
		bulletObj.GetComponent<BulletUpdate> ().Fire (offSet);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//bullets = GameObject.FindGameObjectsWithTag("Bullet");
	}
}
