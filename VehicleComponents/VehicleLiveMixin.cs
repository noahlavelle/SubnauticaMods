using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleLiveMixin : VehicleComponent
{
    public LiveMixin LiveMixin;
    public LiveMixinData LiveMixinData;
    public TemperatureDamage TemperatureDamage;

    private readonly int _health;
    private readonly int _tempDamage;
    private readonly bool _broadcastKillOnDeath;
    private readonly bool _repairable;
    private readonly int _temperatureDamageMinimum;
    private readonly float _temperatureDps;
    private readonly bool _onlyLavaDamage;
    
    public VehicleLiveMixin (int health, int tempDamage, bool broadcastKillOnDeath, bool repairable, int temperatureDamageMinimum, float temperatureDps, bool onlyLavaDamage)
    {
        _health = health;
        _tempDamage = tempDamage;
        _broadcastKillOnDeath = broadcastKillOnDeath;
        _repairable = repairable;
        _temperatureDamageMinimum = temperatureDamageMinimum;
        _temperatureDps = temperatureDps;
        _onlyLavaDamage = onlyLavaDamage;
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        LiveMixin = parentVehicle.Prefab.AddComponent<LiveMixin>();
        TemperatureDamage = parentVehicle.Prefab.AddComponent<TemperatureDamage>();
        LiveMixinData = ScriptableObject.CreateInstance<LiveMixinData>();

        LiveMixin.health = _health;
        LiveMixin.tempDamage = _tempDamage;

        LiveMixinData.broadcastKillOnDeath = _broadcastKillOnDeath;
        LiveMixinData.destroyOnDeath = false;
        LiveMixinData.weldable = _repairable;
        LiveMixinData.maxHealth = _health;

        LiveMixin.data = LiveMixinData;

        parentVehicle.VehicleBehaviour.liveMixin = LiveMixin;

        TemperatureDamage.liveMixin = LiveMixin;
        TemperatureDamage.minDamageTemperature = _temperatureDamageMinimum;
        TemperatureDamage.baseDamagePerSecond = _temperatureDps;
        TemperatureDamage.onlyLavaDamage = _onlyLavaDamage;
    }
}