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
    [SerializeField] private SpriteRenderer sky;
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite afternoonSprite;
    [SerializeField] private Sprite eveningSprite;
    
   

    // Update is called once per frame
    void Update()
    {
       ChangeTime (currentState);
    }

    public void ChangeTime  ( TimehopState newState)
    {
        switch (newState)
        {
            case TimehopState.Morning:
                sky.sprite = morningSprite;
                break;
            case TimehopState.Afternoon:
                sky.sprite = afternoonSprite;
                break;
            case TimehopState.Evening:
                sky.sprite = eveningSprite;
                break;
        }
    }
}
