using UnityEngine.Serialization;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class PhysicsHandler : HandlerComponent
{
    public Rigidbody rigidbody;
    public WorldForces worldForces;
    
    [SerializeField] private PhysicsHandlerConfig _config;
    [SerializeField] private GameObject collisionParent;

    public void Awake()
    {
        var dealDamageOnImpact = gameObject.AddComponent<DealDamageOnImpact>();
        dealDamageOnImpact.speedMinimumForSelfDamage = _config.SpeedMinimumForSelfDamage;
        dealDamageOnImpact.speedMinimumForDamage = _config.SpeedMinimumForDamage;
        dealDamageOnImpact.affectsEcosystem = _config.AffectsEcosystem;
        dealDamageOnImpact.allowDamageToPlayer = _config.AllowDamageToPlayer;
        
        var constructionObstacle = gameObject.AddComponent<ConstructionObstacle>();
        constructionObstacle.reason = "VehicleObstacle";
        
        VehicleBehaviourHandler.collisionModel = collisionParent;
    }

    public PhysicsHandler WithCollision(GameObject collisionParent)
    {
        this.collisionParent = collisionParent;

        return this;
    }
    
    public PhysicsHandler WithPhysicsConfig(PhysicsHandlerConfig config)
    {
        _config = config;

        rigidbody = gameObject.AddComponent<Rigidbody>();
        worldForces = gameObject.AddComponent<WorldForces>();
        
        rigidbody.mass = _config.Mass;
        rigidbody.useGravity = false;
        rigidbody.drag = _config.Drag;
        rigidbody.angularDrag = _config.AngularDrag;
        
        worldForces.underwaterGravity = _config.UnderwaterGravity;
        worldForces.aboveWaterGravity = _config.AboveWaterGravity;
        worldForces.waterDepth = _config.WaterDepth;
        worldForces.useRigidbody = rigidbody;
        
        VehicleBehaviourHandler.useRigidbody = rigidbody;
        VehicleBehaviourHandler.worldForces = worldForces;
        
        return this;
    }
}

public struct PhysicsHandlerConfig
{
    public readonly float Mass;
    public readonly float Drag;
    public readonly float AngularDrag;
    public readonly float UnderwaterGravity;
    public readonly float AboveWaterGravity;
    public readonly float WaterDepth;

    public readonly float SpeedMinimumForSelfDamage;
    public readonly float SpeedMinimumForDamage;
    public readonly bool AffectsEcosystem;
    public readonly bool AllowDamageToPlayer;
    

    public PhysicsHandlerConfig(float mass, float drag, float angularDrag, float underwaterGravity, float aboveWaterGravity, float waterDepth, float speedMinimumForSelfDamage, float speedMinimumForDamage, bool affectsEcosystem, bool allowDamageToPlayer)
    {
        Mass = mass;
        Drag = drag;
        AngularDrag = angularDrag;
        UnderwaterGravity = underwaterGravity;
        AboveWaterGravity = aboveWaterGravity;
        WaterDepth = waterDepth;
        SpeedMinimumForSelfDamage = speedMinimumForSelfDamage;
        SpeedMinimumForDamage = speedMinimumForDamage;
        AffectsEcosystem = affectsEcosystem;
        AllowDamageToPlayer = allowDamageToPlayer;
    }
}