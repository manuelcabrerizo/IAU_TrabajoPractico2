using UnityEngine;

public class AmmoPickup : MonoBehaviour, IPickable
{
    EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();


    [SerializeField] private float timeToSpawn = 30.0f;
    private Collider coll = null;
    private MeshRenderer meshRenderer = null;
    private TaskScheduler taskScheduler = null;

    private void Awake()
    {
        coll = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        taskScheduler = new TaskScheduler();
    }

    private void Update()
    {
        taskScheduler.Tick(Time.deltaTime);
    }

    public void PickUp()
    {
        coll.enabled = false;
        meshRenderer.enabled = false;
        taskScheduler.Schedule(OnReset, timeToSpawn);
        EventBus.Raise<OnAmmoPickUpEvent>();
    }

    public void OnReset()
    {
        coll.enabled = true;
        meshRenderer.enabled = true;
    }
}
