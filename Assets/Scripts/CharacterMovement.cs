using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;

    private CharacterController characterController = null;
    private Animator animator = null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
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
        Debug.DrawLine(playerPosition, playerPosition + direction * 2.0f, Color.red);

        float angle = Mathf.Atan2(direction.x, direction.z);
        transform.rotation = Quaternion.Euler(0.0f, Mathf.Rad2Deg * angle, 0.0f);

        Vector3 movement = new Vector3(horizaontal, 0.0f, vertical);
        characterController.Move(movement.normalized * speed * Time.deltaTime);

        Vector3 localVelocity = (transform.worldToLocalMatrix * characterController.velocity) / speed;
        animator.SetFloat("VelocityZ", localVelocity.z);
        animator.SetFloat("VelocityX", localVelocity.x);
    }
}

