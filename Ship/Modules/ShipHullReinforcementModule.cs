using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipHullReinforcementModule : ShipModuleBase
{
    public ShipHullReinforcementModule() : base(
        "SeamothHullReinforcementModule",
        "Seamoth Hull Reinforcement",
        "SeamothHullReinforcementModule",
        "Preemptively hardens the chassis before collision, eliminating damage under normal conditions. Seamoth/Prawn compatible."
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
                new(TechType.Diamond, 4)
            }
        };
    }
    
    public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
    public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
    public override string[] StepsToFabricatorTab => new[] { "Upgrades", "SeamothUpgrades" };
    public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
}