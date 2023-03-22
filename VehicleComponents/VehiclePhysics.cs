using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehiclePhysics : VehicleComponent
{
    public override void AddComponent(ModVehicle parentVehicle)
    {
        var rigidbody = parentVehicle.Prefab.AddComponent<Rigidbody>();
        rigidbody.mass = 800;
        rigidbody.useGravity = false;
        rigidbody.drag = 2;
        rigidbody.angularDrag = 4;

        parentVehicle.VehicleBehaviour.useRigidbody = rigidbody;

        var worldForces = parentVehicle.Prefab.AddComponent<WorldForces>();
        worldForces.useRigidbody = rigidbody;
        worldForces.underwaterGravity = 0;
        worldForces.aboveWaterGravity = 9.81f;
        worldForces.waterDepth = -5f;

        parentVehicle.VehicleBehaviour.worldForces = worldForces;

        var dealDamageOnImpact = parentVehicle.Prefab.AddComponent<DealDamageOnImpact>();
        dealDamageOnImpact.speedMinimumForSelfDamage = 4;
        dealDamageOnImpact.speedMinimumForDamage = 2;
        dealDamageOnImpact.affectsEcosystem = true;
        dealDamageOnImpact.allowDamageToPlayer = false;

        var constructionObstacle = parentVehicle.Prefab.AddComponent<ConstructionObstacle>();
        constructionObstacle.reason = "VehicleObstacle";
    }
}