using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Seamoth.Ship;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;
using HarmonyLib;
using Seamoth.Ship.Modules;

namespace Seamoth
{
    [BepInPlugin("com.noahlavelle.seamoth", "Seamoth", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log = new("Seamoth");
        public static string ModFolderPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static AssetBundle AssetBundle;

        public static EquipmentType SeamothModuleType;
        public static PingType SeamothPingType;
        public static ShipPrefab SeamothPrefab;

        public static readonly ShipDepthModuleMk1 ShipDepthModuleMk1 = new();
        public static readonly ShipDepthModuleMk2 ShipDepthModuleMk2 = new();
        private static readonly ShipDepthModuleMk3 ShipDepthModuleMk3 = new();
        private static readonly ShipEngineEfficiencyModule ShipEngineEfficiencyModule = new();
        private static readonly ShipHullReinforcementModule ShipHullReinforcementModule = new();
        private static readonly ShipPerimeterDefenceModule ShipPerimeterDefenceModule = new();
        private static readonly ShipSolarChargerModule ShipSolarChargerModule = new();
        private static readonly ShipSonarModule ShipSonarModule = new();
        private static readonly ShipStorageModule ShipStorageModule = new();
        private static readonly ShipTorpedoModule ShipTorpedoModule = new();

        private static readonly List<ShipModuleBase> SeamothModules = new()
        {
            ShipDepthModuleMk1,
            ShipDepthModuleMk2,
            ShipDepthModuleMk3,
            ShipEngineEfficiencyModule,
            ShipHullReinforcementModule,
            ShipPerimeterDefenceModule,
            ShipSolarChargerModule,
            ShipSonarModule,
            ShipStorageModule,
            ShipTorpedoModule,
        };

        private void Awake()
        {
            Logger.LogInfo($"Plugin com.noahlavelle.seamoth is loaded!");
            Log = Logger;

            PatchPrefabs();
        }

        private static void PatchPrefabs()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.noahlavelle.seamoth");

            AssetBundle = AssetBundle.LoadFromFile(Path.Combine(ModFolderPath, "Assets", "seamoth"));

            var seamothUpgradesIcon = AssetBundle.LoadAsset<Sprite>("SeamothCraftIcon");
            CraftTreeHandler.AddTabNode(CraftTree.Type.Fabricator, "SeamothUpgrades", "Seamoth Upgrades", seamothUpgradesIcon, "Upgrades");

            var seamothPingIcon = AssetBundle.LoadAsset<Sprite>("SeamothPingIcon");
            SeamothPingType = PingHandler.RegisterNewPingType("Seamoth", seamothPingIcon);
            
            SeamothModuleType = EquipmentHandler.Main.AddEquipmentType("SeamothModule");
            
            SeamothPrefab = new ShipPrefab("Seamoth", "Seamoth", "One-person sea-and-space vehicle");
            SeamothPrefab.Patch();
            
            SeamothModules.ForEach(module => module.Patch());
        }
    }
}
