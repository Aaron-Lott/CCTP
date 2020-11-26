using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BoidsBehaviour : MonoBehaviour
{
    public float moveSpeed = 4;

    [Range(0.0f, 5.0f)]
    public float perceptiveRange = 2;

    [Range(0.0f, 1.0f)]
    public float cohesionFactor;

    [Range(0.0f, 1.0f)]
    public float seperationFactor;

    [Range(0.0f, 1.0f)]
    public float alignmentFactor;

    public static BoidsBehaviour Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}

