namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class SoundHandler : HandlerComponent
{
    [SerializeField] private EngineRpmSFXManager _engineRpmSfxManager;
    
    [SerializeField] private FMODAsset _welcomeSound;
    [SerializeField] private string _welcomeText;
    [SerializeField] private FMODAsset _revUpSound;
    [SerializeField] private FMODAsset _revLoopSound;
    [SerializeField] private FMODAsset _damageSound;
    [SerializeField] private FMODAsset _splashSound;
    
    public void Awake()
    {
        var enterNotification = gameObject.AddComponent<VoiceNotification>();
        enterNotification.sound = _welcomeSound;
        enterNotification.text = _welcomeText;
        
        var soundOnDamage = gameObject.AddComponent<SoundOnDamage>();
        soundOnDamage.sound = _damageSound;
        soundOnDamage.damageType = DamageType.Collide;
        
        VehicleBehaviour.welcomeNotification = enterNotification;
        VehicleBehaviour.splashSound = _splashSound;
    }

    public SoundHandler WithSounds(FMODAsset welcomeSound, FMODAsset damageSound, FMODAsset splashSound, FMODAsset revUpSound, FMODAsset revLoopSound, string welcomeText)
    {
        _welcomeSound = welcomeSound;
        _welcomeText = welcomeText;
        _damageSound = damageSound;
        _revUpSound = revUpSound;
        _revLoopSound = revLoopSound;
        _splashSound = splashSound;
        
        var engineRevUpEmitter = gameObject.AddComponent<FMOD_CustomEmitter>();
        engineRevUpEmitter.asset = _revUpSound;
        engineRevUpEmitter.restartOnPlay = true;
        
        var engineLoopingEmitter = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
        engineLoopingEmitter.asset = _revLoopSound;
        Plugin.Logger.LogInfo(_revLoopSound);
        
        _engineRpmSfxManager = gameObject.AddComponent<EngineRpmSFXManager>();
        _engineRpmSfxManager.rampDownSpeed = 0.5f;
        _engineRpmSfxManager.engineRevUp = engineRevUpEmitter;
        _engineRpmSfxManager.engineRpmSFX = engineLoopingEmitter;


        return this;
    }
    
    public void Update()
    {
        var vector = AvatarInputHandler.main.IsEnabled() ? GameInput.GetMoveDirection() : Vector3.zero;
        if (VehicleBehaviour.CanPilot() && vector.magnitude > 0f && VehicleBehaviour.GetPilotingMode())
        {
            _engineRpmSfxManager.AccelerateInput();
        }
    }
}