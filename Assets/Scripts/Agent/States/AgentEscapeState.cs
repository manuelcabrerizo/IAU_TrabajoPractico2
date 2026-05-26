using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AgentEscapeState : StateMachineBehaviour
{
    [SerializeField] private float outOfReachRadius = 10.0f;

    private Agent agent = null;
    private Transform attackTarget = null;
    private float pathfindingInterval = 0.01f;
    private float pathfindingTimer = 0.0f;
    private Vector3 escapePosition = Vector3.zero;
    private float searchRadius = 10.0f;
    private Color searchColor = Color.blue;

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
        agent.NavMeshAgent.speed = 5.0f;
        SetEscapeDestination();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.NavMeshAgent.speed = 4.0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.DrawDebugSphere(agent.transform.position, outOfReachRadius, Color.yellow);
        agent.DrawDebugSphere(escapePosition, searchRadius, searchColor);


        pathfindingTimer += Time.deltaTime;
        if (pathfindingTimer >= pathfindingInterval)
        {
            SetEscapeDestination();
            pathfindingTimer -= pathfindingInterval;
        }

        if (IsOutOfRange())
        {
            animator.SetBool("IsPlayerIsEscapeArea", false);
        }
    }

    private bool IsOutOfRange()
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;
        Vector3 target = attackTarget.position;
        target.y = 0.0f;
        float sqrDistance = (target - position).sqrMagnitude;
        return sqrDistance > outOfReachRadius * outOfReachRadius;
    }

    private void SetEscapeDestination()
    {
        searchColor = Color.red;
        Vector3 rawEscapePoint = agent.transform.position + GetEscapeDirection() * 4.0f;
        if (NavMesh.SamplePosition(rawEscapePoint, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
        {
            searchColor = Color.blue;
            escapePosition = hit.position;
            agent.NavMeshAgent.SetDestination(hit.position);
        }
    }

    private Vector3 GetEscapeDirection()
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;
        Vector3 target = attackTarget.position;
        target.y = 0.0f;
        return (position - target).normalized;
    }
}
