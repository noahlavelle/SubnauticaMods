using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipSolarChargerModule : ShipModuleBase
{
    public ShipSolarChargerModule() : base(
        new ShipModuleCraftType(CraftTree.Type.Fabricator, TechCategory.VehicleUpgrades, TechGroup.VehicleUpgrades),
        "SeamothSolarChargerModule",
        "Seamoth Solar Charger",
        "SeamothSolarChargerModule",
        "Recharges the Seamoth's powercell while in sunlight."
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
                new(TechType.WiringKit, 1),
                new(TechType.EnameledGlass, 1),
            }
        };
    }
}