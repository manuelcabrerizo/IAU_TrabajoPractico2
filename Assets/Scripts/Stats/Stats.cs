using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Stats : IService
{
    public bool IsPersistance => true;
    public int Score  = 0;
}
