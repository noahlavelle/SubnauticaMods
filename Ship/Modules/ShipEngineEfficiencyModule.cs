using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipEngineEfficiencyModule : ShipModuleBase
{
    public ShipEngineEfficiencyModule() : base(
        "SeamothEngineEfficiencyModule",
        "Seamoth Engine Efficiency Module",
        "SeamothEngineEfficiencyModule",
        "Recycles heat by-product to minimize power inefficiencies. Seamoth/Prawn compatible. Does stack."
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
                new(TechType.ComputerChip, 1),
                new(TechType.Polyaniline, 1)
            }
        };
    }
    
    public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
    public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
    public override string[] StepsToFabricatorTab => new[] { "Upgrades", "SeamothUpgrades" };
    public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
}