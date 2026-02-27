using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHop : MonoBehaviour
{
    [SerializeField] private TimehopState state;
    private TimehopManager thManager;

    private void Awake()
    {
        thManager = FindAnyObjectByType<TimehopManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            thManager.currentState = state;
        }
    }
}
