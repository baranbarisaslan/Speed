using System.Collections;
using UnityEngine;

public class Card_Option : MonoBehaviour
{
    private bool isFlipped = false;
    private float flipDuration = 0.5f; // Duration of the flip animation
    GameObject child;

    private void Start()
    {
        child = gameObject.transform.GetChild(0).gameObject;
    }

    // Function to flip the card
    public void Flip()
    {
        Debug.Log("FLIPPED");
        isFlipped = !isFlipped;
        StartCoroutine(FlipAnimation());
    }

    // Coroutine for the flip animation
    IEnumerator FlipAnimation()
    {
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
