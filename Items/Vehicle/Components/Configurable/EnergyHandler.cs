namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class EnergyHandler : HandlerComponent
{
    private EnergyMixin _energyMixin;
    private EnergyInterface _energyInterface;
    
    public override void Instantiate()
    {
        _energyMixin = parentVehicle.Model.AddComponent<EnergyMixin>();
        _energyMixin.defaultBattery = TechType.PowerCell;
        _energyMixin.compatibleBatteries = new List<TechType> {TechType.PowerCell};

        _energyInterface = parentVehicle.Model.AddComponent<EnergyInterface>();
        _energyInterface.sources = new[] {_energyMixin};

        parentVehicle.Behaviour.energyInterface = _energyInterface;
    }

    public EnergyHandler WithBatterySlot(Transform batterySlotParent)
    {
        _energyMixin.storageRoot = batterySlotParent.gameObject.AddComponent<ChildObjectIdentifier>();
        
        return this;
    }
    
    public EnergyHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}