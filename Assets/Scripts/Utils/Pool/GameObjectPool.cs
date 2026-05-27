using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private List<GameObject> useObjects = null;
    private Stack<GameObject> freeObjects = null;

    private void Awake()
    {
        useObjects = new List<GameObject>();
        freeObjects = new Stack<GameObject>();
    }

    public GameObject Alloc(Transform parent = null)
    {
        GameObject go = null;
        if (freeObjects.Count == 0)
        {
            go = Instantiate(prefab, parent);
            useObjects.Add(go);
        }
        else 
        {
            go = freeObjects.Pop();
            go.SetActive(true);
            useObjects.Add(go);
        }
        return go;
    }

    public void Free(GameObject go)
    {
        if (useObjects.Contains(go))
        {
            go.SetActive(false);
            freeObjects.Push(go);
            useObjects.Remove(go);
        }
    }
}
