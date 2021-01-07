using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public BoidSettings boidSettings;
    public static BoidManager Instance;

    private int numberOfTargets = 100;

    private Vector3[] targetPositions;

    public List<Species> boidSpecies;

    public List<Fish> allBoids = new List<Fish>();

    private int minX = -60;
    private int maxX = 60;

    private int minY = -18;
    private int maxY = -1;

    private int minZ = -70;
    private int maxZ = 40;

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

        targetPositions = new Vector3[numberOfTargets];
    }

    private void Start()
    {

    }

    public Vector3 GetRandomTarget()
    {
        for(int i = 0; i < numberOfTargets; i++)
        {
            int randX = Random.Range(minX, maxX);
            int randY = Random.Range(minY, maxY);
            int randZ = Random.Range(minZ, maxZ);

            targetPositions[i] = new Vector3(randX, randY, randZ);
        }

        int randTarget = Random.Range(0, targetPositions.Length - 1);

        return targetPositions[randTarget];
        //return new Vector3(50, -15, 0);
    }
}
