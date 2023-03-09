using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipDepthModuleMk1 : ShipModuleBase
{
    public ShipDepthModuleMk1() : base(
        new ShipModuleCraftType(CraftTree.Type.Fabricator, TechCategory.VehicleUpgrades, TechGroup.VehicleUpgrades),
        "SeamothDepthModuleMK1",
        "Seamoth Depth Module MK1",
        "SeamothDepthModule",
        "Enhances safe diving depth by 100m. Does not stack."
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
                new(TechType.TitaniumIngot, 1),
                new(TechType.Glass, 2)
            }
        };
    }
}