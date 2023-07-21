namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class CrushDepthHandler : HandlerComponent
{
    private CrushDamage _crushDamage;
    private VoiceNotification _crushDamageNotification;
    private FMOD_CustomEmitter _crushDamageEmitter;
    private DepthAlarms _depthAlarms;
    private ConditionRules _depthAlarmsConditionRules;
    
    public override void Instantiate()
    {
        _crushDamage = parentVehicle.Model.AddComponent<CrushDamage>();
        _crushDamageNotification = parentVehicle.Model.AddComponent<VoiceNotification>();
        _depthAlarms = parentVehicle.Model.AddComponent<DepthAlarms>();
        _depthAlarmsConditionRules = parentVehicle.Model.AddComponent<ConditionRules>();

        parentVehicle.Behaviour.crushDamage = _crushDamage;
        parentVehicle.Behaviour.depthAlarms = _depthAlarms;
    }

    public CrushDepthHandler WithSound(GameObject audioParent, FMODAsset crushAsset, FMODAsset warningAsset)
    {
        _crushDamageEmitter = audioParent.AddComponent<FMOD_CustomEmitter>();
        _crushDamageEmitter.restartOnPlay = true;
        _crushDamageEmitter.asset = crushAsset;

        _crushDamageNotification.minInterval = 20f;
        _crushDamageNotification.sound = warningAsset;

        _crushDamage.soundOnDamage = _crushDamageEmitter;

        _depthAlarms.crushDamage = _crushDamage;
        _depthAlarms.crushDepthNotification = _crushDamageNotification;
        _depthAlarms.conditionRules = _depthAlarmsConditionRules;

        return this;
    }

    public CrushDepthHandler WithConfig(float baseCrushDepth, float damagePerCrush, float crushPeriod)
    {
        _crushDamage.liveMixin = parentVehicle.HealthHandler.LiveMixin;
        _crushDamage.vehicle = parentVehicle.Behaviour;
        _crushDamage.kBaseCrushDepth = baseCrushDepth;
        _crushDamage.damagePerCrush = damagePerCrush;
        _crushDamage.crushPeriod = crushPeriod;

        return this;
    }
    
    public CrushDepthHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}