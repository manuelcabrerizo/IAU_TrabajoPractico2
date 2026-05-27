using UnityEngine;

public class SlowArea : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshRenderer.material.SetFloat("_UnscaleTime", Time.unscaledTime * (0.125f * 0.5f));
    }
}
