using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Consumer
{

    protected FishSettings fishSettings;

    public bool DoesSchool { get { return fishSettings.doesSchool; } }

    public float SchoolPerceptiveRange { get { return fishSettings.schoolPerceptiveRange; } }

    private BoidManager boidManager;

    protected override void Init()
    {
        base.Init();

        fishSettings = (FishSettings)consumerSettings;

        if(DoesSchool)
        {
            Environment.Instance.AddBoidManagerToFishContainters(this);
            boidManager = GetComponentInParent<BoidManager>();
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void EscapeFromPredator()
    {
        SchoolBehaviour();
    }

    protected override void RemoveFromLists()
    {
        boidManager.RemoveFishFromSchool(this);
    }

    protected override void Exploring()
    {
        if (DoesSchool)
        {
            if(Environment.Instance.IsInsideReef(transform))
            {
                if (Random.Range(0, 5) < 1)
                    SchoolBehaviour();
            }
            else
            {
                moveDirection = Environment.Instance.GetReefCenter() - transform.position;
            }
        }
        else
        {
            base.Exploring();
        }

    }

    protected void SchoolBehaviour()
    {
        moveDirection += (Cohesion() + Seperation() + Alignment());
        moveTarget = boidManager.GoalPosition - transform.position;
    }

    #region Boid Behaviours

    private Vector3 Cohesion()
    {
        int numNeighbours = 0;
        Vector3 center = Vector3.zero;

        foreach (Fish fish in boidManager.GetAllFishInSchool())
        {
            if(fish.Species == this.Species)
            {
                if (Vector3.Distance(transform.position, fish.transform.position) < SchoolPerceptiveRange)
                {
                    center += fish.transform.position;
                    numNeighbours++;
                }
            }
        }

        if (numNeighbours > 0)
        {
            center /= numNeighbours;

             return (center - transform.position).normalized * boidManager.CohesionFactor;
        }

        return Vector3.zero;
    }

    private Vector3 Seperation()
    {
        const float minDistance = 1f;
        Vector3 move = Vector3.zero;

        foreach (Fish fish in boidManager.GetAllFishInSchool())
        {
            if (Vector3.Distance(transform.position, fish.transform.position) < minDistance)
            {
                move += transform.position - fish.transform.position;
            }
        }

        return move.normalized * boidManager.SeperationFactor;
    }

    private Vector3 Alignment()
    {
        Vector3 avgVelocity = Vector3.zero;
        float numNeighbours = 0;

        foreach (Fish fish in boidManager.GetAllFishInSchool())
        {
            if (Vector3.Distance(transform.position, fish.transform.position) < SchoolPerceptiveRange)
            {
                avgVelocity += fish.moveDirection;
                numNeighbours++;
            }
        }

        if (numNeighbours > 0)
        {
            avgVelocity /= numNeighbours;

            return avgVelocity.normalized * boidManager.AlignmentFactor;
        }

        return Vector3.zero;
    }

    #endregion
}
