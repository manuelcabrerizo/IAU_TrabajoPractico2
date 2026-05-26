using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private Transform boidContainer;
    [SerializeField] private int boidCount = 100;
    [SerializeField] private float spawnRadio = 2.5f;

    private List<Boid> boids = new List<Boid>();

    private void Start()
    {
        for (int i = 0; i < boidCount; i++)
        {
            Vector3 spawnPosition = boidContainer.transform.position + (Random.insideUnitSphere * spawnRadio);
            GameObject go = Instantiate(boidPrefab, spawnPosition, Quaternion.identity, boidContainer);
            Boid boid = go.GetComponent<Boid>();
            boids.Add(boid);
        }
    }
}
