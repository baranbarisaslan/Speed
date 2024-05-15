using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AppConstants;

public class PlayAudio : MonoBehaviour
{
    AudioSource audioSource;

    bool audioOn;

    public static PlayAudio Instance;


    public static AudioClip correct;
    public static AudioClip wrong;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if(Instance == null)
        {
            Instance = this;
        }
        correct = Resources.Load<AudioClip>(AudioConstants.AudioPath + "correctAnswer");    
        wrong = Resources.Load<AudioClip>(AudioConstants.AudioPath + "wrongAnswer");

    }

    public void Play(AudioClip audioClip)
    {
        if (audioOn)
        {
            if (audioSource.isPlaying)
            {
                Debug.Log("another AudioPlaying");
            }
            else
            {
                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                }
                audioSource.Play();
            }
        }
    }
}
