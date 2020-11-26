using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [HideInInspector]
    public Vector3 velocity;

    private List<Boid> otherBoids = new List<Boid>();

    private void Start()
    {
        PopulateBoidList();
    }

    private void Update()
    {
        Alignment();
        Cohesion();
        Seperation();

        transform.position += velocity.normalized * BoidsBehaviour.Instance.moveSpeed * Time.deltaTime;

        if(velocity != Vector3.zero)
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), 0.15f);
    }

    private void Cohesion()
    {
        int numNeighbours = 0;
        Vector3 center = Vector3.zero;

        foreach (Boid boid in otherBoids)
        {
            if (Vector3.Distance(transform.position, boid.transform.position) < BoidsBehaviour.Instance.perceptiveRange)
            {
                center += boid.transform.position;
                numNeighbours++;
            }
        }

        if (numNeighbours > 0)
        {
            center /= numNeighbours;

            velocity += (center.normalized - transform.position) * BoidsBehaviour.Instance.cohesionFactor;
        }
    }

    private void Seperation()
    {
        const float minDistance = 1f;
        Vector3 move = Vector3.zero;

        foreach (Boid boid in otherBoids)
        {
            if (Vector3.Distance(transform.position, boid.transform.position) < minDistance)
            {
                move += transform.position - boid.transform.position;
            }
        }

        velocity += move.normalized * BoidsBehaviour.Instance.seperationFactor;
    }

    private void Alignment()
    {
        Vector3 avgVelocity = Vector3.zero;
        float numNeighbours = 0;

        foreach (Boid boid in otherBoids)
        {
            if (Vector3.Distance(transform.position, boid.transform.position) < BoidsBehaviour.Instance.perceptiveRange)
            {
                avgVelocity += boid.velocity;
                numNeighbours++;
            }
        }

        if (numNeighbours > 0)
        {
            avgVelocity /= numNeighbours;

            velocity += avgVelocity.normalized * BoidsBehaviour.Instance.alignmentFactor;
        }
    }

    private void PopulateBoidList()
    {
        foreach (Boid boid in FindObjectsOfType<Boid>())
        {
            if (boid != this)
            {
                otherBoids.Add(boid);
            }
        }
    }
}
