using System;
using System.Collections;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UnityEngine;

namespace VehicleFramework;

public class ModCraftable : Craftable
{
    public TechCategory categoryForPda;
    public TechGroup groupForPda;
    public string[] stepsToFabricatorTab;
    public CraftTree.Type fabricatorType;
    public RecipeData blueprintRecipe;
    public float craftingTime;
    public bool unlockedAtStart;
    public Sprite itemSprite;

    public Action<IOut<GameObject>> VehicleGameObject;
    
    public ModCraftable(string classId, string friendlyName, string description) : base(classId, friendlyName, description)
    {
    }

    protected override RecipeData GetBlueprintRecipe()
    {
        return blueprintRecipe;
    }

    protected override Sprite GetItemSprite()
    {
        return itemSprite;
    }

    public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
    {
        VehicleGameObject(gameObject);
         yield return null;
    }

    public override TechCategory CategoryForPDA => categoryForPda;
    public override TechGroup GroupForPDA => groupForPda;
    public override string[] StepsToFabricatorTab => stepsToFabricatorTab;
    public override CraftTree.Type FabricatorType => fabricatorType;
    public override bool UnlockedAtStart => unlockedAtStart;
    public override float CraftingTime => craftingTime;
}