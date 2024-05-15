using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.loop = true;
        Play_BackgroundMusic();
    }

    public void Play_BackgroundMusic()
    {
        if (audioSource.isPlaying)
        {
            Debug.Log("AudioPlaying");
        }
        else
        {
            audioSource.Play();
        }
    }
}
