using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sizechange : MonoBehaviour
{
    public float MinSize = 0.5f;
    public float MaxSize = 1;
    public float Speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = (MinSize + Mathf.Abs(Mathf.Sin(Time.time * Speed * Mathf.PI)) * (MaxSize - MinSize)) * Vector3.one;
    }
}
