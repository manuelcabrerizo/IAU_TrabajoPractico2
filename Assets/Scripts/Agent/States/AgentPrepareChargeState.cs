using UnityEngine;

public class AgentPrepareChargeState : StateMachineBehaviour
{
    [SerializeField] private float outOfReachRadius = 2.0f;

    private Agent agent = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>();
        }
        agent.NavMeshAgent.isStopped = false;
        agent.NavMeshAgent.speed = agent.Speed;
        animator.ResetTrigger("GoToPrepareCharge");
        PrepareCharge();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.Target != null)
        {
            if (agent.NavMeshAgent.remainingDistance <= agent.NavMeshAgent.stoppingDistance)
            {
                agent.NavMeshAgent.isStopped = true;
                animator.SetTrigger("GoToChargeAttack");
            }
            if (IsOutOfRange())
            {
                animator.SetBool("IsPlayerInAttackArea", false);
            }
        }
        else
        {
            animator.SetBool("IsPlayerInAttackArea", false);
        }
    }

    private void PrepareCharge()
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;
        Vector3 target = agent.Target.position;
        target.y = 0.0f;
        Vector3 direction = (position - target).normalized;
        agent.NavMeshAgent.SetDestination(agent.Target.position + direction * 5.0f);
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
}