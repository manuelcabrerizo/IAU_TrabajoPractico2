using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private int maxAgents = 100;
    [SerializeField] private float meleeSpawnMinTime = 2.0f;
    [SerializeField] private float meleeSpawnMaxTime = 5.0f;
    [SerializeField] private float rangeSpawnMinTime = 4.0f;
    [SerializeField] private float rangeSpawnMaxTime = 10.0f;
    [SerializeField] private GameObjectPool meleePool;
    [SerializeField] private GameObjectPool rangePool;
    [SerializeField] private Transform[] spawnPoints;

    private TaskScheduler taskScheduler = null;
    private List<GameObject> spawnedAgents = null;

    private void Awake()
    {
        taskScheduler = new TaskScheduler();
        spawnedAgents = new List<GameObject>();
    }

    private void Start()
    {
        EventBus.Subscribe<AgentKillEvent>(OnAgentKill);
        taskScheduler.Schedule(OnSpawnMelee, Random.Range(meleeSpawnMinTime, meleeSpawnMaxTime));
        taskScheduler.Schedule(OnSpawnRange, Random.Range(rangeSpawnMinTime, rangeSpawnMaxTime));
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<AgentKillEvent>(OnAgentKill);
    }

    private void Update()
    {
        taskScheduler.Tick(Time.deltaTime);
    }

    private void OnAgentKill(in AgentKillEvent agentKillEvent)
    {
        spawnedAgents.Remove(agentKillEvent.GameObject);
        meleePool.Free(agentKillEvent.GameObject);
        rangePool.Free(agentKillEvent.GameObject);
    }

    private void OnSpawnMelee()
    {
        taskScheduler.Schedule(OnSpawnMelee, Random.Range(meleeSpawnMinTime, meleeSpawnMaxTime));
        if (spawnedAgents.Count >= maxAgents)
        {
            return;
        }
        GameObject go = meleePool.Alloc(transform);
        IHealable healable = go.GetComponent<IHealable>();
        healable.Heal(1000);
        go.transform.position = GetRandomSpawnPoint();
        spawnedAgents.Add(go);
    }

    private void OnSpawnRange()
    {
        taskScheduler.Schedule(OnSpawnRange, Random.Range(rangeSpawnMinTime, rangeSpawnMaxTime));
        if (spawnedAgents.Count >= maxAgents)
        {
            return;
        }
        GameObject go = rangePool.Alloc(transform);
        IHealable healable = go.GetComponent<IHealable>();
        healable.Heal(1000);
        go.transform.position = GetRandomSpawnPoint();
        spawnedAgents.Add(go);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        if(spawnPoints.Length == 0)
            return Vector3.zero;
        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }
}
