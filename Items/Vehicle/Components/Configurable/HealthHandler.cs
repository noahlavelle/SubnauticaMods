using UnityEngine.Serialization;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class HealthHandler : HandlerComponent
{
    public LiveMixin liveMixin;
    
    [SerializeField] private LiveMixinData _liveMixinData;
    [SerializeField] private float _health;
    [SerializeField] private float _temperatureDamageAmount;
    [SerializeField] private float _minDamageTemperature;
    [SerializeField] private float _baseDamagePerSecond;
    [SerializeField] private bool _onlyLavaDamage;

    public void Awake()
    {
        liveMixin = gameObject.AddComponent<LiveMixin>();
        liveMixin.health = _health;
        liveMixin.tempDamage = _temperatureDamageAmount;
        liveMixin.data = _liveMixinData;
        
        var temperatureDamage = gameObject.AddComponent<TemperatureDamage>();
        temperatureDamage.minDamageTemperature = _minDamageTemperature;
        temperatureDamage.baseDamagePerSecond = _baseDamagePerSecond;
        temperatureDamage.onlyLavaDamage = _onlyLavaDamage;
        
        temperatureDamage.liveMixin = liveMixin;

        VehicleBehaviour.liveMixin = liveMixin;
        
    }

    public HealthHandler WithConfig(
        float health, float temperatureDamage, float temperatureDamageMinimum, float temperatureDps, bool onlyLavaDamage, bool repairable, bool broadcastKillOnDeath
        )
    {
        _health = health;
        _temperatureDamageAmount = temperatureDamage;

        _liveMixinData = ScriptableObject.CreateInstance<LiveMixinData>();
        _liveMixinData.broadcastKillOnDeath = broadcastKillOnDeath;
        _liveMixinData.weldable = repairable;
        _liveMixinData.maxHealth = health;

        _minDamageTemperature = temperatureDamageMinimum;
        _baseDamagePerSecond = temperatureDps;
        _onlyLavaDamage = onlyLavaDamage;

        return this;
    }
}