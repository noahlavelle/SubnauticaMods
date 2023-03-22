using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleVoice : VehicleComponent
{
    private readonly VehicleVoiceLine _enterVehicle;
    private readonly FMODAsset _damageSound;
    
    public VehicleVoice(VehicleVoiceLine enterVehicle, FMODAsset damageSound)
    {
        _enterVehicle = enterVehicle;
        _damageSound = damageSound;
    }
    
    public VehicleVoice(VehicleVoiceLine enterVehicle, string damagePath, string damageId, string damageName)
    {
        _enterVehicle = enterVehicle;
        _damageSound = AssetManager.LoadFmodAsset(damageId, damagePath, damageName);
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        var enterNotification = parentVehicle.Prefab.AddComponent<VoiceNotification>();
        enterNotification.sound = _enterVehicle.Asset;
        enterNotification.text = _enterVehicle.Text;

        var soundOnDamage = parentVehicle.Prefab.AddComponent<SoundOnDamage>();
        soundOnDamage.damageType = DamageType.Collide;
        soundOnDamage.sound = _damageSound;

    }
}