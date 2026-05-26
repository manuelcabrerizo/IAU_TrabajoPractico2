using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float shotDistance = 50.0f;
    [SerializeField] private float bulletAnimationSpeed = 1.0f;
    [SerializeField] private Transform shotTransform;
    [SerializeField] private TrailRenderer bulletTrailPrefab;

    private CharacterController characterController = null;
    private Animator animator = null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ProcessMovement();
        ProcessShot();
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
        characterController.Move(movement.normalized * speed * Time.deltaTime);

        Vector3 localVelocity = (transform.worldToLocalMatrix * characterController.velocity) / speed;
        animator.SetFloat("VelocityZ", localVelocity.z);
        animator.SetFloat("VelocityX", localVelocity.x);
    }

    private void ProcessShot()
    {
        Debug.Log("Right: " + shotTransform.right);
        Debug.Log("Up: " + shotTransform.up);
        Debug.Log("Forward: " + shotTransform.forward);

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(ShotAnimation(Instantiate(bulletTrailPrefab)));
        }
    }

    private IEnumerator ShotAnimation(TrailRenderer trailRenderer)
    {
        Vector3 direction = shotTransform.up;
        direction.y = 0.0f;
        direction.Normalize();

        Vector3 startPosition = shotTransform.position;
        Vector3 targetPosition = shotTransform.position + (direction * shotDistance);
        
        float t = 0.0f;
        while (t <= 1.0f)
        {
            trailRenderer.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            t += Time.deltaTime * bulletAnimationSpeed;
            yield return new WaitForEndOfFrame();
        }

        Destroy(trailRenderer.gameObject);
    }
}

