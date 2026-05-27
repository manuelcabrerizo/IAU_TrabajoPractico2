using UnityEngine;

public class AgentDeathState : StateMachineBehaviour
{
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
        agent.NavMeshAgent.isStopped = true;
    }
}
