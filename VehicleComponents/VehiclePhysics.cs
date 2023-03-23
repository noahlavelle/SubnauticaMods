using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehiclePhysics : VehicleComponent
{
    public Rigidbody Rigidbody;
    
    private string _collisionPath;
    
    public VehiclePhysics(string collisionPath = "Collision")
    {
        _collisionPath = collisionPath;
    }
    
    public override void AddComponent(ModVehicle parentVehicle)
    {
        Rigidbody = parentVehicle.Prefab.AddComponent<Rigidbody>();
        Rigidbody.mass = 800;
        Rigidbody.useGravity = false;
        Rigidbody.drag = 2;
        Rigidbody.angularDrag = 4;

        parentVehicle.VehicleBehaviour.useRigidbody = Rigidbody;

        var worldForces = parentVehicle.Prefab.AddComponent<WorldForces>();
        worldForces.useRigidbody = Rigidbody;
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

        parentVehicle.VehicleBehaviour.collisionModel = parentVehicle.Prefab.transform.Find(_collisionPath).gameObject;
    }
}