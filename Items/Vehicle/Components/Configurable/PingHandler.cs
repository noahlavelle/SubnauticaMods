namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class PingHandler : HandlerComponent
{
    private PingInstance _ping;
    private PingType _pingType;
    
    public override void Instantiate()
    {
        _ping = parentVehicle.Model.AddComponent<PingInstance>();
    }

    public PingHandler WithOrigin(Transform origin)
    {
        _ping.origin = origin;
        return this;
    }

    public PingHandler WithIcon(Sprite icon)
    {
        _pingType = EnumHandler.AddEntry<PingType>(parentVehicle.ClassID).WithIcon(icon);
        _ping.pingType = _pingType;
        return this;
    }

    public PingHandler WithName(string displayName)
    {
        LanguageHandler.SetLanguageLine($"Ping{parentVehicle.ClassID}", displayName);
        return this;
    }

    public PingHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle)
    { }
}
