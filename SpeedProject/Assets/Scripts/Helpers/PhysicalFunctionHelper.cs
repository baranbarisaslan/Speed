using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalFunctionHelper : MonoBehaviour
{
    public static IEnumerator Fade(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        if(canvasGroup != null)
        {
            float currentTime = 0f;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
                canvasGroup.alpha = alpha;
                yield return null;
            }
            canvasGroup.alpha = endAlpha;
        }

    }

    public static IEnumerator ScaleOverTime(RectTransform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = targetScale;
    }

}
