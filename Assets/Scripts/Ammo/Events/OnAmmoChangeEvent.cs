public struct OnAmmoChangeEvent : IEvent
{
    public int Magazines;
    public int Ammo;

    public void Assign(params object[] parameters)
    {
        Magazines = (int)parameters[0];
        Ammo = (int)parameters[1];
    }
    public void Reset()
    {
        Magazines = 0;
    }
}
