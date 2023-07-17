namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class PhysicsHandler : IHandlerComponent
{
    public PhysicsHandlerConfig HandlerConfig { get; set; }

    public GameObject GameObject { get; set; }

    public void Instantiate()
    {
        var rigidbody = GameObject.AddComponent<Rigidbody>();
        rigidbody.mass = HandlerConfig.Mass;
        rigidbody.useGravity = false;
        rigidbody.drag = HandlerConfig.Drag;
        rigidbody.angularDrag = HandlerConfig.AngularDrag;

        var worldForces = GameObject.AddComponent<WorldForces>();
        worldForces.useRigidbody = rigidbody;
        worldForces.underwaterGravity = HandlerConfig.UnderwaterGravity;
        worldForces.aboveWaterGravity = HandlerConfig.AboveWaterGravity;
        worldForces.waterDepth = HandlerConfig.WaterDepth;

        var dealDamageOnImpact = GameObject.AddComponent<DealDamageOnImpact>();
        dealDamageOnImpact.speedMinimumForSelfDamage = HandlerConfig.SpeedMinimumForSelfDamage;
        dealDamageOnImpact.speedMinimumForDamage = HandlerConfig.SpeedMinimumForDamage;
        dealDamageOnImpact.affectsEcosystem = HandlerConfig.AffectsEcosystem;
        dealDamageOnImpact.allowDamageToPlayer = HandlerConfig.AllowDamageToPlayer;

        var constructionObstacle = GameObject.AddComponent<ConstructionObstacle>();
        constructionObstacle.reason = "VehicleObstacle";
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