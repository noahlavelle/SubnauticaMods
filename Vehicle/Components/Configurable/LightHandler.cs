namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class LightHandler : HandlerComponent
{
    [SerializeField] private ToggleLights _toggleLights;
    
    [SerializeField] private Transform _lightingParent;
    [SerializeField] private EnergyHandler _energyHandler;
    [SerializeField] private FMODAsset _onSound;
    [SerializeField] private FMODAsset _offSound;

    public void Awake()
    {
        _toggleLights = gameObject.AddComponent<ToggleLights>();
        _toggleLights.lightsOnSound = gameObject.AddComponent<FMOD_StudioEventEmitter>();
        _toggleLights.lightsOffSound = gameObject.AddComponent<FMOD_StudioEventEmitter>();
        
        _toggleLights.lightsParent = _lightingParent.gameObject;
        _toggleLights.lightsActive = false;
        _toggleLights.energyMixin = _energyHandler.energyMixin;
        
        _toggleLights.lightsOnSound.asset = _onSound;
        _toggleLights.lightsOnSound.path = _onSound.path;
        _toggleLights.lightsOffSound.asset = _offSound;
        _toggleLights.lightsOffSound.path = _offSound.path;
    }

    public LightHandler WithLightingParent(Transform lightingParent)
    {
        _lightingParent = lightingParent;

        return this;
    }

    public LightHandler WithEnergyHandler(EnergyHandler energyHandler)
    {
        _energyHandler = energyHandler;
        
        return this;
    }

    public LightHandler WithSound(FMODAsset onSound, FMODAsset offSound)
    {
        _onSound = onSound;
        _offSound = offSound;

        return this;
    }

    public void Update()
    {
        if (VehicleBehaviourHandler.GetPilotingMode())
            _toggleLights.CheckLightToggle();
    }
}