using UnityEngine;

public class RangeAgent : Agent
{
    [SerializeField] public GameObjectPool BulletPool;
    [SerializeField] public float ShotDistance = 50.0f;
    [SerializeField] public float BulletAnimationSpeed = 1.0f;
    [SerializeField] public Transform ShotTransform;
}
