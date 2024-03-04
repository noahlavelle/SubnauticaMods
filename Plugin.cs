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
using VehicleFrameworkNautilus.Testing;

namespace VehicleFrameworkNautilus;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }
    
    private static readonly Harmony Harmony = new(PluginInfo.PLUGIN_GUID);
    
    public static string ModFolderPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public static AssetBundle AssetBundle;

    public static readonly Dictionary<TechType, VehicleContainerHandler> RegisteredVehicles = new();

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
        
        var seamothContainer = new SeamothContainer();
        seamothContainer.Register();
        
        var seamothDepth1 = new SeamothDepthMK1
        {
            VehicleContainer = seamothContainer
        };
        var seamothDepth2 = new SeamothDepthMK2
        {
            VehicleContainer = seamothContainer
        };
        var seamothDepth3 = new SeamothDepthMK3
        {
            VehicleContainer = seamothContainer
        };
        
        
        seamothDepth1.Register();
        seamothDepth2.Register();
        seamothDepth3.Register();
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}