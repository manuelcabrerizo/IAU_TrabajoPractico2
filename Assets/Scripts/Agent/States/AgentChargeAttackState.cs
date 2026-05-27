using System;
using UnityEngine;

public class AgentChargeAttackState : StateMachineBehaviour
{
    [SerializeField] private float outOfReachRadius = 2.0f;
    [SerializeField] float chargeSpeed = 20.0f;
    private EliteAgent agent = null;
    private bool damageDonde = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!agent)
        {
            agent = animator.GetComponent<Agent>() as EliteAgent;
        }
        agent.NavMeshAgent.isStopped = false;
        agent.NavMeshAgent.speed = chargeSpeed;
        damageDonde = false;
        animator.ResetTrigger("GoToChargeAttack");
        Charge();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.Target != null)
        {
            if (agent.NavMeshAgent.remainingDistance <= agent.NavMeshAgent.stoppingDistance)
            {
                if (!damageDonde)
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
                    damageDonde = true;
                }
                agent.NavMeshAgent.isStopped = true;
                animator.SetTrigger("GoToPrepareCharge");
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

    private void Charge()
    {
        Vector3 position = agent.transform.position;
        position.y = 0.0f;
        Vector3 target = agent.Target.position;
        target.y = 0.0f;
        Vector3 direction = (target - position).normalized;
        agent.NavMeshAgent.SetDestination(agent.Target.position);
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
}
