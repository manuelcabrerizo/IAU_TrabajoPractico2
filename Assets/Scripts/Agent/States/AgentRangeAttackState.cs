using System.Collections;
using UnityEngine;

public class AgentRangeAttackState : StateMachineBehaviour
{
    [SerializeField] private float outOfReachRadius = 10.0f;
    [SerializeField] private float escapeRadius = 5.0f;

    private RangeAgent agent = null;
    private float rotationSpeed = 10.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>() as RangeAgent;
        }
        agent.NavMeshAgent.isStopped = true;
        agent.NavMeshAgent.updateRotation = false;
        agent.OnAction += Fire;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.OnAction -= Fire;
        agent.NavMeshAgent.isStopped = false;
        agent.NavMeshAgent.updateRotation = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;
        Vector3 target = agent.Target.position;
        target.y = 0.0f;
        Vector3 direction = (target - position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.z);

        Quaternion targetRotation = Quaternion.Euler(0.0f, Mathf.Rad2Deg * angle, 0.0f);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        if (IsOutOfRange())
        {
            animator.SetBool("IsPlayerInAttackArea", false);
        }
        if (IsPlayerInScapeRange())
        {
            animator.SetBool("IsPlayerIsEscapeArea", true);
        }
    }

    private void Fire()
    {
        agent.StartCoroutine(Shot(agent.BulletPool.Alloc(agent.BulletPool.transform).GetComponent<TrailRenderer>()));
    }

    private IEnumerator Shot(TrailRenderer trailRenderer)
    {
        Vector3 direction = agent.transform.forward;
        direction.y = 0.0f;
        direction.Normalize();
        Vector3 startPosition = agent.ShotTransform.position;
        if (Physics.Raycast(startPosition, direction, out RaycastHit hit, agent.ShotDistance, agent.TargetLayerMask))
        {
            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                int damage = agent.IsMad ? agent.MadDamage : agent.NormalDamage;
                damagable.TakeDamage(damage);
            }
        }
        Vector3 targetPosition = agent.ShotTransform.position + (direction * agent.ShotDistance);
        float t = 0.0f;
        while (t <= 1.0f)
        {
            trailRenderer.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            t += Time.deltaTime * agent.BulletAnimationSpeed;
            yield return new WaitForEndOfFrame();
        }
        trailRenderer.Clear();
        agent.BulletPool.Free(trailRenderer.gameObject);
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

    private bool IsPlayerInScapeRange()
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;
        Vector3 target = agent.Target.position;
        target.y = 0.0f;
        float sqrDistance = (target - position).sqrMagnitude;
        return sqrDistance <= escapeRadius * escapeRadius;
    }
}
