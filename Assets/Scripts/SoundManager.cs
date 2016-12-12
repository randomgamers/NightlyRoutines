using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {


	public AudioSource efxSource;
	public List<GameObject> audioSourcePool;

	private static SoundManager instance;
	public static SoundManager Instance {
		get {
			if (instance == null)
				instance = GameObject.FindGameObjectWithTag("Global").GetComponent<SoundManager>();
			return instance;
		}
	}

	public void Start() {
		audioSourcePool = new List<GameObject>();
		
		// Play(backgroundMusic, true);
		// musicSource.clip = backgroundMusic;
		// musicSource.loop = true;
		// musicSource.Play();
		
	}

	public void Update() {
	}

	public AudioSource Play(AudioClip clip) {
		return Play(clip, false);
	}

	public AudioSource Play(AudioClip clip, bool loop) {
		if (!clip) {
			return null;
		}

		AudioSource mySource = null;
		// foreach (GameObject audioSourceOwner in audioSourcePool) {
		// 	AudioSource source = audioSourceOwner.GetComponent<AudioSource>();  
		// 	if (!source.isPlaying) {
		// 		mySource = source;
		// 		audioSourceOwner.SetActive(true);
		// 		break;
		// 	}
		// }
		if (!mySource) {

			GameObject mySourceOwner = Instantiate(Resources.Load("AudioSource")) as GameObject;
			mySourceOwner.SetActive(true);
			// audioSourcePool.Add(mySourceOwner);
			mySource = mySourceOwner.GetComponent<AudioSource>();
		}

		Debug.Log("Shit");

		mySource.enabled = true;
		
		mySource.clip = clip;
		mySource.pitch = Random.Range(0.95f, 1.05f);
		mySource.loop = loop;
		mySource.Play();

		return mySource;
	}


}