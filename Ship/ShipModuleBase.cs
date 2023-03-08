using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace Seamoth.Ship;

public abstract class ShipModuleBase : Equipable
{
    private readonly string _assetName;
    public override string AssetsFolder => Path.Combine(Plugin.ModFolderPath, "Assets", "Modules");

    public ShipModuleBase(string classId, string friendlyName, string assetName, string description) : base(
        classId, friendlyName, description
    )
    {
        _assetName = assetName;
    }

    public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
    {
        var taskResult = new TaskResult<GameObject>();
        yield return CraftData.InstantiateFromPrefabAsync(TechType.SeaTruckUpgradeHull1, taskResult);
        var obj = taskResult.Get();

        var techTag = obj.GetComponent<TechTag>();
        var prefabIdentifier = obj.GetComponent<PrefabIdentifier>();

        techTag.type = TechType;
        prefabIdentifier.ClassId = ClassID;
        gameObject.Set(obj);
    }

    protected override Sprite GetItemSprite()
    {
        return Plugin.LoadSprite(Path.Combine(AssetsFolder, String.Concat(_assetName, ".png")));
    }
    
    public override EquipmentType EquipmentType => EquipmentType.VehicleModule;
    public override TechType RequiredForUnlock => Plugin.SeamothPrefab.TechType;
    public override bool UnlockedAtStart => true;
}