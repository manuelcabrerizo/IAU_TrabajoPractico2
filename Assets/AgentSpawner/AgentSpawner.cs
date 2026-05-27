using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

public class AgentSpawner : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private int maxAgents = 100;
    [SerializeField] private Transform spawnerTransform;
    [SerializeField] private float meleeSpawnMinTime = 2.0f;
    [SerializeField] private float meleeSpawnMaxTime = 5.0f;
    [SerializeField] private float rangeSpawnMinTime = 4.0f;
    [SerializeField] private float rangeSpawnMaxTime = 10.0f;
    [SerializeField] private float spawnRadio = 10.0f;
    [SerializeField] private GameObjectPool meleePool;
    [SerializeField] private GameObjectPool rangePool;

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
        if (spawnedAgents.Count >= maxAgents)
        {
            taskScheduler.Schedule(OnSpawnMelee, Random.Range(meleeSpawnMinTime, meleeSpawnMaxTime));
            return;
        }

        if (TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            GameObject go = meleePool.Alloc(transform);
            IHealable healable = go.GetComponent<IHealable>();
            healable.Heal(1000);
            go.transform.position = spawnPosition;
            spawnedAgents.Add(go);
            taskScheduler.Schedule(OnSpawnMelee, Random.Range(meleeSpawnMinTime, meleeSpawnMaxTime));
        }
        else 
        {
            taskScheduler.Schedule(OnSpawnMelee, Random.Range(meleeSpawnMinTime, 0.01f));
        }
    }

    private void OnSpawnRange()
    {
        if (spawnedAgents.Count >= maxAgents)
        {
            taskScheduler.Schedule(OnSpawnRange, Random.Range(rangeSpawnMinTime, rangeSpawnMaxTime));
            return;
        }

        if (TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            GameObject go = rangePool.Alloc(transform);
            IHealable healable = go.GetComponent<IHealable>();
            healable.Heal(1000);
            go.transform.position = spawnPosition;
            spawnedAgents.Add(go);
            taskScheduler.Schedule(OnSpawnRange, Random.Range(rangeSpawnMinTime, rangeSpawnMaxTime));
        }
        else
        {
            taskScheduler.Schedule(OnSpawnRange, Random.Range(rangeSpawnMinTime, 0.01f));
        }
    }

    private bool TryGetSpawnPosition(out Vector3 position)
    {
        position = Vector3.zero;
        float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
        Vector3 spawnPosition = spawnerTransform.position + direction * spawnRadio;
        float searchRadio = 2.0f;
        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, searchRadio, NavMesh.AllAreas))
        {
            position = hit.position;
            return true;
        }
        return false;
    }
}
