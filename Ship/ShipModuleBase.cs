using System.Collections;
using SMLHelper.V2.Assets;
using UnityEngine;

namespace Seamoth.Ship;

public abstract class ShipModuleBase : Equipable
{
    private readonly ShipModuleCraftType _craftType;
    private readonly string _assetName;

    protected ShipModuleBase(ShipModuleCraftType craftType, string classId, string friendlyName, string assetName, string description) : base(
        classId, friendlyName, description
    )
    {
        _craftType = craftType;
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
        return Plugin.AssetBundle.LoadAsset<Sprite>(_assetName);
    }
    
    public override EquipmentType EquipmentType => Plugin.SeamothModuleType;
    public override QuickSlotType QuickSlotType => QuickSlotType.Passive;
    public override TechType RequiredForUnlock => Plugin.SeamothPrefab.TechType;
    
    public override CraftTree.Type FabricatorType => _craftType.FabricatorType;
    public override TechCategory CategoryForPDA => _craftType.CategoryForPda;
    public override TechGroup GroupForPDA => _craftType.GroupForPda;
    public override string[] StepsToFabricatorTab
    {
        get
        {
            return _craftType.FabricatorType == CraftTree.Type.Fabricator 
                ? new[] {"Upgrades", "SeamothUpgrades"}
                : null;
        }
    }

    public override bool UnlockedAtStart => true;
}