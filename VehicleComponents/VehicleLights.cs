using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleLights : VehicleComponent
{
    private ToggleLights _lightsToggle;
    private VFXVolumetricLight[] _volumetricLights;
    private GameObject _lightingParent;

    public VehicleLights(GameObject lightingParent = null)
    {
        _lightingParent = lightingParent;
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        if (!_lightingParent)
        {
            _lightingParent = parentVehicle.Prefab.transform.Find("Lighting").gameObject;
        }

        var energyManager = parentVehicle.GetVehicleComponentOfType<VehicleEnergyManager>();
        if (energyManager == null)
        {
            Plugin.Log.LogError("VehicleLights requires a VehicleEnergyManager component");
            return;
        }

        _lightsToggle = _lightingParent.AddComponent<ToggleLights>();
        _lightsToggle.lightsParent = _lightingParent;
        _lightsToggle.energyMixin = energyManager.EnergyMixin;
        _lightsToggle.lightsActive = false;

        var lightsOnSound = AssetManager.LoadFmodAsset("event:/sub/seamoth/seamoth_light_on", "{ff87fd3b-ef80-40be-8eed-cf3b1da407c5}", "seamoth_light_on");
        var lightsOffSound = AssetManager.LoadFmodAsset("event:/sub/seamoth/seamoth_light_off", "{b98a6113-583d-437d-bece-ea8e245cbe55}", "seamoth_light_off");

        _lightsToggle.lightsOnSound = parentVehicle.Prefab.AddComponent<FMOD_StudioEventEmitter>();
        _lightsToggle.lightsOffSound = parentVehicle.Prefab.AddComponent<FMOD_StudioEventEmitter>();

        _lightsToggle.lightsOnSound.asset = lightsOnSound;
        _lightsToggle.lightsOnSound.path = "{ff87fd3b-ef80-40be-8eed-cf3b1da407c5}";
        
        _lightsToggle.lightsOffSound.asset = lightsOffSound;
        _lightsToggle.lightsOffSound.path = "{b98a6113-583d-437d-bece-ea8e245cbe55}";

        parentVehicle.VehicleBehaviour.LightsToggle = _lightsToggle;
    }
}