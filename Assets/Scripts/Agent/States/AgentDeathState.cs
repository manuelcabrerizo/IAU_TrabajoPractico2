using UnityEngine;

public class AgentDeathState : StateMachineBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private Agent agent = null;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>();
        }
        agent.NavMeshAgent.isStopped = true;
        agent.gameObject.GetComponent<Collider>().enabled = false;
        if (agent is RangeAgent || agent is MeleeAgent)
        {
            EventBus.Raise<AgentDeadStateEnteredEvent>();
        }
        else if (agent is NPCAgent)
        {
            EventBus.Raise<NpcAgenKillEvent>(agent.gameObject);
        }
    }
}
