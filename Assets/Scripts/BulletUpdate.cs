using UnityEngine;
using System.Collections;

public class BulletUpdate : MonoBehaviour 
{
	GameObject player, target;
	float lifeTime, timer, dt, t;

	int id;


	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		lifeTime = 3.0f;
	}

	public void SetTarget(GameObject t)
	{
		target = t;
	}
	public void SetTime(float f)
	{
		t = f;
	}

	public void Fire()
	{
		StartCoroutine (MoveToPos (t, target));
	}

	public void Fire(Vector3 offSet)
	{
		StartCoroutine (MoveToPos (t, target, offSet));
	}
	public IEnumerator MoveToPos(float time, GameObject target, Vector3 startPos)
	{
		float elapsed = 0.0f;
		Vector3 endPos = target.transform.position;
		
		while(elapsed < time)
		{
			endPos = target.transform.position; //if target is moving, keep updating endPos
			transform.position = Vector3.Lerp(startPos, endPos, (elapsed/time));
			elapsed += Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator MoveToPos(float time, GameObject target)
	{
		float elapsed = 0.0f;
		Vector3 startPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		Vector3 endPos = target.transform.position;
		
		while(elapsed < time)
		{
			endPos = target.transform.position; //if target is moving, keep updating endPos
			transform.position = Vector3.Lerp(startPos, endPos, (elapsed/time));
			elapsed += Time.deltaTime;
			yield return null;
		}
	}

	public void SetId(int i)
	{
		id = i;
	}

	public int GetId()
	{
		return id;
	}
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Bullet" || col.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
			return;
	}
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if(timer >= lifeTime)
			ObjectPool.instance.PoolObject(gameObject);
	
	}
}
