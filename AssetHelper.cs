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

    public static readonly FMODAsset StorageOpenSound = LoadFmodAsset(
        "event:/sub/seamoth/storage_open",
        "{b2821359-928c-4780-876a-0957de85f193}",
        "storage_open");

    public static readonly FMODAsset StorageCloseSound = LoadFmodAsset(
        "event:/sub/seamoth/storage_close",
        "{88d7834f-d050-470b-8ee5-1db7b7defa6f}",
        "storage_close");

    public static readonly FMODAsset SeamothWelcomeSound = LoadFmodAsset(
        "event:/sub/seamoth/welcome",
        "{d1db5c9b-4953-4ee7-ae98-fcc72347bf10}",
        "seamoth-welcome");

    public static readonly FMODAsset SeamothRevUpSound = LoadFmodAsset(
        "event:/sub/seamoth/seamoth_rev_up",
        "{66b9bbc2-44d6-44b4-9d5c-2af7ac64e05c}",
        "seamoth_rev_up");

    public static readonly FMODAsset SeamothRevLoopSound = LoadFmodAsset(
        "event:/sub/seamoth/seamoth_loop_rpm",
        "{102faef9-1e51-45a3-9032-e9025cbeae3e}",
        "seamoth_loop_rpm");

    public static readonly FMODAsset SeamothDepthWarning = LoadFmodAsset(
        "event:/sub/seamoth/crush_depth_warning",
        "{26b49966-51f1-4336-a6bf-64cc2baf13d1}",
        "crush_depth_warning");

    public static readonly FMODAsset DamageSound = LoadFmodAsset(
        "event:/sub/seamoth/impact_solid_soft",
        "{15dc7344-7b0a-4ffd-9b5c-c40f923e4f4d}",
        "impact_solid_soft");
    
    public static readonly FMODAsset SplashSound = LoadFmodAsset(
        "event:/sub/common/splash_in_and_out",
        "{eefd976d-e272-4ad7-9a60-3e0d22066b66}",
        "splash_in_and_out");

    public static readonly FMODAsset CrushDamageSound = LoadFmodAsset(
        "event:/sub/seamoth/crush_damage",
        "{905b4b2c-e1cc-4420-8040-bf45df04ce08}",
        "crush_damage");

    public static readonly FMODAsset LightOnSound = LoadFmodAsset(
        "event:/sub/seamoth/seamoth_light_on",
        "{ff87fd3b-ef80-40be-8eed-cf3b1da407c5}",
        "seamoth_light_on");
    
    public static readonly FMODAsset LightOffSound = LoadFmodAsset(
        "event:/sub/seamoth/seamoth_light_off",
        "{b98a6113-583d-437d-bece-ea8e245cbe55}",
        "seamoth_light_off");
}