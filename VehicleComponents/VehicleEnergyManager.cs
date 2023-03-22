using System.Collections.Generic;
using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleEnergyManager : VehicleComponent
{
    public EnergyInterface EnergyInterface;
    public EnergyMixin EnergyMixin;

    private GameObject _batterySlotParent;
    
    public VehicleEnergyManager(GameObject batterySlotParent = null)
    {
        _batterySlotParent = batterySlotParent;
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        if (!_batterySlotParent)
        {
            _batterySlotParent = parentVehicle.Prefab.transform.Find("BatterySlot").gameObject;
        }
        
        EnergyMixin = parentVehicle.Prefab.AddComponent<EnergyMixin>();
        EnergyInterface = parentVehicle.Prefab.AddComponent<EnergyInterface>();
        EnergyInterface.sources = new[] { EnergyMixin };
        EnergyMixin.defaultBattery = TechType.PowerCell;
        EnergyMixin.compatibleBatteries = new List<TechType> { TechType.PowerCell };
        EnergyMixin.storageRoot = _batterySlotParent.gameObject.AddComponent<ChildObjectIdentifier>();

        parentVehicle.VehicleBehaviour.energyInterface = EnergyInterface;
    }
}