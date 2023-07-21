namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class SoundHandler : HandlerComponent
{
    private VoiceNotification _enterNotification;
    private SoundOnDamage _soundOnDamage;

    private FMOD_CustomEmitter _engineRevUpEmitter;
    private FMOD_CustomLoopingEmitter _engineLoopingEmitter;
    private EngineRpmSFXManager _engineRpmSfxManager;
    
    public override void Instantiate()
    {
        _enterNotification = parentVehicle.Model.AddComponent<VoiceNotification>();
        _soundOnDamage = parentVehicle.Model.AddComponent<SoundOnDamage>();
        _engineRevUpEmitter = parentVehicle.Model.AddComponent<FMOD_CustomEmitter>();
        _engineLoopingEmitter = parentVehicle.Model.AddComponent<FMOD_CustomLoopingEmitter>();
        _engineRpmSfxManager = parentVehicle.Model.AddComponent<EngineRpmSFXManager>();
        
        _soundOnDamage.damageType = DamageType.Collide;

        _engineRevUpEmitter.restartOnPlay = true;
        _engineRpmSfxManager.rampDownSpeed = 0.5f;
        _engineRpmSfxManager.engineRevUp = _engineRevUpEmitter;
        _engineRpmSfxManager.engineRpmSFX = _engineLoopingEmitter;

        parentVehicle.Behaviour.welcomeNotification = _enterNotification;
        parentVehicle.Behaviour.engineRpmSfxManager = _engineRpmSfxManager;

    }

    public SoundHandler WithSounds(FMODAsset welcomeSound, FMODAsset damageSound, FMODAsset splashSound, FMODAsset revUpSound, FMODAsset revLoopSound, string welcomeText)
    {
        _enterNotification.sound = welcomeSound;
        _enterNotification.text = welcomeText;
        
        _engineRevUpEmitter.asset = revUpSound;
        _engineLoopingEmitter.asset = revLoopSound;

        _soundOnDamage.sound = damageSound;
        parentVehicle.Behaviour.splashSound = splashSound;

        return this;
    }
    
    public SoundHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}