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

    [Header("Sprites")]
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite afternoonSprite;
    [SerializeField] private Sprite eveningSprite;

    [Header("Renderers")]
    [SerializeField] private SpriteRenderer currentSky; // ตัวปัจจุบัน
    [SerializeField] private SpriteRenderer nextSky;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 1.0f; // ระยะเวลาการ Fade


    // Update is called once per frame
    void Update()
    {
       ChangeTime (currentState);
    }

    public void ChangeTime  ( TimehopState newState)
    {
        Sprite targetSprite = null;
        switch (newState)
        {
            case TimehopState.Morning: targetSprite = morningSprite; break;
            case TimehopState.Afternoon: targetSprite = afternoonSprite; break;
            case TimehopState.Evening: targetSprite = eveningSprite; break;
        }
        if (targetSprite != null)
        {
            StartCoroutine(CrossfadeSky(targetSprite));
        }

   IEnumerator CrossfadeSky(Sprite nextSprite)
    {
        // 1. ตั้งค่าภาพใหม่เตรียมไว้ (แต่ยังให้ล่องหนอยู่)
        nextSky.sprite = nextSprite;
        nextSky.color = new Color(1, 1, 1, 0);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeDuration;

            // 2. ค่อยๆ จางตัวเก่าออก และจางตัวใหม่เข้า
            currentSky.color = new Color(1, 1, 1, 1 - alpha);
            nextSky.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        // 3. เมื่อจบการ Fade ให้สลับตัวแปรกัน เพื่อเตรียมสำหรับการ Fade ครั้งต่อไป
        currentSky.sprite = nextSprite;
        currentSky.color = new Color(1, 1, 1, 1);
        nextSky.color = new Color(1, 1, 1, 0);
    }

}
}
