using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralSpawner : MonoBehaviour
{
    public int amount = 20;

    public GameObject[] corals;

    public float scaleVaritation = 0.25f;

    public float spacingFactor = 6.0f;

    private void Start()
    {
        Environment.Instance.MassInstaniateEntities(corals, amount, transform, spacingFactor, scaleVaritation);
    }
}

