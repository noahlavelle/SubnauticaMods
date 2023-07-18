namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class HealthHandler : HandlerComponent
{
    private LiveMixin _liveMixin;
    private LiveMixinData _liveMixinData;
    private TemperatureDamage _temperatureDamage;

    public override void Instantiate()
    {
        _liveMixin = parentVehicle.Model.AddComponent<LiveMixin>();
        _temperatureDamage = parentVehicle.Model.AddComponent<TemperatureDamage>();

        parentVehicle.Behaviour.liveMixin = _liveMixin;
        _temperatureDamage.liveMixin = _liveMixin;
    }

    public HealthHandler WithConfig(
        float health, float temperatureDamage, float temperatureDamageMinimum, float temperatureDps, bool onlyLavaDamage, bool repairable, bool broadcastKillOnDeath
        )
    {
        _liveMixin.health = health;
        _liveMixin.tempDamage = temperatureDamage;

        _liveMixinData = ScriptableObject.CreateInstance<LiveMixinData>();
        _liveMixinData.broadcastKillOnDeath = broadcastKillOnDeath;
        _liveMixinData.weldable = repairable;
        _liveMixinData.maxHealth = health;

        _liveMixin.data = _liveMixinData;

        _temperatureDamage.minDamageTemperature = temperatureDamageMinimum;
        _temperatureDamage.baseDamagePerSecond = temperatureDps;
        _temperatureDamage.onlyLavaDamage = onlyLavaDamage;

        return this;
    }
    
    public HealthHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}