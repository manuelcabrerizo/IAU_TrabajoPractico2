using UnityEngine;

public class Boid : MonoBehaviour
{
    private void Move(Vector3 force)
    { 
        Vector3 currentPosition = transform.position;
        currentPosition += force * Time.deltaTime;
        transform.rotation.SetLookRotation(force.normalized);
    }
}
