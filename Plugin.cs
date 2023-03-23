using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace VehicleFramework
{
    [BepInPlugin("com.noahlavelle.vehicleframework", "VehicleFramework", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log = new("VehicleFramework");
        
        public static string ModFolderPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static AssetBundle AssetBundle;
        
        public static Dictionary<TechType, ModVehicle> Vehicles;
        
        private void Awake()
        {
            Log = Logger;
            Vehicles = new Dictionary<TechType, ModVehicle>();
            
            AssetBundle = AssetBundle.LoadFromFile(Path.Combine(ModFolderPath, "Assets", "seamoth"));
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.noahlavelle.seamoth");
            
            var _ = new Seamoth.Seamoth();

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
