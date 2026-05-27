using UnityEngine;

public class GameManager : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private int score = 0;

    private void Awake()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());
        EventBus.Subscribe<AgentKillEvent>(OnAgentKill);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<AgentKillEvent>(OnAgentKill);
    }

    private void OnAgentKill(in AgentKillEvent agentKillEvent)
    {
        score++;
    }
}