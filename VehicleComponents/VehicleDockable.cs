using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleDockable : VehicleComponent
{
    public readonly string AnimationName;
    public readonly Vector3 DockingEndPoint;

    private ColorCustomizer _colorCustomizer;

    public VehicleDockable(string animationName, Vector3 dockingEndPoint)
    {
        AnimationName = animationName;
        DockingEndPoint = dockingEndPoint;
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        var dockable = parentVehicle.Prefab.AddComponent<Dockable>();
        dockable.rb = parentVehicle.Rigidbody;
        dockable.vehicle = parentVehicle.VehicleBehaviour;
        
        _colorCustomizer = parentVehicle.Prefab.AddComponent<ColorCustomizer>();
        _colorCustomizer.isBase = false;
    }
}