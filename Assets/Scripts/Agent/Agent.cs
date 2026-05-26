using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private NavMeshAgent navMeshAgent = null;
    public NavMeshAgent NavMeshAgent => navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // TODO: delete
    public void DrawDebugSphere(Vector3 center, float radius, Color color)
    {
        // Dibuja 3 círculos en los ejes X, Y y Z para simular la esfera
        int segments = 12;
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float a1 = Mathf.Deg2Rad * (i * angleStep);
            float a2 = Mathf.Deg2Rad * ((i + 1) * angleStep);

            // Círculo horizontal (XZ)
            Vector3 xz1 = center + new Vector3(Mathf.Cos(a1), 0, Mathf.Sin(a1)) * radius;
            Vector3 xz2 = center + new Vector3(Mathf.Cos(a2), 0, Mathf.Sin(a2)) * radius;
            Debug.DrawLine(xz1, xz2, color);

            // Círculo vertical (XY)
            Vector3 xy1 = center + new Vector3(Mathf.Cos(a1), Mathf.Sin(a1), 0) * radius;
            Vector3 xy2 = center + new Vector3(Mathf.Cos(a2), Mathf.Sin(a2), 0) * radius;
            Debug.DrawLine(xy1, xy2, color);

            // Círculo vertical (YZ)
            Vector3 yz1 = center + new Vector3(0, Mathf.Cos(a1), Mathf.Sin(a1)) * radius; // Corrección visual rápida:
            Vector3 yz1_fixed = center + new Vector3(0, Mathf.Sin(a1), Mathf.Cos(a1)) * radius;
            Vector3 yz2_fixed = center + new Vector3(0, Mathf.Sin(a2), Mathf.Cos(a2)) * radius;
            Debug.DrawLine(yz1_fixed, yz2_fixed, color);
        }
    }
}
