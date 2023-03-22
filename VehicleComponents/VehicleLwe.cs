namespace VehicleFramework.VehicleComponents;

public class VehicleLwe : VehicleComponent
{
    public override void AddComponent(ModVehicle parentVehicle)
    {
        var lwe = parentVehicle.Prefab.AddComponent<LargeWorldEntity>();
        lwe.cellLevel = LargeWorldEntity.CellLevel.Global;
    }
}