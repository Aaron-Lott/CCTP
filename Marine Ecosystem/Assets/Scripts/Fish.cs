using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Consumer
{
    private List<Fish> otherFish = new List<Fish>();

    protected override void Init()
    {
        base.Init();
        PopulateFishList();
    }

    protected override void EscapeFromPredator()
    {
        Shoal();
        moveSpeed = fastMoveSpeed;
    }

    protected override void Socialising()
    {
        //Shoal();
    }

    protected void Shoal()
    {
        moveDirection += (Cohesion() + Seperation() + Alignment());
    }

    #region Boid Behaviours

    private Vector3 Cohesion()
    {
        int numNeighbours = 0;
        Vector3 center = Vector3.zero;

        foreach (Fish fish in otherFish)
        {
            if (Vector3.Distance(transform.position, fish.transform.position) < perceptiveRange)
            {
                center += fish.transform.position;
                numNeighbours++;
            }
        }

        if (numNeighbours > 0)
        {
            center /= numNeighbours;

             return (center - transform.position).normalized * BoidsBehaviour.Instance.cohesionFactor;
        }

        return Vector3.zero;
    }

    private Vector3 Seperation()
    {
        const float minDistance = 1f;
        Vector3 move = Vector3.zero;

        foreach (Fish fish in otherFish)
        {
            if (Vector3.Distance(transform.position, fish.transform.position) < minDistance)
            {
                move += transform.position - fish.transform.position;
            }
        }

        return move.normalized * BoidsBehaviour.Instance.seperationFactor;
    }

    private Vector3 Alignment()
    {
        Vector3 avgVelocity = Vector3.zero;
        float numNeighbours = 0;

        foreach (Fish fish in otherFish)
        {
            if (Vector3.Distance(transform.position, fish.transform.position) < perceptiveRange)
            {
                avgVelocity += fish.moveDirection;
                numNeighbours++;
            }
        }

        if (numNeighbours > 0)
        {
            avgVelocity /= numNeighbours;

            return avgVelocity.normalized * BoidsBehaviour.Instance.alignmentFactor;
        }

        return Vector3.zero;
    }

    private void PopulateFishList()
    {
        foreach (Fish fish in FindObjectsOfType<Fish>())
        {
            if (fish != this)
            {
                otherFish.Add(fish);
            }
        }
    }

    #endregion
}
