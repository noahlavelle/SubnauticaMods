using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleSounds : VehicleComponent
{
    private readonly VehicleVoiceLine _enterVehicle;
    private readonly FMODAsset _damageSound;
    private readonly FMODAsset _splashSound;
    
    public VehicleSounds(VehicleVoiceLine enterVehicle, FMODAsset damageSound, FMODAsset splashSound)
    {
        _enterVehicle = enterVehicle;
        _damageSound = damageSound;
        _splashSound = splashSound;
    }
    
    public VehicleSounds(VehicleVoiceLine enterVehicle,
        string damagePath = "event:/sub/seamoth/impact_solid_soft", string damageId = "{15dc7344-7b0a-4ffd-9b5c-c40f923e4f4d}", string damageName = "impact_solid_soft",
        string splashPath = "event:/sub/common/splash_in_and_out", string splashId = "{eefd976d-e272-4ad7-9a60-3e0d22066b66}", string splashName = "splash_in_and_out")
    {
        _enterVehicle = enterVehicle;
        _damageSound = AssetManager.LoadFmodAsset(damageId, damagePath, damageName);
        _splashSound = AssetManager.LoadFmodAsset(splashId, splashPath, splashName);
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        var enterNotification = parentVehicle.Prefab.AddComponent<VoiceNotification>();
        enterNotification.sound = _enterVehicle.Asset;
        enterNotification.text = _enterVehicle.Text;

        var soundOnDamage = parentVehicle.Prefab.AddComponent<SoundOnDamage>();
        soundOnDamage.damageType = DamageType.Collide;
        soundOnDamage.sound = _damageSound;

        parentVehicle.VehicleBehaviour.splashSound = _splashSound;
        parentVehicle.VehicleBehaviour.welcomeNotification = enterNotification;
    }
}