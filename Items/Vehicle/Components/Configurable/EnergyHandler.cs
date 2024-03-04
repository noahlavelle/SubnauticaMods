using UnityEngine.Serialization;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class EnergyHandler : HandlerComponent
{
    public EnergyMixin energyMixin;
    
    [SerializeField] private Transform _batterySlotParent;
    
    public void Awake()
    {
        var energyInterface = gameObject.AddComponent<EnergyInterface>();
        energyInterface.sources = new[] {energyMixin};

        VehicleBehaviourHandler.energyInterface = energyInterface;
    }

    public EnergyHandler WithEnergyConfig(TechType batteryType)
    {
        energyMixin = gameObject.AddComponent<EnergyMixin>();
        energyMixin.defaultBattery = batteryType;
        energyMixin.compatibleBatteries = new List<TechType> {batteryType};
        energyMixin.storageRoot = _batterySlotParent.gameObject.AddComponent<ChildObjectIdentifier>();
        
        return this;
    }

    public EnergyHandler WithBatterySlot(Transform batterySlotParent)
    {
        _batterySlotParent = batterySlotParent;
        
        return this;
    }
}