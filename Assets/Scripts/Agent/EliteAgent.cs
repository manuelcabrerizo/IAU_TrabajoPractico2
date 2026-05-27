using System;
using UnityEngine;

public class EliteAgent : EnemyAgent
{
    [SerializeField] public int NormalChargeAttackDamage = 16;
    [SerializeField] public int MadChargeAttackDamage = 24;
    [SerializeField] public int NormalAreaAttackDamage = 8;
    [SerializeField] public int MadAreaAttackDamage = 16;
    [SerializeField] public float chargeDuration = 20.0f;
    [SerializeField] public float areaDuration = 10.0f;

    private TaskScheduler taskScheduler = null;
    private int currentFase = 0;
    
    protected override void OnAwake()
    {
        taskScheduler = new TaskScheduler();
        taskScheduler.Schedule(OnChangeFase, chargeDuration);
    }

    protected override void OnUpdate()
    {
        taskScheduler.Tick(Time.deltaTime);
    }

    private void OnChangeFase()
    {
        currentFase = (currentFase + 1) % 2;
        animator.SetInteger("CurrentFase", currentFase);
        if (currentFase == 0)
        {
            taskScheduler.Schedule(OnChangeFase, chargeDuration);
        }
        else
        {
            taskScheduler.Schedule(OnChangeFase, areaDuration);
        }
    }
}