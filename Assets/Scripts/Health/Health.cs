using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamagable, IHealable
{
    [SerializeField] private Image lifeBar;
    [SerializeField] private int maxHealth = 100;
    public int CurrentHealth { get; private set; } = 0;

    public bool IsAlive => CurrentHealth > 0;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        lifeBar.fillAmount = (float)CurrentHealth / maxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Math.Max(CurrentHealth - amount, 0);
        lifeBar.fillAmount = (float)CurrentHealth / maxHealth;
    }

    public void Heal(int amount)
    {
        CurrentHealth = Math.Min(CurrentHealth + amount, maxHealth);
        lifeBar.fillAmount = (float)CurrentHealth / maxHealth;
    }
}
