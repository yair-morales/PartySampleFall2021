using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioClip bgm;
	
	public void Awake() {
		var audioSource = GetComponent<AudioSource>();
		if (audioSource == null) {
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		audioSource.clip = bgm;
		audioSource.loop = true;
		
		if (audioSource.clip) audioSource.Play();
	}
}
