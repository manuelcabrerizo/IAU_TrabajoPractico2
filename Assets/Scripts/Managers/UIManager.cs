using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text powerUpText;

    private void Awake()
    {
        EventBus.Subscribe<OnScoreChangeEvent>(OnScoreChange);
        EventBus.Subscribe<OnPowerUpGrabEvent>(OnPowerGrab);
        EventBus.Subscribe<OnPowerUpEndEvent>(OnPowerUpEnd);
        EventBus.Subscribe<OnAmmoChangeEvent>(OnAmmoChange);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnScoreChangeEvent>(OnScoreChange);
        EventBus.Unsubscribe<OnPowerUpGrabEvent>(OnPowerGrab);
        EventBus.Unsubscribe<OnPowerUpEndEvent>(OnPowerUpEnd);
        EventBus.Unsubscribe<OnAmmoChangeEvent>(OnAmmoChange);
    }

    private void OnAmmoChange(in OnAmmoChangeEvent onAmmoChangeEvent)
    {
        ammoText.text = "Ammo: " + onAmmoChangeEvent.Ammo + " | " + onAmmoChangeEvent.Magazines;
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
