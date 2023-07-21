namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class HealthHandler : HandlerComponent
{
    public LiveMixin LiveMixin;
    private LiveMixinData _liveMixinData;
    private TemperatureDamage _temperatureDamage;

    public override void Instantiate()
    {
        LiveMixin = parentVehicle.Model.AddComponent<LiveMixin>();
        _temperatureDamage = parentVehicle.Model.AddComponent<TemperatureDamage>();

        parentVehicle.Behaviour.liveMixin = LiveMixin;
        _temperatureDamage.liveMixin = LiveMixin;
    }

    public HealthHandler WithConfig(
        float health, float temperatureDamage, float temperatureDamageMinimum, float temperatureDps, bool onlyLavaDamage, bool repairable, bool broadcastKillOnDeath
        )
    {
        LiveMixin.health = health;
        LiveMixin.tempDamage = temperatureDamage;

        _liveMixinData = ScriptableObject.CreateInstance<LiveMixinData>();
        _liveMixinData.broadcastKillOnDeath = broadcastKillOnDeath;
        _liveMixinData.weldable = repairable;
        _liveMixinData.maxHealth = health;

        LiveMixin.data = _liveMixinData;

        _temperatureDamage.minDamageTemperature = temperatureDamageMinimum;
        _temperatureDamage.baseDamagePerSecond = temperatureDps;
        _temperatureDamage.onlyLavaDamage = onlyLavaDamage;

        return this;
    }
    
    public HealthHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}