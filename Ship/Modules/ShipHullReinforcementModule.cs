using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipHullReinforcementModule : ShipModuleBase
{
    public ShipHullReinforcementModule() : base(
        new ShipModuleCraftType(CraftTree.Type.Fabricator, TechCategory.VehicleUpgrades, TechGroup.VehicleUpgrades),
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
}