using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private int attackPower = 10;
    [SerializeField] private float powerUpDuration = 10.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float slowSpeed = 2.5f;
    [SerializeField] private float shotDistance = 50.0f;
    [SerializeField] private float bulletAnimationSpeed = 1.0f;
    [SerializeField] private Transform shotTransform;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private LayerMask slowAreaLayerMask;
    [SerializeField] private LayerMask npcLayerMask;
    [SerializeField] private GameObjectPool bulletPool;

    private CharacterController characterController = null;
    private Animator animator = null;
    private TaskScheduler taskScheduler = null;
    private int currentAttackPower = 0;
    private float currentSpeed = 0.0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        taskScheduler = new TaskScheduler();
        currentSpeed = speed;
        currentAttackPower = attackPower;
    }

    private void Update()
    {
        taskScheduler.Tick(Time.deltaTime);
        ProcessMovement();
        ProcessShot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.TestLayer(other.gameObject, slowAreaLayerMask))
        {
            currentSpeed = slowSpeed;
        }
        if (Utils.TestLayer(other.gameObject, npcLayerMask))
        {
            EventBus.Raise<AgentKillEvent>(other.gameObject);
            EventBus.Raise<OnPowerUpGrabEvent>();
            currentAttackPower = 1000;
            taskScheduler.Schedule(OnPowerUpEnd, powerUpDuration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Utils.TestLayer(other.gameObject, slowAreaLayerMask))
        {
            currentSpeed = speed;
        }
    }

    private void ProcessMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizaontal = Input.GetAxis("Horizontal");

        Plane worldPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        worldPlane.Raycast(ray, out float distance);

        Vector3 worldPosition = ray.GetPoint(distance);
        worldPosition.y = 0.0f;

        Vector3 playerPosition = transform.position;
        playerPosition.y = 0.0f;

        Vector3 direction = (worldPosition - playerPosition).normalized;
        
        float angle = Mathf.Atan2(direction.x, direction.z);
        transform.rotation = Quaternion.Euler(0.0f, Mathf.Rad2Deg * angle, 0.0f);

        Vector3 movement = new Vector3(horizaontal, 0.0f, vertical);
        characterController.Move(movement.normalized * currentSpeed * Time.deltaTime);

        Vector3 localVelocity = (transform.worldToLocalMatrix * characterController.velocity) / currentSpeed;
        animator.SetFloat("VelocityZ", localVelocity.z);
        animator.SetFloat("VelocityX", localVelocity.x);
    }

    private void ProcessShot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shot(bulletPool.Alloc(bulletPool.transform).GetComponent<TrailRenderer>()));
        }
    }

    private void OnPowerUpEnd()
    {
        EventBus.Raise<OnPowerUpEndEvent>();
        currentAttackPower = attackPower;
    }

    private IEnumerator Shot(TrailRenderer trailRenderer)
    {
        Vector3 direction = transform.forward;
        direction.y = 0.0f;
        direction.Normalize();
        Vector3 startPosition = shotTransform.position;
        if (Physics.Raycast(startPosition, direction, out RaycastHit hit, shotDistance, enemyLayerMask))
        {
            IDamagable damagable = hit.collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(currentAttackPower);
            }
        }
        Vector3 targetPosition = shotTransform.position + (direction * shotDistance);
        float t = 0.0f;
        while (t <= 1.0f)
        {
            trailRenderer.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            t += Time.deltaTime * bulletAnimationSpeed;
            yield return new WaitForEndOfFrame();
        }
        trailRenderer.Clear();
        bulletPool.Free(trailRenderer.gameObject);
    }
}