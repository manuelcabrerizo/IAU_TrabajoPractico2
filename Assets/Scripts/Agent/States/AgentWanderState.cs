using UnityEngine;

public class AgentWanderState : StateMachineBehaviour
{
    [SerializeField] private float searchRadius = 10.0f;

    private Agent agent = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>();
        }
        agent.NavMeshAgent.updateRotation = true;
        agent.NavMeshAgent.SetDestination(GetNextDestination());
        agent.Target = null;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.NavMeshAgent.remainingDistance <= agent.NavMeshAgent.stoppingDistance)
        {
            agent.NavMeshAgent.SetDestination(GetNextDestination());
        }
        if (agent.Target != null && IsInChaseRange())
        {
            animator.SetBool("IsPlayerInChaseArea", true);
        }
    }

    private bool IsInChaseRange()
    {
        float sqrDistance = (agent.Target.position - agent.transform.position).sqrMagnitude;
        return sqrDistance <= searchRadius * searchRadius;
    }

    private Vector3 GetNextDestination()
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
        return agent.transform.position + direction * 10.0f;
    }
}
