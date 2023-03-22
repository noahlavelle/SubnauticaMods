using UnityEngine;

namespace VehicleFramework;

public class VehicleVoiceLine
{
    public FMODAsset Asset;
    public string Text;

    public VehicleVoiceLine(FMODAsset asset, string text)
    {
        Asset = asset;
        Text = text;
    }

    public VehicleVoiceLine(string path, string id, string name, string text)
    {
        Asset = ScriptableObject.CreateInstance<FMODAsset>();
        Asset.path = path;
        Asset.id = id;
        Asset.name = name;
        Text = text;
    }
}