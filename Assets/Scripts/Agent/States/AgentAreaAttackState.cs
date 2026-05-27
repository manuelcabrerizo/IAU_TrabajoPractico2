using UnityEngine;
using UnityEngine.AI;

public class AgentAreaAttackState : StateMachineBehaviour
{
    [SerializeField] private float outOfReachRadius = 2.0f;
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float radio = 3.0f;

    private EliteAgent agent = null;
    private TaskScheduler taskScheduler = null;
    private float currentAngle = 0.0f;
    private float increment = (2.0f * Mathf.PI) / 12.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>() as EliteAgent;
            taskScheduler = new TaskScheduler();
        }
        agent.NavMeshAgent.isStopped = false;
        agent.NavMeshAgent.speed = speed;
        agent.NavMeshAgent.updateRotation = false;
        if (agent.Target != null)
        {
            SetNextDestination();
        }

        taskScheduler.Schedule(MakeDamage, 2.0f);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.NavMeshAgent.updateRotation = true;
        agent.NavMeshAgent.speed = agent.Speed;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        taskScheduler.Tick(Time.deltaTime);
        if (agent.Target != null)
        {
            if (agent.NavMeshAgent.remainingDistance <= agent.NavMeshAgent.stoppingDistance)
            {
                SetNextDestination();
            }
            if (IsOutOfRange())
            {
                animator.SetBool("IsPlayerInAttackArea", false);
            }
        }
        else
        {
            animator.SetBool("IsPlayerInAttackArea", false);
        }
    }

    private bool IsOutOfRange()
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;
        Vector3 target = agent.Target.position;
        target.y = 0.0f;
        float sqrDistance = (target - position).sqrMagnitude;
        return sqrDistance > outOfReachRadius * outOfReachRadius;
    }

    private void MakeDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(agent.transform.position, 1.5f, agent.TargetLayerMask);
        foreach (Collider collider in colliders)
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                int damage = agent.IsMad ? agent.MadChargeAttackDamage : agent.NormalChargeAttackDamage;
                damagable.TakeDamage(damage);
            }
        }
        taskScheduler.Schedule(MakeDamage, 2.0f);
    }

    private void SetNextDestination()
    {
        Vector3 position = agent.Target.position;
        position.y = 0.0f;
        Vector3 direction = new Vector3(Mathf.Cos(currentAngle), 0.0f, Mathf.Sin(currentAngle));
        Vector3 target = position + direction * radio;
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 3.0f, NavMesh.AllAreas))
        {
            agent.NavMeshAgent.SetDestination(hit.position);
        }
        else
        {
            agent.NavMeshAgent.SetDestination(agent.Target.position);
        }
        currentAngle += increment;
        if (currentAngle >= Mathf.PI * 2.0f)
        {
            currentAngle = 0.0f;
        }
    }
}
