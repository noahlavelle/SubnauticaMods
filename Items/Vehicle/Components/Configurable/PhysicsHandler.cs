namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class PhysicsHandler : HandlerComponent
{
    private Rigidbody _rigidbody;
    private WorldForces _worldForces;
    private DealDamageOnImpact _dealDamageOnImpact;

    public override void Instantiate()
    {
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _worldForces = gameObject.AddComponent<WorldForces>();
        _dealDamageOnImpact = gameObject.AddComponent<DealDamageOnImpact>();
        
        _worldForces.useRigidbody = _rigidbody;

        var constructionObstacle = gameObject.AddComponent<ConstructionObstacle>();
        constructionObstacle.reason = "VehicleObstacle";
    }
    
    public PhysicsHandler WithPhysicsConfig(PhysicsHandlerConfig config)
    {
        _rigidbody.mass = config.Mass;
        _rigidbody.useGravity = false;
        _rigidbody.drag = config.Drag;
        _rigidbody.angularDrag = config.AngularDrag;
        
        _worldForces.underwaterGravity = config.UnderwaterGravity;
        _worldForces.aboveWaterGravity = config.AboveWaterGravity;
        _worldForces.waterDepth = config.WaterDepth;
        
        _dealDamageOnImpact.speedMinimumForSelfDamage = config.SpeedMinimumForSelfDamage;
        _dealDamageOnImpact.speedMinimumForDamage = config.SpeedMinimumForDamage;
        _dealDamageOnImpact.affectsEcosystem = config.AffectsEcosystem;
        _dealDamageOnImpact.allowDamageToPlayer = config.AllowDamageToPlayer;
        
        
        return this;
    }

    public PhysicsHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
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