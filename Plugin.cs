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
    public override float CraftTime => 20f;
    public override Sprite CraftIcon => null;

    public override RecipeData Recipe => new()
    {
        craftAmount = 1,
        Ingredients =
        {
            new Ingredient(TechType.TitaniumIngot, 1)
        }
    };

    public override GameObject gameObject => Plugin.AssetBundle.LoadAsset<GameObject>("SeamothPrefab.prefab");

    protected override void PrepareGameObject()
    { 
        base.PrepareGameObject();

        gameObject.ApplyAlterraVehicleMaterial();

        AddComponent<PhysicsHandler>()
            .WithPhysicsConfig(
                new PhysicsHandlerConfig(800, 2, 4, 0, 9.81f, -5f, 4, 2, true, false
                ));

        AddComponent<PingHandler>()
            .WithOrigin(gameObject.transform.Find("PingOrigin"))
            .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("SeamothPingIcon"))
            .WithName("Seamoth");
    }
}