﻿using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using VehicleFramework.VehicleComponents;

namespace VehicleFramework.Seamoth;

public class Seamoth : ModVehicle
{
    public Seamoth() : base("Seamoth", "Seamoth", "One-person sea-and-space vehicle", Plugin.AssetBundle.LoadAsset<GameObject>("SeamothPrefab.prefab"))
    {
        AddVehicleComponent(new VehicleLiveMixin(200, -1, true, true, 70, 0.2f, false));
        AddVehicleComponent(new VehicleEnergyManager());
        
        AddVehicleComponent(new VehiclePhysics());
        AddVehicleComponent(new VehicleLights());
        AddVehicleComponent(new VehicleConstructionVFX());
        AddVehicleComponent(new VehicleSeat(true));
        AddVehicleComponent(new VehicleEcoTarget());
        AddVehicleComponent(new VehiclePing(Plugin.AssetBundle.LoadAsset<Sprite>("SeamothPingIcon")));
        AddVehicleComponent(new VehicleCrushDepth(
            new VehicleCrushDepth.VehicleCrushDepthConfig(200f, 20f, 3f),
            "event:/sub/seamoth/crush_damage",
            "{905b4b2c-e1cc-4420-8040-bf45df04ce08}",
            "crush_damage",
            "event:/sub/seamoth/crush_depth_warning",
            "{26b49966-51f1-4336-a6bf-64cc2baf13d1}",
            "crush_depth_warning"
        ));
        
        AddVehicleComponent(
            new VehicleSounds(new VehicleVoiceLine(
                    "event:/sub/seamoth/welcome",
                "{d1db5c9b-4953-4ee7-ae98-fcc72347bf10}",
                "seamoth-welcome",
                "Seamoth: Welcome aboard, captain"
            )));
        
        AddVehicleComponent(new VehicleEngineSFX(
            "event:/sub/seamoth/seamoth_rev_up",
            "{66b9bbc2-44d6-44b4-9d5c-2af7ac64e05c}",
            "seamoth_rev_up",
            "event:/sub/seamoth/seamoth_loop_rpm",
            "{102faef9-1e51-45a3-9032-e9025cbeae3e}",
            "seamoth_loop_rpm"
        ));
        
        AddVehicleComponent(new VehicleCustomisation(
            new VehicleCustomisation.ColorData("Model/Vehicle_Anim/Exterior_geo", 0),
            new VehicleCustomisation.ColorData("Model/Vehicle_Anim/Exterior_geo", 1),
            new VehicleCustomisation.ColorData("Model/Vehicle_Anim/Interior_geo", 1),
            new VehicleCustomisation.ColorData("Model/Vehicle_Anim/Hatch_Exterior_geo", 0),
            new VehicleCustomisation.ColorData("Model/Vehicle_Anim/UpgradeSlot_Hatch_geo", 0),
            new VehicleCustomisation.ColorData("Model/Vehicle_Anim/Flap_L_geo", 0),
            new VehicleCustomisation.ColorData("Model/Vehicle_Anim/Flap_R_geo", 0),
            new VehicleCustomisation.ColorData("Model/Vehicle_Anim/Engine_PowerSlot_geo", 0)
        ));
        
        AddVehicleComponent(new VehicleDockable(
            new Vector3(0, 0, 0),
            Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_dock"),
            Plugin.AssetBundle.LoadAsset<AnimationClip>("loop_seamoth_docked"),
            Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_launch_left"),
            Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_launch_right"),
            Plugin.AssetBundle.LoadAsset<AnimationClip>("player_view_moon_seamoth_dock")
        ));

        var storageOpen = AssetManager.LoadFmodAsset(
            "event:/sub/seamoth/storage_open",
            "{b2821359-928c-4780-876a-0957de85f193}",
            "storage_open");
        var storageClose = AssetManager.LoadFmodAsset(
            "event:/sub/seamoth/storage_close",
            "{88d7834f-d050-470b-8ee5-1db7b7defa6f}",
            "storage_close");
        
        AddVehicleComponent(new VehicleStorage(4, 3, storageOpen, storageClose));
        AddVehicleComponent(new VehicleUpgradeConsole(storageOpen, storageClose));
    }

    public override TechCategory CategoryForPDA => TechCategory.Constructor;
    public override TechGroup GroupForPDA => TechGroup.Constructor;
    public override string[] StepsToFabricatorTab => null;
    public override CraftTree.Type FabricatorType => CraftTree.Type.Constructor;
    public override bool UnlockedAtStart => true;
    public override float CraftingTime => 9f;

    protected override void PrefabConstructionInitialised()
    {
        AddVehicleBehaviour<SeamothBehaviour>();
    }

    public override Sprite ItemSprite => Plugin.AssetBundle.LoadAsset<Sprite>("SeamothCraftIcon");
    public override RecipeData BlueprintRecipe => new()
    {
        craftAmount = 1,
        Ingredients =  new List<Ingredient>()
        {
            new(TechType.TitaniumIngot, 1),
            new(TechType.PowerCell, 1),
            new(TechType.Glass, 2),
            new(TechType.Lubricant, 1),
            new(TechType.Lead, 1)
        }
    };
}