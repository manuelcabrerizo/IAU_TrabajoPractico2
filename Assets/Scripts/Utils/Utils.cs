using UnityEngine;

public static class Utils
{
    public static bool TestLayer(GameObject go, LayerMask layer)
    {
        return ((1 << go.layer) & layer) != 0;
    }
}
