using UnityEngine;

public class AgentDeathState : StateMachineBehaviour
{
    private Agent agent = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>();
        }
        agent.NavMeshAgent.isStopped = true;
    }
}
