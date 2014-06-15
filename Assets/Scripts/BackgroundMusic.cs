using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	public AudioClip[] backgroundMusic;
	AudioSource source;

	int random;

	// Use this for initialization
	void Start () {

		source = GetComponent<AudioSource> ();
		random = Random.Range (0, backgroundMusic.Length);

		source.PlayOneShot (backgroundMusic [random]);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			source.Stop();
			random--;
			if(random < 0)
				random = backgroundMusic.Length - 1;
			source.PlayOneShot (backgroundMusic [random]);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			source.Stop();
			random++;
			if(random >= backgroundMusic.Length)
				random = 0;
			source.PlayOneShot (backgroundMusic [random]);
		}

	}
}
