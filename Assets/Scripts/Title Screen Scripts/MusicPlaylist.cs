using UnityEngine;
using System.Collections;

public class MusicPlaylist : MonoBehaviour {

	public AudioClip[] clips;
	private AudioSource audioSource;
    private GlobalController GC;

	void Start () {
        audioSource = GameObject.Find("MusicBox").GetComponent<AudioSource>();
		audioSource.loop = false;

	}

	private AudioClip GetRandomClip() {
		return clips[Random.Range (0, clips.Length)];
	}

	//Update is called once per frame
	void Update() {

        if (!audioSource.isPlaying) {
			audioSource.clip = GetRandomClip();
			audioSource.Play();
		}

	}

}