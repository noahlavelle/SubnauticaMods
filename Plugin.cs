global using BepInEx;
global using BepInEx.Logging;
global using System;
global using UWE;
global using UnityEngine;
global using HarmonyLib;
global using Nautilus.Assets.PrefabTemplates;
global using System.Collections.Generic;
global using Nautilus.Assets.Gadgets;
global using System.Collections;
global using Nautilus.Handlers;
global using Nautilus.Crafting;
global using Nautilus.Utility;
global using static CraftData;
global using Nautilus.Assets;
global using System.IO;
global using System.Reflection;
using VehicleFrameworkNautilus.Extensions;
using VehicleFrameworkNautilus.Items.Vehicle;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

namespace VehicleFrameworkNautilus;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }
    
    private static readonly Harmony Harmony = new(PluginInfo.PLUGIN_GUID);
    
    public static string ModFolderPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public static AssetBundle AssetBundle;

    public static readonly Dictionary<TechType, BaseVehiclePrefab> RegisteredVehicles = new();

    private void Awake()
    {
        CoroutineHost.StartCoroutine(AwakeAsync());
    }

    private IEnumerator AwakeAsync()
    {
        Logger = base.Logger;
        
        AssetBundle = AssetBundle.LoadFromFile(Path.Combine(ModFolderPath, "Assets", "seamoth"));
        yield return VehicleHelper.LoadReferenceVehicleAsync();

        Harmony.PatchAll();
        new SeaMoth().Register();
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}

public class SeaMoth : BaseVehiclePrefab
{
    public override string ClassID => "seamothmod";
    public override string DisplayName => "Alterra Seamoth";
    public override string Description => "Desc";
    public override float CraftTime => 5f;
    public override Sprite CraftIcon => null;

    public override RecipeData Recipe => new()
    {
        craftAmount = 1,
        Ingredients =
        {
            new Ingredient(TechType.TitaniumIngot, 1)
        }
    };

    public override GameObject Model => Plugin.AssetBundle.LoadAsset<GameObject>("SeamothPrefab.prefab");

    protected override void ConfigureVehicle()
    { 
        AddBehaviour<SeaMothBehaviour>();
        
        base.ConfigureVehicle();

        Model.ApplyAlterraVehicleMaterial();

        PhysicsHandler
            .WithPhysicsConfig(
                new PhysicsHandlerConfig(800, 2, 4, 0, 9.81f, -5f, 4, 2, true, false
                ))
            .WithCollision(Model.transform.Find("Collision").gameObject);

        PingHandler
            .WithOrigin(Model.transform.Find("PingOrigin"))
            .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("SeamothPingIcon"))
            .WithName("Seamoth");

        UpgradeModulesHandler
            .WithUpgradeConsole(
                Model.transform.Find("UpgradeConsole"), Model.transform.Find("Model/Vehicle_Anim/UpgradeSlot_Hatch_geo"), Model.transform.Find("UpgradeModulesRoot"))
            .WithSound(AssetHelper.StorageOpenSound, AssetHelper.StorageCloseSound);

        EnergyHandler
            .WithBatterySlot(Model.transform.Find("BatterySlot"));

        PositionHandler
            .WithPositions(
                Model.transform.Find("Model/Vehicle_Anim/Joints/AttachJoint/CameraPivot"),
                Model.transform.Find("EndPosition"),
                Model.transform.Find(
                    "Model/Vehicle_Anim/Joints/Wheel_Base/Wheel_Segment_1/Wheel_Segment_2/Wheel_Head/LeftIKTarget"),
                Model.transform.Find(
                    "Model/Vehicle_Anim/Joints/Wheel_Base/Wheel_Segment_1/Wheel_Segment_2/Wheel_Head/RightIKTarget"),
                Model.transform.Find(
                    "AlternateExits")
            );

        SoundHandler
            .WithSounds(
                AssetHelper.SeamothWelcomeSound,
                AssetHelper.DamageSound,
                AssetHelper.SplashSound, 
                AssetHelper.SeamothRevUpSound,
                AssetHelper.SeamothRevLoopSound,
                "Seamoth: Welcome aboard, captain"
            );

        HealthHandler
            .WithConfig(
                200, -1, 70, 0.2f, true, true, false
            );

        CrushDepthHandler
            .WithConfig(200, 20, 3)
            .WithSound(Model.transform.Find("crushDamageSound").gameObject, AssetHelper.CrushDamage, AssetHelper.SeamothDepthWarning);

        AddComponent<DockingHandler>()
            .WithPositions(new Vector3(0, 0, 0))
            .WithAnimations(Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_dock"),
                Plugin.AssetBundle.LoadAsset<AnimationClip>("loop_seamoth_docked"),
                Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_launch_left"),
                Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_launch_right"));
    }
}

public class SeaMothBehaviour : BaseVehicleBehaviour
{
    protected override string EnterVehicleText => "Enter Seamoth";
    protected override ControlSheme ControlScheme => ControlSheme.Submersible;
    protected override float EnergyConsumptionRate => 1f / 15f;
    protected override float ForwardForce => 12.5f;
    protected override float BackwardForce => 5.4f;
    protected override float SidewardForce => 12.52f;
    protected override float VerticalForce => 11.93f;
}