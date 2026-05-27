using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AgentManager : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private int maxAgents = 100;
    [SerializeField] private float meleeSpawnMinTime = 2.0f;
    [SerializeField] private float meleeSpawnMaxTime = 5.0f;
    [SerializeField] private float rangeSpawnMinTime = 4.0f;
    [SerializeField] private float rangeSpawnMaxTime = 10.0f;
    [SerializeField] private float fastMeleeSpawnMinTime = 7.0f;
    [SerializeField] private float fastMeleeSpawnMaxTime = 10.0f;
    [SerializeField] private float fastRangeSpawnMinTime = 10.0f;
    [SerializeField] private float fastRangeSpawnMaxTime = 15.0f;
    [SerializeField] private float npcSpawnMinTime = 20.0f;
    [SerializeField] private float npcSpawnMaxTime = 40.0f;
    [SerializeField] private float eliteSpawnTime = 50.0f;
    [SerializeField] private GameObjectPool meleePool;
    [SerializeField] private GameObjectPool rangePool;
    [SerializeField] private GameObjectPool fastMeleePool;
    [SerializeField] private GameObjectPool fastRangePool;
    [SerializeField] private GameObjectPool npcPool;
    [SerializeField] private GameObjectPool elitePool;
    [SerializeField] private GameObjectPool agentBulletPool;

    [SerializeField] private Transform[] spawnPoints;

    private TaskScheduler taskScheduler = null;
    private List<GameObject> spawnedAgents = null;
    private bool lastMadValue = true;

    private void Awake()
    {
        taskScheduler = new TaskScheduler();
        spawnedAgents = new List<GameObject>();
        EventBus.Subscribe<AgentKillEvent>(OnAgentKill);
        EventBus.Subscribe<OnLowHealthEvent>(OnLowHealth);
        EventBus.Subscribe<OnHightHealthEvent>(OnHightHealth);
        EventBus.Subscribe<NpcAgenKillEvent>(OnNpcAgentKill);
        taskScheduler.Schedule(OnSpawnMelee, Random.Range(meleeSpawnMinTime, meleeSpawnMaxTime));
        taskScheduler.Schedule(OnSpawnRange, Random.Range(rangeSpawnMinTime, rangeSpawnMaxTime));
        taskScheduler.Schedule(OnSpawFastMelee, Random.Range(fastMeleeSpawnMinTime, fastMeleeSpawnMaxTime));
        taskScheduler.Schedule(OnSpawnFastRange, Random.Range(fastRangeSpawnMinTime, fastRangeSpawnMaxTime));
        taskScheduler.Schedule(OnSpawnNPC, Random.Range(npcSpawnMinTime, npcSpawnMaxTime));
        taskScheduler.Schedule(OnSpawnElite, eliteSpawnTime);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<AgentKillEvent>(OnAgentKill);
        EventBus.Unsubscribe<OnLowHealthEvent>(OnLowHealth);
        EventBus.Unsubscribe<OnHightHealthEvent>(OnHightHealth);
        EventBus.Unsubscribe<NpcAgenKillEvent>(OnNpcAgentKill);
    }

    private void Update()
    {
        taskScheduler.Tick(Time.deltaTime);
    }

    private void OnHightHealth(in OnHightHealthEvent callback)
    {
        if (lastMadValue)
        {
            SetIsMad(false);
            lastMadValue = false;
        }
    }

    private void OnLowHealth(in OnLowHealthEvent callback)
    {
        if (!lastMadValue)
        {
            SetIsMad(true);
            lastMadValue = true;
        }
    }

    private void OnAgentKill(in AgentKillEvent agentKillEvent)
    {
        spawnedAgents.Remove(agentKillEvent.GameObject);
        meleePool.Free(agentKillEvent.GameObject);
        rangePool.Free(agentKillEvent.GameObject);
        fastMeleePool.Free(agentKillEvent.GameObject);
        fastRangePool.Free(agentKillEvent.GameObject);
        npcPool.Free(agentKillEvent.GameObject);
        elitePool.Free(agentKillEvent.GameObject);
    }

    private void OnNpcAgentKill(in NpcAgenKillEvent npcAgenKillEvent)
    {
        RemoveTargteFromAttakingAgents(npcAgenKillEvent.GameObject);
    }

    private void OnSpawnMelee()
    {
        taskScheduler.Schedule(OnSpawnMelee, Random.Range(meleeSpawnMinTime, meleeSpawnMaxTime));
        if (spawnedAgents.Count >= maxAgents)
        {
            return;
        }
        GameObject go = meleePool.Alloc(transform);
        go.GetComponent<Collider>().enabled = true;
        IHealable healable = go.GetComponent<IHealable>();
        healable.HealFull();
        MeleeAgent meleeAgent = go.GetComponent<MeleeAgent>();
        meleeAgent.IsMad = false;
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
        go.GetComponent<Collider>().enabled = true;
        IHealable healable = go.GetComponent<IHealable>();
        healable.HealFull();
        RangeAgent rangeAgent = go.GetComponent<RangeAgent>();
        rangeAgent.BulletPool = agentBulletPool;
        rangeAgent.IsMad = false;
        go.transform.position = GetRandomSpawnPoint();
        spawnedAgents.Add(go);
    }

    private void OnSpawFastMelee()
    {
        taskScheduler.Schedule(OnSpawFastMelee, Random.Range(fastMeleeSpawnMinTime, fastMeleeSpawnMaxTime));
        if (spawnedAgents.Count >= maxAgents)
        {
            return;
        }
        GameObject go = fastMeleePool.Alloc(transform);
        go.GetComponent<Collider>().enabled = true;
        IHealable healable = go.GetComponent<IHealable>();
        healable.HealFull();
        MeleeAgent meleeAgent = go.GetComponent<MeleeAgent>();
        meleeAgent.IsMad = false;
        go.transform.position = GetRandomSpawnPoint();
        spawnedAgents.Add(go);
    }

    private void OnSpawnFastRange()
    {
        taskScheduler.Schedule(OnSpawnFastRange, Random.Range(fastRangeSpawnMinTime, fastRangeSpawnMaxTime));
        if (spawnedAgents.Count >= maxAgents)
        {
            return;
        }
        GameObject go = fastRangePool.Alloc(transform);
        go.GetComponent<Collider>().enabled = true;
        IHealable healable = go.GetComponent<IHealable>();
        healable.HealFull();
        RangeAgent rangeAgent = go.GetComponent<RangeAgent>();
        rangeAgent.BulletPool = agentBulletPool;
        rangeAgent.IsMad = false;
        go.transform.position = GetRandomSpawnPoint();
        spawnedAgents.Add(go);
    }

    private void OnSpawnNPC()
    {
        taskScheduler.Schedule(OnSpawnNPC, Random.Range(npcSpawnMinTime, npcSpawnMaxTime));
        if (spawnedAgents.Count >= maxAgents)
        {
            return;
        }
        GameObject go = npcPool.Alloc(transform);
        go.GetComponent<Collider>().enabled = true;
        IHealable healable = go.GetComponent<IHealable>();
        healable.HealFull();
        go.transform.position = GetRandomSpawnPoint();
        spawnedAgents.Add(go);
    }

    private void OnSpawnElite()
    {
        taskScheduler.Schedule(OnSpawnElite, eliteSpawnTime);
        if (spawnedAgents.Count >= maxAgents)
        {
            return;
        }
        GameObject go = elitePool.Alloc(transform);
        go.GetComponent<Collider>().enabled = true;
        IHealable healable = go.GetComponent<IHealable>();
        healable.HealFull();
        EliteAgent eliteAgent = go.GetComponent<EliteAgent>();
        eliteAgent.IsMad = false;
        go.transform.position = GetRandomSpawnPoint();
        spawnedAgents.Add(go);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0)
            return Vector3.zero;
        return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }

    private void SetIsMad(bool value)
    {
        foreach (GameObject go in spawnedAgents)
        {
            if (go.TryGetComponent<Agent>(out Agent agent))
            {
                agent.IsMad = value;
            }
        }
    }

    private void RemoveTargteFromAttakingAgents(GameObject npc)
    {
        foreach (GameObject go in spawnedAgents)
        {
            if (go.TryGetComponent<Agent>(out Agent agent))
            {
                if (agent.Target == npc.transform)
                {
                    agent.Target = null;
                }
            }
        }
    }
}
