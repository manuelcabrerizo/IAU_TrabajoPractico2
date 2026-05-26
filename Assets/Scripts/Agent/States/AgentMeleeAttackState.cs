using UnityEngine;
using UnityEngine.UIElements;

public class AgentMeleeAttackState : StateMachineBehaviour
{
    [SerializeField] private float outOfReachRadius = 2.0f;

    private Agent agent = null;
    private Transform attackTarget = null;
    private float rotationSpeed = 10.0f;

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
        agent.NavMeshAgent.isStopped = true;
        agent.NavMeshAgent.updateRotation = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.NavMeshAgent.isStopped = false;
        agent.NavMeshAgent.updateRotation = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.DrawDebugSphere(agent.transform.position, outOfReachRadius, Color.yellow);

        Vector3 position = agent.transform.position;
        position.y = 0.0f;
        Vector3 target = attackTarget.position;
        target.y = 0.0f;
        Vector3 direction = (target - position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z);

        Quaternion targetRotation = Quaternion.Euler(0.0f, Mathf.Rad2Deg * angle, 0.0f);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (IsOutOfRange())
        {
            animator.SetBool("IsPlayerInAttackArea", false);
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
}
