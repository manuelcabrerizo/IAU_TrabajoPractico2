using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    Stats Stats => ServiceProvider.Instance.GetService<Stats>();

    private int score = 0;

    private void Awake()
    {
        if (!ServiceProvider.Instance.ContainsService<Stats>())
        {
            ServiceProvider.Instance.AddService<Stats>(new Stats());
        }
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
        Stats.Score = score;
        SceneManager.LoadScene("GameOver");
    }

    private void OnAgentDeadStateEntered(in AgentDeadStateEnteredEvent agentDeadStateEnteredEvent)
    {
        score++;
        EventBus.Raise<OnScoreChangeEvent>(score);
    }
}