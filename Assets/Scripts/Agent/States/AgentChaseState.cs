using UnityEngine;

public class AgentChaseState : StateMachineBehaviour
{
    [SerializeField] private float outOfReachRadius = 10.0f;
    [SerializeField] private float attackRadius = 1.0f;
    private float pathfindingInterval = 0.25f;
    private float pathfindingTimer = 0.0f;

    private Agent agent = null;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>();
        }
        agent.NavMeshAgent.updateRotation = true;
        pathfindingTimer = 0.0f;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.NavMeshAgent.velocity = Vector3.zero;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.Target != null)
        {
            pathfindingTimer += Time.deltaTime;
            if (pathfindingTimer >= pathfindingInterval)
            {

                agent.NavMeshAgent.SetDestination(agent.Target.position);
                pathfindingTimer -= pathfindingInterval;
            }
            if (IsOutOfRange())
            {
                animator.SetBool("IsPlayerInChaseArea", false);
            }
            else if (IsInAttackRange())
            {
                animator.SetBool("IsPlayerInAttackArea", true);
            }
        }
        else
        {
            animator.SetBool("IsPlayerInChaseArea", false);
        }
    }

    private bool IsOutOfRange()
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;

        Vector3 target = agent.Target.position;
        target.y = 0.0f;

        float sqrDistance = (target - position).sqrMagnitude;
        return sqrDistance > outOfReachRadius * outOfReachRadius;
    }

    private bool IsInAttackRange()
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;

        Vector3 target = agent.Target.position;
        target.y = 0.0f;

        float sqrDistance = (target - position).sqrMagnitude;
        return sqrDistance <= attackRadius * attackRadius;
    }
}
