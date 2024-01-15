using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEvent : MonoBehaviour
{
    public Action<Collider2D> OnTriggerEnter;
    public Action<Collider2D> OnTriggerExit;

    public Action<Collision2D> OnCollisionEnter;
    public Action<Collision2D> OnCollisionExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit?.Invoke(collision);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter?.Invoke(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionExit?.Invoke(collision);
    }
}
