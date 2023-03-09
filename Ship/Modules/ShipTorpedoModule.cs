using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipTorpedoModule : ShipModuleBase
{
    public ShipTorpedoModule() : base(
        new(CraftTree.Type.Fabricator, TechCategory.VehicleUpgrades, TechGroup.VehicleUpgrades),
        "SeamothTorpedoModule",
        "Seamoth Torpedo System",
        "SeamothTorpedoModule",
        "A standard underwater payload delivery system adapted to fire torpedoes."
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
                new(TechType.Aerogel, 1),
            }
        };
    }
}