using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioLord : MonoBehaviour
{
    public AudioClip music;
    private void Awake()
    {
        Instance.clip = music;
        Instance.Play();
        Instance.loop = true;
    }

    public static AudioSource Instance
    {
        get
        {
            if (!bgm)
            {
                GameObject audioLord = new GameObject("audioLord");
                bgm = audioLord.AddComponent<AudioSource>();
                DontDestroyOnLoad(audioLord);
            }

            return bgm;
        }
    }

    public static GameObject singletonRoot;
    private static AudioSource bgm;

    public static void AudioPlay()
    {

    }
}