using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    public Transform[] PatrolPoints => patrolPoints;

    private NavMeshAgent navMeshAgent = null;
    public NavMeshAgent NavMeshAgent => navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
}
