using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleEngineSFX : VehicleComponent
{
    private readonly FMODAsset _revUpAsset;
    private readonly FMODAsset _loopAsset;
    private GameObject _sfxObject;
    
    public VehicleEngineSFX(FMODAsset engineRevUpAsset, FMODAsset engineLoopAsset, GameObject sfxObject = null)
    {
        _revUpAsset = engineRevUpAsset;
        _loopAsset = engineLoopAsset;
        _sfxObject = sfxObject;
    }

    public VehicleEngineSFX(string revUpPath, string revUpID, string revUpName, string revLoopPath,
        string revLoopID, string revLoopName, GameObject sfxObject = null)
    {
        _revUpAsset = AssetManager.LoadFmodAsset(revUpID, revUpPath, revUpName);
        _loopAsset = AssetManager.LoadFmodAsset(revLoopID, revLoopPath, revLoopName);
        _sfxObject = sfxObject;
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        if (!_sfxObject)
        {
            _sfxObject = parentVehicle.Prefab.transform.Find("EngineRpmSFX").gameObject;
        }

        var engineRevUpEmitter = _sfxObject.AddComponent<FMOD_CustomEmitter>();
        var engineLoopingEmitter = _sfxObject.AddComponent<FMOD_CustomLoopingEmitter>();
        var engineRpmManager = _sfxObject.AddComponent<EngineRpmSFXManager>();

        engineRevUpEmitter.restartOnPlay = true;
        engineRevUpEmitter.asset = _revUpAsset;
        engineLoopingEmitter.asset = _loopAsset;

        engineRpmManager.rampDownSpeed = 0.5f;
        engineRpmManager.engineRevUp = engineRevUpEmitter;
        engineRpmManager.engineRpmSFX = engineLoopingEmitter;
    }
}