using UnityEngine;

public class AgentWanderState : StateMachineBehaviour
{
    [SerializeField] private float searchRadius = 10.0f;

    private Agent agent = null;
    private Transform attackTarget = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>();
        }
        if (!attackTarget)
        {
            attackTarget = FindAnyObjectByType<PlayerController>().transform;
        }
        agent.NavMeshAgent.SetDestination(GetNextDestination());
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.DrawDebugSphere(agent.transform.position, searchRadius, Color.green);

        if (agent.NavMeshAgent.remainingDistance <= agent.NavMeshAgent.stoppingDistance)
        {
            agent.NavMeshAgent.SetDestination(GetNextDestination());
        }
        if (IsInChaseRange())
        {
            animator.SetBool("IsPlayerInChaseArea", true);
        }
    }

    private bool IsInChaseRange()
    {
        float sqrDistance = (attackTarget.position - agent.transform.position).sqrMagnitude;
        return sqrDistance <= searchRadius * searchRadius;
    }

    private Vector3 GetNextDestination()
    {
        return Vector3.zero;
    }
}
