namespace VehicleFrameworkNautilus;

public static class AssetHelper
{
    public static FMODAsset LoadFmodAsset(string id, string path, string name)
    {
        var asset = ScriptableObject.CreateInstance<FMODAsset>();
        asset.id = id;
        asset.path = path;
        asset.name = name;

        return asset;
    }

    public static FMODAsset StorageOpenSound = LoadFmodAsset(
        "event:/sub/seamoth/storage_open",
        "{b2821359-928c-4780-876a-0957de85f193}",
        "storage_open");

    public static FMODAsset StorageCloseSound = LoadFmodAsset(
        "event:/sub/seamoth/storage_close",
        "{88d7834f-d050-470b-8ee5-1db7b7defa6f}",
        "storage_close");

    public static FMODAsset SeamothWelcomeSound = LoadFmodAsset(
        "event:/sub/seamoth/welcome",
        "{d1db5c9b-4953-4ee7-ae98-fcc72347bf10}",
        "seamoth-welcome");

    public static FMODAsset SeamothRevUpSound = LoadFmodAsset(
        "event:/sub/seamoth/seamoth_rev_up",
        "{66b9bbc2-44d6-44b4-9d5c-2af7ac64e05c}",
        "seamoth_rev_up");

    public static FMODAsset SeamothRevLoopSound = LoadFmodAsset(
        "event:/sub/seamoth/seamoth_loop_rpm",
        "{102faef9-1e51-45a3-9032-e9025cbeae3e}",
        "seamoth_loop_rpm");

    public static FMODAsset DamageSound = LoadFmodAsset(
        "event:/sub/seamoth/impact_solid_soft",
        "{15dc7344-7b0a-4ffd-9b5c-c40f923e4f4d}",
        "impact_solid_soft");
    
    public static FMODAsset SplashSound = LoadFmodAsset(
        "event:/sub/common/splash_in_and_out",
        "{eefd976d-e272-4ad7-9a60-3e0d22066b66}",
        "splash_in_and_out");
}