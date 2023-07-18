namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Base;

public class EcoTargetHandler : HandlerComponent
{
    public override void Instantiate()
    {
        var ecoTarget = parentVehicle.Model.AddComponent<EcoTarget>();
        ecoTarget.type = EcoTargetType.Shark;

        var creatureUtils = parentVehicle.Model.AddComponent<CreatureUtils>();
        creatureUtils.setupEcoTarget = true;
        creatureUtils.setupEcoBehaviours = false;
        creatureUtils.addedComponents = new Component[] {ecoTarget};

        parentVehicle.Behaviour.ecoTarget = ecoTarget;
    }
    
    public EcoTargetHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}