using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderUtility : MonoBehaviour
{
    public int TouchCount { get; private set; } = 0;

    public Action<Collider2D> OnTriggerEnter;
    public Action<Collider2D> OnTriggerExit;

    public Action<Collision2D> OnCollisionEnter;
    public Action<Collision2D> OnCollisionExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision);
        TouchCount++;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit?.Invoke(collision); 
        TouchCount--;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter?.Invoke(collision);
        TouchCount++;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionExit?.Invoke(collision);
        TouchCount--;
    }
}