using System.Collections.Generic;
using UnityEngine;

public abstract class FlockingBehaviour
{
    public abstract Vector3 Process(Boid boid, List<Boid> neighbors);
}

public class AlignmentBehaviour : FlockingBehaviour
{
    public override Vector3 Process(Boid boid, List<Boid> neighbors)
    {
        Vector3 direction = Vector3.zero;
        foreach (Boid neighbor in neighbors)
        {
            direction += neighbor.transform.forward;
        }
        if (neighbors.Count > 0)
        {
            direction /= neighbors.Count;
            direction.Normalize();
        }
        return direction;
    }
}

public class CohesionBehaviour : FlockingBehaviour
{
    public override Vector3 Process(Boid boid, List<Boid> neighbors)
    {
        Vector3 averagePosition = Vector3.zero;
        foreach (Boid neighbor in neighbors) 
        {
            averagePosition += neighbor.transform.position;
        }
        Vector3 direction = Vector3.zero;
        if (neighbors.Count > 0)
        {
            averagePosition /= neighbors.Count;
            direction = (averagePosition - boid.transform.position).normalized;
        }
        return direction;
    }
}

public class SeparationBehaviour : FlockingBehaviour
{
    private float separationRadious = 0.5f;
    public override Vector3 Process(Boid boid, List<Boid> neighbors)
    {
        Vector3 averagePosition = Vector3.zero;
        int boidCount = 0;
        foreach (Boid neighbor in neighbors)
        {
            float sqrDistance = (boid.transform.position - neighbor.transform.position).sqrMagnitude;
            if (sqrDistance > separationRadious * separationRadious)
            {
                continue;
            }
            averagePosition += neighbor.transform.position;
            boidCount++;
        }
        Vector3 direction = Vector3.zero;
        if (boidCount > 0)
        {
            averagePosition /= (float)boidCount;
            direction = (averagePosition - boid.transform.position).normalized;
        }
        return direction;
    }
}