using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text powerUpText;

    private void Start()
    {
        EventBus.Subscribe<OnScoreChangeEvent>(OnScoreChange);
        EventBus.Subscribe<OnPowerUpGrabEvent>(OnPowerGrab);
        EventBus.Subscribe<OnPowerUpEndEvent>(OnPowerUpEnd);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnScoreChangeEvent>(OnScoreChange);
        EventBus.Unsubscribe<OnPowerUpGrabEvent>(OnPowerGrab);
        EventBus.Unsubscribe<OnPowerUpEndEvent>(OnPowerUpEnd);
    }

    private void OnPowerUpEnd(in OnPowerUpEndEvent onPowerUpEndEvent)
    {
        powerUpText.text = "Instakill: Disbale";
    }

    private void OnPowerGrab(in OnPowerUpGrabEvent onPowerUpGrabEvent)
    {
        powerUpText.text = "Instakill: Enable";
    }

    private void OnScoreChange(in OnScoreChangeEvent onScoreChangeEvent)
    {
        scoreText.text = "Score: " + onScoreChangeEvent.Score;
    }
}
