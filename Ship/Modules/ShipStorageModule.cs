using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipStorageModule : ShipModuleBase
{
    public ShipStorageModule() : base(
        "SeamothStorageModule",
        "Seamoth Storage Module",
        "SeamothStorageModule",
        "A small storage locker. Seamoth compatible"
    )
    {
        
    }

    protected override RecipeData GetBlueprintRecipe()
    {
        return new RecipeData
        {
            craftAmount = 1,
            Ingredients = new List<Ingredient>
            {
                new(TechType.Titanium, 3),
                new(TechType.Lithium, 1),
            }
        };
    }
    
    public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
    public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
    public override string[] StepsToFabricatorTab => new[] { "Upgrades", "SeamothUpgrades" };
    public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
}