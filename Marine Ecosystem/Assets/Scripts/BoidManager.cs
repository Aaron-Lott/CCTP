using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public BoidSettings boidSettings;
    public static BoidManager Instance;

    public List<Species> boidSpecies;

    public List<Fish> allBoids = new List<Fish>();


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }
}
