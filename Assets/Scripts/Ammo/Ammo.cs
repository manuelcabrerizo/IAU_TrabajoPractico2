using System;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private int maxAmmo = 25;
    [SerializeField] private int maxMagazine = 5;
    public int CurrentAmmo { get; private set; } = 0;

    private int magazines = 0;

    private void Start()
    {
        magazines = maxMagazine;
        CurrentAmmo = maxAmmo;
        EventBus.Raise<OnAmmoChangeEvent>(magazines, CurrentAmmo);
    }

    public bool HasAmmo()
    {
        bool canFire = CurrentAmmo > 0;
        CurrentAmmo = Math.Max(CurrentAmmo - 1, 0);
        EventBus.Raise<OnAmmoChangeEvent>(magazines, CurrentAmmo);
        return canFire;
    }

    public void LoadMagazine()
    {
        magazines = Math.Min(magazines + 1, maxMagazine);
        EventBus.Raise<OnAmmoChangeEvent>(magazines, CurrentAmmo);
    }

    public void Reload()
    {
        if (magazines > 0)
        {
            CurrentAmmo = maxAmmo;
            magazines = Math.Max(magazines - 1, 0);
            EventBus.Raise<OnAmmoChangeEvent>(magazines, CurrentAmmo);
        }
    }
}
