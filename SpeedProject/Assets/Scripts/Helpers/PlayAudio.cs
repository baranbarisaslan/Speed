using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AppConstants;

public class PlayAudio : MonoBehaviour
{
    static AudioSource audioSource;

    public static PlayAudio Instance;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if(Instance == null)
        {
            Instance = this;
        }


    }

    public static void Play(AudioClip audioClip)
    {
        if (audioSource.isPlaying)
        {
           audioSource.Stop();
        }
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
