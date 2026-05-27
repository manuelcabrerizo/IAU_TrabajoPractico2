using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] public float Speed = 4;
    [SerializeField] public float SlowSpeed = 1.5f;
    [SerializeField] public float EscapeSpeed = 6.0f;
    [SerializeField] private LayerMask slowAreaLayerMask;
    [SerializeField] public LayerMask TargetLayerMask;

    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public Transform Target = null;

    private Health health = null;
    private Animator animator = null;
    private NavMeshAgent navMeshAgent = null;



    public Action OnAction;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();

        health.OnHealthChange += OnHealthChange;
        navMeshAgent.speed = Speed;
    }

    private void OnDestroy()
    {
        health.OnHealthChange -= OnHealthChange;
    }

    private void Update()
    {
        SearchTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & slowAreaLayerMask) != 0)
        {
            navMeshAgent.speed = SlowSpeed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & slowAreaLayerMask) != 0)
        {
            navMeshAgent.speed = SlowSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & slowAreaLayerMask) != 0)
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
}