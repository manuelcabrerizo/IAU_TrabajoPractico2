using UnityEngine;

public struct NpcAgenKillEvent : IEvent
{
    public GameObject GameObject;
    public void Assign(params object[] parameters)
    {
        GameObject = (GameObject)parameters[0];
    }
    public void Reset()
    {
        GameObject = null;
    }
}

