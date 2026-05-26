using System.Collections.Generic;
using UnityEngine;

public class FlockingComposer
{
    private List<FlockingBehaviour> behaviours;

    public FlockingComposer()
    {
        behaviours = new List<FlockingBehaviour>()
        {
             new AlignmentBehaviour(),
             new CohesionBehaviour(),
             new SeparationBehaviour()
        };
    }

    public Vector3 GetDirection(Boid boid, List<Boid> neighbors)
    {
        Vector3 direction = Vector3.zero;
        foreach (FlockingBehaviour behaviour in behaviours) 
        {
            direction += behaviour.Process(boid, neighbors);
        }
        direction /= behaviours.Count;
        return direction;
    }
}

