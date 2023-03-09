using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipPerimeterDefenceModule : ShipModuleBase
{
    public ShipPerimeterDefenceModule() : base(
        new ShipModuleCraftType(CraftTree.Type.Fabricator, TechCategory.VehicleUpgrades, TechGroup.VehicleUpgrades),
        "SeamothPerimeterDefenceModule",
        "Seamoth Perimeter Defence System",
        "SeamothPerimeterDefenceModule",
        "Generates a localized electric field designed to ward off aggressive fauna."
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
                new(TechType.Polyaniline, 1),
                new(TechType.WiringKit, 1),
            }
        };
    }
}