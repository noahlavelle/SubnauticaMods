using SMLHelper.V2.Handlers;
using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehiclePing : VehicleComponent
{
    public PingInstance Ping;

    private readonly string _pingOriginPath;
    private readonly Sprite _pingIcon;

    public VehiclePing(Sprite pingIcon, string pingOriginPath = "PingOrigin")
    {
        _pingIcon = pingIcon;
        _pingOriginPath = pingOriginPath;
    }
    
    public override void AddComponent(ModVehicle parentVehicle)
    {
        var pingType = PingHandler.RegisterNewPingType(parentVehicle.FriendlyName, _pingIcon);
        
        Ping = parentVehicle.Prefab.AddComponent<PingInstance>();
        Ping.origin = parentVehicle.Prefab.transform.Find(_pingOriginPath);
        Ping.pingType = pingType;
    }
}