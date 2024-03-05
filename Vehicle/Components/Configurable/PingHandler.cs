namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class PingHandler : HandlerComponent
{
    [SerializeField] private Transform _origin;
    [SerializeField] private PingType _type;
    private PingInstance _ping;

    public void Awake()
    {
        Plugin.Logger.LogInfo(_origin);
        _ping = gameObject.AddComponent<PingInstance>();
        _ping.pingType = _type;
        _ping.origin = _origin;
    }

    public PingHandler WithOrigin(Transform origin)
    {
        _origin = origin;
        return this;
    }

    public PingHandler WithIcon(Sprite icon, string classID)
    {
        _type = EnumHandler.AddEntry<PingType>(classID).WithIcon(icon);
        return this;
    }

    public PingHandler WithName(string displayName, string classID)
    {
        LanguageHandler.SetLanguageLine($"Ping{classID}", displayName);
        return this;
    }
}
