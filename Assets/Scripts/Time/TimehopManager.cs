using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimehopState
{
    Morning,
    Afternoon,
    Evening
}
public class TimehopManager : MonoBehaviour
{
    [Header("Timehop Settings")]
    [SerializeField] public TimehopState currentState;
    private bool isFading = false;

    [Header("Sprites")]
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite afternoonSprite;
    [SerializeField] private Sprite eveningSprite;

    [Header("Renderers")]
    [SerializeField] private SpriteRenderer currentSky; 
    [SerializeField] private SpriteRenderer nextSky;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 1.0f; // fade sky time


    public void ChangeTime(TimehopState newState)
    {
        if (isFading || currentState == newState) return;
        Sprite targetSprite = null;
        switch (newState)
        {
            case TimehopState.Morning: targetSprite = morningSprite; break;
            case TimehopState.Afternoon: targetSprite = afternoonSprite; break;
            case TimehopState.Evening: targetSprite = eveningSprite; break;
        }
        if (targetSprite != null)
        {
            currentState = newState;
            StartCoroutine(CrossfadeSky(targetSprite));
        }
        //Debug.Log($"Timehop state changed to: {newState}");
        //Debug.Log($"Current fading state: {isFading}");
    }

   IEnumerator CrossfadeSky(Sprite nextSprite)
    {
        isFading = true;
        nextSky.sprite = nextSprite;
        nextSky.color = new Color(1, 1, 1, 0);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeDuration;

            currentSky.color = new Color(1, 1, 1, 1 - alpha);
            nextSky.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        currentSky.sprite = nextSprite;
        currentSky.color = new Color(1, 1, 1, 1);
        nextSky.color = new Color(1, 1, 1, 0);
        isFading = false;
    }

}
