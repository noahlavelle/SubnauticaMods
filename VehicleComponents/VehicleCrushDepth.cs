using UnityEngine;
using UWE;

namespace VehicleFramework.VehicleComponents;

public class VehicleCrushDepth : VehicleComponent
{
    private readonly VehicleCrushDepthConfig _crushDepthConfig;
    private readonly FMODAsset _crushDamageAsset;
    private readonly FMODAsset _crushDamageWarningAsset;
    private GameObject _crushDamageParent;
    
    public VehicleCrushDepth(VehicleCrushDepthConfig crushDepthConfig, FMODAsset crushDamageAsset, FMODAsset crushDamageWarningAsset, GameObject crushDamageParent = null)
    {
        _crushDepthConfig = crushDepthConfig;
        _crushDamageAsset = crushDamageAsset;
        _crushDamageWarningAsset = crushDamageWarningAsset;
        _crushDamageParent = crushDamageParent;
    }
    
    public VehicleCrushDepth(VehicleCrushDepthConfig crushDepthConfig, string crushDamagePath, string crushDamageID, string crushDamageName,
        string crushWarningPath, string crushWarningID, string crushWarningName, GameObject crushDamageParent = null
    )
    {
        _crushDepthConfig = crushDepthConfig;
        _crushDamageParent = crushDamageParent;
        _crushDamageAsset = AssetManager.LoadFmodAsset(crushDamageID, crushDamagePath, crushDamageName);
        _crushDamageWarningAsset = AssetManager.LoadFmodAsset(crushWarningID, crushWarningPath, crushWarningName);
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        var vehicleConfig = parentVehicle.GetVehicleComponentOfType<VehicleLiveMixin>();
        if (vehicleConfig == null)
        {
            Plugin.Log.LogError("VehicleCrushDepth requires a VehicleConfig");
            return;
        }
        
        if (!_crushDamageParent)
        {
            _crushDamageParent = parentVehicle.Prefab.transform.Find("crushDamageSound").gameObject;
        }
        
        var crushDamage = parentVehicle.Prefab.AddComponent<CrushDamage>();
        var crushDamageNotification = parentVehicle.Prefab.AddComponent<VoiceNotification>();
        var crushDamageEmitter = _crushDamageParent.AddComponent<FMOD_CustomEmitter>();
        
        crushDamageEmitter.restartOnPlay = true;
        crushDamageEmitter.asset = _crushDamageAsset;
        
        crushDamageNotification.minInterval = 20f;
        crushDamageNotification.sound = _crushDamageWarningAsset;

        crushDamage.liveMixin = vehicleConfig.LiveMixin;
        crushDamage.vehicle = parentVehicle.VehicleBehaviour;
        crushDamage.kBaseCrushDepth = _crushDepthConfig.BaseCrushDepth;
        crushDamage.damagePerCrush = _crushDepthConfig.DamagePerCrush;
        crushDamage.crushPeriod = _crushDepthConfig.CrushPeriod;
        crushDamage.soundOnDamage = crushDamageEmitter;

        var depthAlarms = parentVehicle.Prefab.AddComponent<DepthAlarms>();
        depthAlarms.crushDamage = crushDamage;
        depthAlarms.crushDepthNotification = crushDamageNotification;
        depthAlarms.conditionRules = parentVehicle.Prefab.AddComponent<ConditionRules>();
    }

    public class VehicleCrushDepthConfig
    {
        public float BaseCrushDepth;
        public float DamagePerCrush;
        public float CrushPeriod;

        public VehicleCrushDepthConfig(float baseCrushDepth, float damagePerCrush, float crushPeriod)
        {
            BaseCrushDepth = baseCrushDepth;
            DamagePerCrush = damagePerCrush;
            CrushPeriod = crushPeriod;
        }
    }
}