using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamagable, IHealable
{
    [SerializeField] private Image lifeBar;
    [SerializeField] private int maxHealth = 100;
    public int CurrentHealth { get; private set; } = 0;

    public Action OnHealthChange;

    public bool IsAlive => CurrentHealth > 0;

    private void Start()
    {
        HealFull();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Math.Max(CurrentHealth - amount, 0);
        OnHealthChange?.Invoke();
        lifeBar.fillAmount = (float)CurrentHealth / maxHealth;
    }

    public void Heal(int amount)
    {
        CurrentHealth = Math.Min(CurrentHealth + amount, maxHealth);
        OnHealthChange?.Invoke();
        lifeBar.fillAmount = (float)CurrentHealth / maxHealth;
    }

    public void HealFull()
    {
        CurrentHealth = maxHealth;
        OnHealthChange?.Invoke();
        lifeBar.fillAmount = (float)CurrentHealth / maxHealth;
    }
}
