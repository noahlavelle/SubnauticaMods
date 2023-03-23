using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleEcoTarget : VehicleComponent
{
    public CreatureUtils CreatureUtils;
    public EcoTarget EcoTarget;
    
    public override void AddComponent(ModVehicle parentVehicle)
    {
        EcoTarget = parentVehicle.Prefab.AddComponent<EcoTarget>();
        EcoTarget.type = EcoTargetType.Shark;

        CreatureUtils = parentVehicle.Prefab.AddComponent<CreatureUtils>();
        CreatureUtils.setupEcoTarget = true;
        CreatureUtils.setupEcoBehaviours = false;
        CreatureUtils.addedComponents = new Component[] { EcoTarget };

        parentVehicle.VehicleBehaviour.ecoTarget = EcoTarget;
    }
}