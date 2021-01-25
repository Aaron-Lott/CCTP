using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Consumer
{
    protected override void Init()
    {
        base.Init();
        BoidManager.Instance.allBoids.Add(this);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void EscapeFromPredator()
    {
        Shoal();
    }

    protected override void Exploring()
    {
        base.Exploring();
        //Shoal();
    }

    protected void Shoal()
    {
        moveDirection += (Cohesion() + Seperation() + Alignment());
    }

    protected override void RemoveFromLists()
    {
        BoidManager.Instance.allBoids.Remove(this);
    }

    #region Boid Behaviours

    private Vector3 Cohesion()
    {
        int numNeighbours = 0;
        Vector3 center = Vector3.zero;

        foreach (Fish fish in BoidManager.Instance.allBoids)
        {
            if(fish.Species == this.Species)
            {
                if (Vector3.Distance(transform.position, fish.transform.position) < PerceptiveRange)
                {
                    center += fish.transform.position;
                    numNeighbours++;
                }
            }
        }

        if (numNeighbours > 0)
        {
            center /= numNeighbours;

             return (center - transform.position).normalized * BoidManager.Instance.boidSettings.cohesionFactor;
        }

        return Vector3.zero;
    }

    private Vector3 Seperation()
    {
        const float minDistance = 1f;
        Vector3 move = Vector3.zero;

        foreach (Fish fish in BoidManager.Instance.allBoids)
        {
            if (fish.Species == this.Species)
            {
                if (Vector3.Distance(transform.position, fish.transform.position) < minDistance)
                {
                    move += transform.position - fish.transform.position;
                }
            }
        }

        return move.normalized * BoidManager.Instance.boidSettings.seperationFactor;
    }

    private Vector3 Alignment()
    {
        Vector3 avgVelocity = Vector3.zero;
        float numNeighbours = 0;

        foreach (Fish fish in BoidManager.Instance.allBoids)
        {
            if (fish.Species == this.Species)
            {
                if (Vector3.Distance(transform.position, fish.transform.position) < PerceptiveRange)
                {
                    avgVelocity += fish.moveDirection;
                    numNeighbours++;
                }
            }
        }

        if (numNeighbours > 0)
        {
            avgVelocity /= numNeighbours;

            return avgVelocity.normalized * BoidManager.Instance.boidSettings.alignmentFactor;
        }

        return Vector3.zero;
    }

    #endregion
}
