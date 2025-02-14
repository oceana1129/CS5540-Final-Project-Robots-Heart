using System;
using UnityEngine;

public class PlanetaryDimensions : MonoBehaviour
{
    public float scale;                 // scale of the object
    public float x;                     // x position of the object

    void Start()
    {
        gameObject.transform.localScale = new Vector3(scale, scale, scale);     // set object size based on scale
        gameObject.transform.position = new Vector3(x, 0, 0);                   // change position based on x value
    }
}
