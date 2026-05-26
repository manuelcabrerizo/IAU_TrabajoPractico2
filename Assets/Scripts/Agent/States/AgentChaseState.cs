using UnityEngine;

public class AgentChaseState : StateMachineBehaviour
{
    [SerializeField] private float searchRadius = 10.0f;
    private float pathfindingInterval = 0.5f;
    private float pathfindingTimer = 0.0f;

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
            attackTarget = FindAnyObjectByType<CharacterMovement>().transform;
        }
        agent.NavMeshAgent.SetDestination(attackTarget.position);
        pathfindingTimer = 0.0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pathfindingTimer += Time.deltaTime;
        if (pathfindingTimer >= pathfindingInterval)
        {
            agent.NavMeshAgent.SetDestination(attackTarget.position);
            pathfindingTimer -= pathfindingInterval;
        }

        if (IsOutOfRange())
        {
            animator.SetTrigger("PlayerOutOfReach");
        }
    }

    private bool IsOutOfRange()
    {
        float sqrDistance = (attackTarget.position - agent.transform.position).sqrMagnitude;
        return sqrDistance > searchRadius * searchRadius;
    }
}
