using System;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] public float Speed = 4;
    [SerializeField] public float SlowSpeed = 1.5f;
    [SerializeField] public float EscapeSpeed = 6.0f;
    [SerializeField] protected LayerMask slowAreaLayerMask;
    [SerializeField] public LayerMask TargetLayerMask;

    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public Transform Target = null;
    public bool IsMad { get; set; } = false;


    protected Health health = null;
    protected Animator animator = null;
    protected NavMeshAgent navMeshAgent = null;

    public Action OnAction;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();

        health.OnHealthChange += OnHealthChange;
        navMeshAgent.speed = Speed;

        OnAwake();
    }


    private void OnDestroy()
    {
        health.OnHealthChange -= OnHealthChange;
    }

    private void Update()
    {
        SearchTarget();
        OnUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.TestLayer(other.gameObject, slowAreaLayerMask))
        {
            navMeshAgent.speed = SlowSpeed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Utils.TestLayer(other.gameObject, slowAreaLayerMask))
        {
            navMeshAgent.speed = SlowSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Utils.TestLayer(other.gameObject, slowAreaLayerMask))
        {
            navMeshAgent.speed = Speed;
        }
    }

    private void OnHealthChange()
    {
        animator.SetBool("IsAlive", health.IsAlive);
    }

    public void OnDeathAnimationEnd()
    {
        EventBus.Raise<AgentKillEvent>(gameObject);
    }

    public void OnMakeAction()
    {
        OnAction?.Invoke();
    }

    public void SearchTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15.0f, TargetLayerMask);
        foreach (Collider collider in colliders)
        {
            Target = collider.transform;
            break;
        }
    }

    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
}