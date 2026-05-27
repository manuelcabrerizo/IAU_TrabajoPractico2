using UnityEngine;
using UnityEngine.UI;

public class UILookCamera : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private Vector3 offset;
    void LateUpdate()
    {
        ui.transform.position = transform.position + offset;
        ui.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
