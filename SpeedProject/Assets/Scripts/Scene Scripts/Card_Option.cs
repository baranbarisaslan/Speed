using System.Collections;
using UnityEngine;
using static AppConstants;

public class Card_Option : MonoBehaviour
{
    private bool isFlipped = false;
    private float flipDuration = 0.5f;
   public GameObject child;

    public string planet;

    public void Home()
    {
        NavigationManager.SetMainScene(NavigationManager.SceneName.Menu);
    }

    public void GameFlip()
    {
        Debug.Log("GAME FLIP");
        AudioClip clip = Resources.Load<AudioClip>(AudioConstants.AudioPath + "audio_flip");
        PlayAudio.Play(clip);

        if(FindAnyObjectByType<PlayMode_Control>().flippedCards.Contains(gameObject))
        {
            FindAnyObjectByType<PlayMode_Control>().flippedCards.Remove(gameObject);
            GenerateFlip();
        }
        else
        {
            FindAnyObjectByType<PlayMode_Control>().AddCardToList(gameObject);
            StartCoroutine(FlipAnimation());
        }

    }


    public void GenerateFlip()
    {
        Debug.Log("GENERATE FLIP");
        StartCoroutine(FlipAnimation());
    }

    IEnumerator FlipAnimation()
    {
        isFlipped = !isFlipped;
        float elapsedTime = 0f;
        Quaternion initialRotation = child.transform.rotation;
        Quaternion targetRotation;

        if(isFlipped)
            targetRotation = Quaternion.Euler(0f, 180f, 0f); 
        else
            targetRotation = Quaternion.Euler(0f, 0f, 0f); 

        while (elapsedTime < flipDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / flipDuration);
            child.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        child.transform.rotation = targetRotation; 
    }
}
