using UnityEngine;

namespace VehicleFramework;

public static class AssetManager
{
    public static FMODAsset LoadFmodAsset(string id, string path, string name)
    {
        var asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.id = id;
        asset.path = path;
        asset.name = name;

        return asset;
    }
}