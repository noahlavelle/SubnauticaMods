using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleLights : VehicleComponent
{
    private ToggleLights _lightToggle;
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
            Plugin.Log.LogError("VehicleLights requires a VehicleEnergyManager");
            return;
        }

        _lightToggle = _lightingParent.AddComponent<ToggleLights>();
        _lightToggle.lightsParent = _lightingParent;
        _lightToggle.energyMixin = energyManager.EnergyMixin;
        _lightToggle.lightsActive = false;

        var lightsOnSound = AssetManager.LoadFmodAsset("event:/sub/seamoth/seamoth_light_on", "{ff87fd3b-ef80-40be-8eed-cf3b1da407c5}", "seamoth_light_on");
        var lightsOffSound = AssetManager.LoadFmodAsset("event:/sub/seamoth/seamoth_light_off", "{b98a6113-583d-437d-bece-ea8e245cbe55}", "seamoth_light_off");

        _lightToggle.lightsOnSound = parentVehicle.Prefab.AddComponent<FMOD_StudioEventEmitter>();
        _lightToggle.lightsOffSound = parentVehicle.Prefab.AddComponent<FMOD_StudioEventEmitter>();

        _lightToggle.lightsOnSound.asset = lightsOnSound;
        _lightToggle.lightsOnSound.path = "{ff87fd3b-ef80-40be-8eed-cf3b1da407c5}";
        
        _lightToggle.lightsOffSound.asset = lightsOffSound;
        _lightToggle.lightsOffSound.path = "{b98a6113-583d-437d-bece-ea8e245cbe55}";
    }
}