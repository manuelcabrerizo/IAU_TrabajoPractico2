using UnityEngine;

public class GameManager : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private int score = 0;

    private void Awake()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());
        EventBus.Subscribe<OnPlayerDieEvent>(OnPlayerDie);
        EventBus.Subscribe<AgentDeadStateEnteredEvent>(OnAgentDeadStateEntered);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnPlayerDieEvent>(OnPlayerDie);
        EventBus.Unsubscribe<AgentDeadStateEnteredEvent>(OnAgentDeadStateEntered);
    }

    private void OnPlayerDie(in OnPlayerDieEvent onPlayerDieEvent)
    {
        // TODO: GameOver screen
    }

    private void OnAgentDeadStateEntered(in AgentDeadStateEnteredEvent agentDeadStateEnteredEvent)
    {
        score++;
        EventBus.Raise<OnScoreChangeEvent>(score);
    }
}