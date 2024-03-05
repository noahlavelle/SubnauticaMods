namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Base;

public class EcoTargetHandler : HandlerComponent
{
    public void Awake()
    {
        var ecoTarget = gameObject.AddComponent<EcoTarget>();
        ecoTarget.type = EcoTargetType.Shark;

        var creatureUtils = gameObject.AddComponent<CreatureUtils>();
        creatureUtils.setupEcoTarget = true;
        creatureUtils.setupEcoBehaviours = false;
        creatureUtils.addedComponents = new Component[] {ecoTarget};

        VehicleBehaviourHandler.ecoTarget = ecoTarget;
    }
}