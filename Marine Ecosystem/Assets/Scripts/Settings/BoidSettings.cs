using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Boid Settings")]
public class BoidSettings : ScriptableObject
{
    [Range(0.0f, 1.0f)]
    public float cohesionFactor;

    [Range(0.0f, 1.0f)]
    public float seperationFactor;

    [Range(0.0f, 1.0f)]
    public float alignmentFactor;
}