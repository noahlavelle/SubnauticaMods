namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

[RequireComponent(typeof(HealthHandler))]
public class CrushDepthHandler : HandlerComponent
{
    [SerializeField] private FMOD_CustomEmitter _crushDamageEmitter;
    [SerializeField] private FMODAsset _warningAsset;
    [SerializeField] private float _kBaseCrushDepth;
    [SerializeField] private float _damagePerCrush;
    [SerializeField] private float _crushPeriod;
    
    public void Awake()
    {
        var crushDamage = gameObject.AddComponent<CrushDamage>();
        crushDamage.soundOnDamage = _crushDamageEmitter;
        crushDamage.liveMixin = gameObject.GetComponent<HealthHandler>().liveMixin;
        crushDamage.vehicle = VehicleBehaviourHandler;
        crushDamage.kBaseCrushDepth = _kBaseCrushDepth;
        crushDamage.damagePerCrush = _damagePerCrush;
        crushDamage.crushPeriod = _crushPeriod;
        
        var crushDamageNotification = gameObject.AddComponent<VoiceNotification>();
        crushDamageNotification.minInterval = 20f;
        crushDamageNotification.sound = _warningAsset;
        
        var depthAlarmsConditionRules = gameObject.AddComponent<ConditionRules>();
        
        var depthAlarms = gameObject.AddComponent<DepthAlarms>();
        depthAlarms.crushDamage = crushDamage;
        depthAlarms.crushDepthNotification = crushDamageNotification;
        depthAlarms.conditionRules = depthAlarmsConditionRules;

        VehicleBehaviourHandler.crushDamage = crushDamage;
        VehicleBehaviourHandler.depthAlarms = depthAlarms;
    }

    public CrushDepthHandler WithSound(GameObject audioParent, FMODAsset crushAsset, FMODAsset warningAsset)
    {
        _crushDamageEmitter = audioParent.AddComponent<FMOD_CustomEmitter>();
        _crushDamageEmitter.restartOnPlay = true;
        _crushDamageEmitter.asset = crushAsset;
        
        _warningAsset = warningAsset;

        return this;
    }

    public CrushDepthHandler WithConfig(float baseCrushDepth, float damagePerCrush, float crushPeriod)
    {
        _kBaseCrushDepth = baseCrushDepth;
        _damagePerCrush = damagePerCrush;
        _crushPeriod = crushPeriod;

        return this;
    }
}