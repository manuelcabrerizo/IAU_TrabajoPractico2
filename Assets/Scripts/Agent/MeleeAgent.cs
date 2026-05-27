using UnityEngine;

public class MeleeAgent : Agent
{
    [SerializeField] public LayerMask TargetLayerMask;
    protected override void OnAwaken()
    {
    }

    protected override void OnDestroyed()
    {
    }
}